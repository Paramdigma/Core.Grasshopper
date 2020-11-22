using System;
using GH = Grasshopper;
using Grasshopper.Kernel;

namespace Paramdigma.Core.Grasshopper.MeshTopology
{
    public class VertexTopologyComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VertexTopology class.
        /// </summary>
        public VertexTopologyComponent()
          : base("Vertex Topology", "Vert. Topo",
              "Computes the vertex adjacency data for the given mesh.",
              "Paramdigma", "Topology")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("VV", "VV", "VV", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("VE", "VE", "VE", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("VF", "VF", "VF", GH_ParamAccess.tree);

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

            Paramdigma.Core.HalfEdgeMesh.MeshTopology topo = new Paramdigma.Core.HalfEdgeMesh.MeshTopology(hE_Mesh);

            topo.ComputeVertexAdjacency();

            GH.DataTree<int> vvTopo = new GH.DataTree<int>();
            GH.DataTree<int> veTopo = new GH.DataTree<int>();
            GH.DataTree<int> vfTopo = new GH.DataTree<int>();

            foreach (int key in topo.VertexVertex.Keys)
            {
                vvTopo.AddRange(topo.VertexVertex[key], new GH.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.VertexEdges.Keys)
            {
                veTopo.AddRange(topo.VertexEdges[key], new GH.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.VertexFaces.Keys)
            {
                vfTopo.AddRange(topo.VertexFaces[key], new GH.Kernel.Data.GH_Path(key));
            }

            DA.SetDataTree(0, vvTopo);
            DA.SetDataTree(1, veTopo);
            DA.SetDataTree(2, vfTopo);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.TopologyVertex;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("77b54c47-084b-4174-a761-969b6a699581");
    }
}