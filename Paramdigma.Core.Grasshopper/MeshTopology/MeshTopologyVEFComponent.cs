using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Paramdigma.Core.HalfEdgeMesh;
using Rhino.Geometry;

namespace Paramdigma.Core.Grasshopper.MeshTopology
{
    public class MeshTopologyVEFComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshTopologyVEFComponent class.
        /// </summary>
        public MeshTopologyVEFComponent()
          : base("Half-Edge Mesh VEF", "V E F",
              "Returns the vertices edges and faces of a Half-Edge Mesh",
              "Paramdigma", "Topology")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new HE_MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Vertices", "V", "List of vertices of the mesh", GH_ParamAccess.list);
            pManager.AddLineParameter("Edges", "E", "List of edges of the mesh", GH_ParamAccess.list);
            pManager.AddMeshParameter("Faces", "F", "List of meshes representing each face of the mesh", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MeshGHData hE_MeshData = new MeshGHData();

            if (!DA.GetData(0, ref hE_MeshData)) return;

            Paramdigma.Core.HalfEdgeMesh.Mesh hE_Mesh = hE_MeshData.Value;

            List<Point3d> vertices = new List<Point3d>();
            List<Line> edges = new List<Line>();
            List<Rhino.Geometry.Mesh> faces = new List<Rhino.Geometry.Mesh>();

            foreach(MeshVertex v in hE_Mesh.Vertices)
            {
                vertices.Add(new Point3d(v.X, v.Y, v.Z));
            }
            foreach (MeshEdge e in hE_Mesh.Edges)
            {
                MeshVertex v1 = e.HalfEdge.Vertex;
                MeshVertex v2 = e.HalfEdge.Twin.Vertex;

                edges.Add(new Line(new Point3d(v1.X, v1.Y, v1.Z), new Point3d(v2.X, v2.Y, v2.Z)));
            }
            foreach (Paramdigma.Core.HalfEdgeMesh.MeshFace f in  hE_Mesh.Faces)
            {
                List<MeshVertex> vs = f.AdjacentVertices();
            
                List<int> faceVs = new List<int>();
                List<Point3d> facePoints = new List<Point3d>();

                int vi = 0;

                foreach(MeshVertex v in vs)
                {
                    facePoints.Add(new Point3d(v.X, v.Y, v.Z));
                    faceVs.Add(vi);
                    vi++;
                }

                Rhino.Geometry.Mesh m = new Rhino.Geometry.Mesh();
                m.Vertices.AddVertices(facePoints);

                if (vs.Count == 3) m.Faces.AddFace(0, 1, 2);
                else if (vs.Count == 4) m.Faces.AddFace(0, 1, 2, 3);

                faces.Add(m);
            }

            DA.SetDataList(0, vertices);
            DA.SetDataList(1, edges);
            DA.SetDataList(2, faces);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                //return Properties.Resources.AR_Lib_TopologyVEF;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b72a840e-ee01-4292-bb39-149e830fd3ab"); }
        }
    }  
}