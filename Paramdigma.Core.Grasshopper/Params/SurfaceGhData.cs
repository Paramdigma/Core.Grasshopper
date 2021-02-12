using Grasshopper.Kernel.Types;
using Paramdigma.Core.Geometry;
using RG = Rhino.Geometry;
using Paramdigma.Core.Conversion.Rhino;

namespace Paramdigma.Core.Grasshopper
{
    public sealed class SurfaceGhData : GH_Goo<NurbsSurface>
    {
        /// <summary>
        /// Constructs an empty SurfaceGHData object
        /// </summary>
        public SurfaceGhData()
        {
            Value = null;
        }

        /// <summary>
        /// Constructs a SurfaceGHData object from a Rhino.Geometry.NurbsSurface
        /// </summary>
        /// <param name="surface">A Rhino NurbsSurface</param>
        public SurfaceGhData(RG.NurbsSurface surface)
        {
            // TODO: Handle conversion.
            Value = null;
        }

        /// <summary>
        /// Constructs a SurfaceGHData object from an Paramdigma.Core NurbsSurface
        /// </summary>
        /// <param name="surface">A Nurbs Surface</param>
        public SurfaceGhData(NurbsSurface surface)
        {
            Value = surface;
        }

        /// <summary>
        /// Returns true if object is valid.
        /// TODO: Currently it ALWAYS returns true.
        /// </summary>
        public override bool IsValid => true;

        public override string TypeName => "Nurbs Surface";

        public override string TypeDescription => "Nurbs Surface structure from Paramdigma.Core";

        public override IGH_Goo Duplicate() => new SurfaceGhData(Value);

        public override string ToString() => "Core NurbsSurface";

        public override object ScriptVariable() => Value;

        public override bool CastFrom(object source)
        {
            var surface = (source as GH_Surface)?.Value;
            if (surface == null || !surface.IsSurface)
                return false;
            Value = surface.Surfaces[0].ToNurbsSurface().ToCore();
            return true;
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (!(target is GH_Surface) || Value == null)
                return false;
            
            target = (Q) (object) new GH_Surface(Value.ToRhino());
            return true;
        }
    }
}