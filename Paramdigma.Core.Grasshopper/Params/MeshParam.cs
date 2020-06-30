using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Paramdigma.Core.Grasshopper
{
    /// <summary>
    /// Grasshopper GH_Param encapsulating a MeshGHData object.
    /// </summary>
    public class MeshParam : GH_Param<MeshGhData>
    {
        public MeshParam() : base(
            "Half-edge mesh",
            "HE Mesh",
            "Half-edge mesh",
            "Paramdigma",
            "Params",
            GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("{d397c3bc-5c0c-4435-88a6-bbc988b1e6ca}");

        protected override Bitmap Icon => Properties.Resources.Param_HalfEdgeMesh;
    }
}