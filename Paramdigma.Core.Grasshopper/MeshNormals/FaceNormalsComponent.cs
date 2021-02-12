using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Paramdigma.Core.Grasshopper.MeshNormals
{
    public class FaceNormalsComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FaceNormalsComponent class.
        /// </summary>
        public FaceNormalsComponent()
          : base("Face Normals", "F Norm",
              "Computes the face normals of a given triangular mesh.",
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
            pManager.AddVectorParameter("Face Normals", "N", "The computed face normals",GH_ParamAccess.list);
            pManager.AddPointParameter("Centroids", "C", "Face centroids", GH_ParamAccess.list);
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

            List<Vector3d> normals = new List<Vector3d>();
            List<Point3d> centroids = new List<Point3d>();
            List<Point3d> circumcenters = new List<Point3d>();


            foreach (Geometry.MeshFace face in hE_Mesh.Faces)
            {
                Paramdigma.Core.Geometry.Vector3d v = Paramdigma.Core.Geometry.MeshGeometry.FaceNormal(face);
                Paramdigma.Core.Geometry.Point3d centroid = Paramdigma.Core.Geometry.MeshGeometry.Centroid(face);

                normals.Add(new Vector3d(v.X, v.Y, v.Z));
                centroids.Add(new Point3d(centroid.X, centroid.Y, centroid.Z));

            }

            DA.SetDataList(0, normals);
            DA.SetDataList(1, centroids);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.FaceNormals;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("5d7430f8-3718-4e7d-8f06-75f0c4e67333");
    }
}