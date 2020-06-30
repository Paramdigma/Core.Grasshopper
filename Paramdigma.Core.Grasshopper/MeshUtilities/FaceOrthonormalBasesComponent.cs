using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using PG = Paramdigma.Core.Geometry;
using Rhino.Geometry;

namespace Paramdigma.Core.Grasshopper.MeshNormals
{
    public class FaceOrthonormalBasesComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OrthonormalBasesComponent class.
        /// </summary>
        public FaceOrthonormalBasesComponent()
          : base("OrthonormalBasesComponent", "Ortho",
              "Computes the orthonormal bases on each face of a triangular mesh",
              "Paramdigma", "Mesh")
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("e1", "e1", "e1 vector", GH_ParamAccess.list);
            pManager.AddVectorParameter("e2", "e2", "e2 vector", GH_ParamAccess.list);
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

            List<Vector3d> meshE1 = new List<Vector3d>();
            List<Vector3d> meshE2 = new List<Vector3d>();

            foreach (Paramdigma.Core.HalfEdgeMesh.MeshFace face in hE_Mesh.Faces)
            {
                List<Vector3d> rhinoV = new List<Vector3d>();
                Paramdigma.Core.Geometry.Vector3d[] vects = PG.MeshGeometry.OrthonormalBases(face);
                meshE1.Add(new Vector3d(vects[0].X, vects[0].Y, vects[0].Z));
                meshE2.Add(new Vector3d(vects[1].X, vects[1].Y, vects[1].Z));    
            }

            DA.SetDataList(0, meshE1);
            DA.SetDataList(1, meshE2);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.FaceOrthonormalBases;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9b5feeba-913f-43f7-8cc7-9b5694b1eac9");
    }
}