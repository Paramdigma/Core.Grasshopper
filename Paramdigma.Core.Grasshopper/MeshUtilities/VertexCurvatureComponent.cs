using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Paramdigma.Core.Grasshopper.MeshUtilities
{
    public class VertexCurvatureComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshCurvatureComponent class.
        /// </summary>
        public VertexCurvatureComponent()
          : base("Vertex Curvature", "Curvature",
              "Computes the scalar and vector curvature of a given triangular mesh",
              "Paramdigma", "Mesh")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new MeshParam(),"Half-Edge Mesh", "hE", "Half-Edge Mesh",GH_ParamAccess.item );
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("k1", "k1", "k1 scalar value of curvature", GH_ParamAccess.list);
            pManager.AddNumberParameter("k2", "k2", "k2 scalar value of curvature", GH_ParamAccess.list);
            pManager.AddNumberParameter("K", "K", "Gaussian curvature", GH_ParamAccess.list);
            pManager.AddNumberParameter("Km", "Km", "Mean curvature", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            MeshGhData hE_MeshData = new MeshGhData();

            if (!DA.GetData(0, ref hE_MeshData)) return;

            Geometry.Mesh hE_Mesh = hE_MeshData.Value;

            if (!hE_Mesh.IsTriangularMesh())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Mesh is not triangular!");
                return;
            }

            List<double> k1 = new List<double>();
            List<double> k2 = new List<double>();
            List<double> K = new List<double>();
            List<double> km = new List<double>();

            foreach (Geometry.MeshVertex v in hE_Mesh.Vertices)
            {
                double[] k = Paramdigma.Core.Geometry.MeshGeometry.PrincipalCurvatures(v);
                k1.Add(k[0]);
                k2.Add(k[1]);
                K.Add(Paramdigma.Core.Geometry.MeshGeometry.ScalarGaussCurvature(v));
                km.Add(Paramdigma.Core.Geometry.MeshGeometry.ScalarMeanCurvature(v));
            }

            DA.SetDataList(0, k1);
            DA.SetDataList(1, k2);
            DA.SetDataList(2, K);
            DA.SetDataList(3, km);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.VertexCurvature;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("714d95a4-fde5-43dd-b4ac-7d600fd008de");
    }
}