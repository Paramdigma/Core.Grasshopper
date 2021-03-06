﻿using System;
using System.Collections.Generic;
using Paramdigma.Core.HalfEdgeMesh;
using Grasshopper.Kernel;
using Grasshopper;
using GH = Grasshopper;
using Rhino.Geometry;

namespace Paramdigma.Core.Grasshopper.MeshCurves
{
    public class LevelSetsComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the LevelSetsComponent class.
        /// </summary>
        public LevelSetsComponent()
          : base("Mesh Level Sets", "Level Sets",
              "Computes the level sets given a list of numeric values for each vertex of the mesh and a step size for the level set calculation.",
              "Paramdigma", "Curves")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scalar values", "V", "List of numerical values to place on each vertex of the mesh", GH_ParamAccess.list);
            pManager.AddNumberParameter("Levels", "L", "List of levels to compute", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Level Sets", "L", "Level set curves on the mesh", GH_ParamAccess.tree);
        }
        
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MeshGhData hE_MeshData = new MeshGhData();
            List<double> scalarValues = new List<double>();
            List<double> levels = new List<double>();
            string key = "sets1";

            if (!DA.GetData(0, ref hE_MeshData)) return;
            if (!DA.GetDataList(1, scalarValues)) return;
            if (!DA.GetDataList(2, levels)) return;

            Paramdigma.Core.HalfEdgeMesh.Mesh hE_Mesh = hE_MeshData.Value;

            // Check for invalid inputs
            if (!hE_Mesh.IsTriangularMesh())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh is not triangular!");
                return;
            }
            if (levels.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Level count can't be 0!");
                return;
            }
            if (scalarValues.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Value count can't be 0!");
                return;

            }
            // Assign values to mesh vertices
            foreach (MeshVertex v in hE_Mesh.Vertices)
            {   
                if (v.UserValues.ContainsKey(key)) v.UserValues[key] = scalarValues[v.Index];
                else v.UserValues.Add(key, scalarValues[v.Index]); 
            }

            // Compute level sets
            Paramdigma.Core.Curves.LevelSets.ComputeLevels(key, levels, hE_Mesh, out List<List<Paramdigma.Core.Geometry.Line>> levelLines);

            //Convert AR_Lib.Curve.Line to Rhino.Geometry.Line
            DataTree<Line> resultLevel = new DataTree<Line>();
            int count = 0;
            foreach(List<Paramdigma.Core.Geometry.Line> tempList in levelLines)
            {
                List<Line> tempResult = new List<Line>();
                foreach (Paramdigma.Core.Geometry.Line l in tempList)
                {
                    tempResult.Add(new Line(new Point3d(l.StartPoint.X, l.StartPoint.Y, l.StartPoint.Z), new Point3d(l.EndPoint.X, l.EndPoint.Y, l.EndPoint.Z)));
                }
                resultLevel.AddRange(tempResult, new GH.Kernel.Data.GH_Path(count));
                count++;
            }

            // Return list of lines
            DA.SetDataTree(0, resultLevel);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.LevelSets;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("6f7b2c13-d5da-4932-97c0-cf751bfec6ef");
    }
}