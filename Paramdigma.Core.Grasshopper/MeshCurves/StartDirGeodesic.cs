﻿using System;
using Grasshopper.Kernel;

namespace Paramdigma.Core.Grasshopper.MeshCurves
{
    public class StartDirGeodesic : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the StartDirGeodesic class.
        /// </summary>
        public StartDirGeodesic()
          : base("Geodesic P/V", "P/V geod",
              "Computes a geodesic curve on a triangular mesh given a starting point and a direction vector.",
              "Paramdigma", "Curves")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "Pt", "Startting point for the geodesic",GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Dir", "Starting direction for the geodesic", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Geodesic", "G", "Computed geodesic curve on the mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MeshGhData hE_MeshData = new MeshGhData();

            if (!DA.GetData(0, ref hE_MeshData)) return;

            Paramdigma.Core.HalfEdgeMesh.Mesh hE_Mesh = hE_MeshData.Value;

            if (!hE_Mesh.IsTriangularMesh())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh is not triangular!");
                return;
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.GeodesicStartDir;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("13df0941-dd3f-49ff-a0d4-951839c886e5");
    }
}