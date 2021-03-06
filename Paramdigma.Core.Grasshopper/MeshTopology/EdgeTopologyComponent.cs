﻿using System;
using GH = Grasshopper;
using Grasshopper.Kernel;

namespace Paramdigma.Core.Grasshopper.MeshTopology
{
    public class EdgeTopologyComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the EdgeTopology class.
        /// </summary>
        public EdgeTopologyComponent()
            : base("Edge Topology", "Edge Topo",
                "Computes the edge adjacency data for the given mesh",
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
            pManager.AddIntegerParameter("EV", "EV", "EV", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("EE", "EE", "EE", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("EF", "EF", "EF", GH_ParamAccess.tree);
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

            topo.ComputeEdgeAdjacency();

            GH.DataTree<int> evTopo = new GH.DataTree<int>();
            GH.DataTree<int> efTopo = new GH.DataTree<int>();
            GH.DataTree<int> eeTopo = new GH.DataTree<int>();

            foreach (int key in topo.EdgeVertex.Keys)
            {
                evTopo.AddRange(topo.EdgeVertex[key], new GH.Kernel.Data.GH_Path(key));
            }

            foreach (int key in topo.EdgeEdge.Keys)
            {
                eeTopo.AddRange(topo.EdgeEdge[key], new GH.Kernel.Data.GH_Path(key));
            }

            foreach (int key in topo.EdgeFace.Keys)
            {
                efTopo.AddRange(topo.EdgeFace[key], new GH.Kernel.Data.GH_Path(key));
            }

            DA.SetDataTree(0, evTopo);
            DA.SetDataTree(1, eeTopo);
            DA.SetDataTree(2, efTopo);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.TopologyEdge;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("a06f9af2-a97a-4bf0-a363-620c76118c27");
    }
}