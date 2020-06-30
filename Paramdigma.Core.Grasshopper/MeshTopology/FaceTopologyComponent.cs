using System;
using GH = Grasshopper;

namespace Paramdigma.Core.Grasshopper
{
    public class FaceTopologyComponent : GH.Kernel.GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FaceTopology class.
        /// </summary>
        public FaceTopologyComponent()
          : base("Face Topology", "Face Topo",
              "Computes the face adjacency data for the given mesh",
              "Paramdigma", "Topology")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH.Kernel.GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new MeshParam(), "Half-Edge Mesh", "hE", "Half-Edge Mesh", GH.Kernel.GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH.Kernel.GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("FV", "FV", "FV", GH.Kernel.GH_ParamAccess.tree);
            pManager.AddIntegerParameter("FE", "FE", "FE", GH.Kernel.GH_ParamAccess.tree);
            pManager.AddIntegerParameter("FF", "FF", "FF", GH.Kernel.GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(GH.Kernel.IGH_DataAccess DA)
        {
            MeshGhData hE_MeshData = new MeshGhData();

            if (!DA.GetData(0, ref hE_MeshData)) return;

            Paramdigma.Core.HalfEdgeMesh.Mesh hE_Mesh = hE_MeshData.Value;

            Paramdigma.Core.HalfEdgeMesh.MeshTopology topo = new Paramdigma.Core.HalfEdgeMesh.MeshTopology(hE_Mesh);

            topo.ComputeFaceAdjacency();

            GH.DataTree<int> fvTopo = new GH.DataTree<int>();
            GH.DataTree<int> feTopo = new GH.DataTree<int>();
            GH.DataTree<int> ffTopo = new GH.DataTree<int>();

            foreach (int key in topo.FaceVertex.Keys)
            {
                fvTopo.AddRange(topo.FaceVertex[key], new GH.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.FaceVertex.Keys)
            {
                feTopo.AddRange(topo.FaceVertex[key], new GH.Kernel.Data.GH_Path(key));
            }
            foreach (int key in topo.FaceFace.Keys)
            {
                ffTopo.AddRange(topo.FaceFace[key], new GH.Kernel.Data.GH_Path(key));
            }

            DA.SetDataTree(0, fvTopo);
            DA.SetDataTree(1, feTopo);
            DA.SetDataTree(2, ffTopo);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.TopologyFace;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9044ea40-e6d9-4a9f-97ba-fb85bf5ed429");
    }
}