using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
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
            pManager.AddParameter(new MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH_ParamAccess.item);
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
            var hE_MeshData = new MeshGhData();

            if (!DA.GetData(0, ref hE_MeshData)) return;

            var hE_Mesh = hE_MeshData.Value;

            var faces = new List<Rhino.Geometry.Mesh>();

            var vertices = hE_Mesh.Vertices.Select(v => new Point3d(v.X, v.Y, v.Z)).ToList();
            var edges = (from e in hE_Mesh.Edges let v1 = e.HalfEdge.Vertex let v2 = e.HalfEdge.Twin.Vertex select new Line(new Point3d(v1.X, v1.Y, v1.Z), new Point3d(v2.X, v2.Y, v2.Z))).ToList();
            foreach (var f in  hE_Mesh.Faces)
            {
                var vs = f.AdjacentVertices();
            
                var faceVs = new List<int>();
                var facePoints = new List<Point3d>();

                var vi = 0;

                foreach(var v in vs)
                {
                    facePoints.Add(new Point3d(v.X, v.Y, v.Z));
                    faceVs.Add(vi);
                    vi++;
                }

                var m = new Mesh();
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
        protected override System.Drawing.Bitmap Icon => Properties.Resources.TopologyVEF;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("b72a840e-ee01-4292-bb39-149e830fd3ab");
    }  
}