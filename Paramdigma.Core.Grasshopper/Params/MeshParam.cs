using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.Collections.Generic;
using Paramdigma.Core.Grasshopper.Converter;

namespace Paramdigma.Core.Grasshopper
{
    /// <summary>
    /// Grasshopper GH_Param encapsulating a MeshGHData object.
    /// </summary>
    public class MeshParam : GH_Param<MeshGhData>, IGH_PreviewObject
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

        public BoundingBox ClippingBox => clippingBox;

        private BoundingBox clippingBox = new BoundingBox();

        public bool Hidden { get; set; }

        public bool IsPreviewCapable => true;

        protected override Bitmap Icon => Properties.Resources.Param_HalfEdgeMesh;

        public void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            clippingBox = new BoundingBox();
            foreach (var path in VolatileData.Paths)
            {
                var branch = VolatileData.get_Branch(path) as IList<MeshGhData>;
                foreach (var item in branch)
                {
                    var converted = item.Value.ToRhino();
                    args.Display.DrawMeshShaded(converted, Attributes.Selected ? args.ShadeMaterial_Selected : args.ShadeMaterial);
                    clippingBox.Union(converted.GetBoundingBox(false));
                }
            }
        }


        public void DrawViewportWires(IGH_PreviewArgs args)
        {
         
            foreach (var path in VolatileData.Paths)
            {
                var branch = VolatileData.get_Branch(path) as IList<MeshGhData>;
                foreach (var item in branch)
                {
                    var mesh = item.Value;
                    foreach (var edge in mesh.Edges)
                    {
                        var vertices = edge.AdjacentVertices();
                        var line = new Line(vertices[0].ToRhino(), vertices[1].ToRhino());
                        args.Display.DrawLine(line,Attributes.Selected ? args.WireColour_Selected: args.WireColour);
                        clippingBox.Union(line.BoundingBox);
                    }
                }
            }
        }
    }
}