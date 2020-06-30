using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Paramdigma.Core.HalfEdgeMesh;
using Rhino.Geometry;

namespace Paramdigma.Core.Grasshopper.MeshCurves
{
    public class MeshGradientComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshGradientComponent class.
        /// </summary>
        public MeshGradientComponent()
          : base("Mesh Gradient", "Gradient",
              "Mesh Gradient",
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
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Gradient", "G", "Gradient of the given scalar function", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MeshGhData hE_MeshData = new MeshGhData();
            List<double> scalarValues = new List<double>();
            string key = "sets1";

            if (!DA.GetData(0, ref hE_MeshData)) return;
            if (!DA.GetDataList(1, scalarValues)) return;

            HalfEdgeMesh.Mesh hE_Mesh = hE_MeshData.Value;

            // Check for invalid inputs
            if (!hE_Mesh.IsTriangularMesh())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh is not triangular!");
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

            List<Paramdigma.Core.Geometry.Vector3d> arVectors = Paramdigma.Core.Curves.LevelSets.ComputeGradientField(key, hE_Mesh);
            List<Vector3d> rhinoVectors = new List<Vector3d>();
            arVectors.ForEach(arVect => rhinoVectors.Add(new Vector3d(arVect.X, arVect.Y, arVect.Z)));

            DA.SetDataList(0, rhinoVectors);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.LevelGrad;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("ca57a248-054f-41f7-b7d2-d316d6db748a");
    }
}