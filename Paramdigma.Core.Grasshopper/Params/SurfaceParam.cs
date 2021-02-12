using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Paramdigma.Core.Conversion.Rhino;
using Rhino.Geometry;

namespace Paramdigma.Core.Grasshopper
{
    /// <summary>
    /// Grasshopper GH_Param encapsulating a MeshGHData object.
    /// </summary>
    public class SurfaceParam : GH_Param<SurfaceGhData>, IGH_PreviewObject
    {
        public SurfaceParam() : base(
            "Core Nurbs Surface",
            "Core Surf",
            "Paramdigma.Core Nurbs Surface.",
            "Paramdigma",
            "Params",
            GH_ParamAccess.item)
        {
        }

        public override Guid ComponentGuid =>
            new Guid("E1A5C6B2-5E1D-4DAD-97E8-14F2DDF4609B");

        public bool Hidden { get; set; }

        public bool IsPreviewCapable => true;

        public BoundingBox ClippingBox => clippingBox;

        private BoundingBox clippingBox = new BoundingBox();

        protected override Bitmap Icon => null;

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            clippingBox = new BoundingBox();
            foreach (var path in VolatileData.Paths)
            {
                var branch = VolatileData.get_Branch(path) as IList<SurfaceGhData>;
                foreach (var item in branch)
                {
                    var converted = item.Value.ToRhino();
                    args.Display.DrawBrepShaded(converted.ToBrep(), Attributes.Selected ? args.ShadeMaterial_Selected : args.ShadeMaterial);
                    clippingBox.Union(converted.GetBoundingBox(false));
                }
            }
        }

        public void DrawViewportWires(IGH_PreviewArgs args)
        {
            foreach (var path in VolatileData.Paths)
            {
                var branch = VolatileData.get_Branch(path) as IList<SurfaceGhData>;
                foreach (var item in branch)
                {
                    var converted = item.Value.ToRhino();
                    args.Display.DrawBrepWires(converted.ToBrep(), Attributes.Selected ? args.WireColour_Selected : args.WireColour);
                }
            }
        }
    }
}