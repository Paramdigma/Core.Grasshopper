using Grasshopper.Kernel.Types;
using Paramdigma.Core.HalfEdgeMesh;

namespace Paramdigma.Core.Grasshopper
{
    public sealed class MeshGhData : GH_Goo<Mesh>
    {
        /// <summary>
        /// Constructs an empty MeshGHData object
        /// </summary>
        public MeshGhData()
        {
            Value = null;
        }

        /// <summary>
        /// Constructs a MeshGHData object from a Rhino.Geometry.Mesh
        /// </summary>
        /// <param name="mesh">A Rhino Mesh</param>
        public MeshGhData(Rhino.Geometry.Mesh mesh)
        {
            RhinoConvert.FromRhinoMesh(mesh, out Mesh coreMesh);
            Value = coreMesh;
        }

        /// <summary>
        /// Constructs a MeshGHData object from an AR_Lib HalfEdgeMesh
        /// </summary>
        /// <param name="mesh">A Half-Edge Mesh</param>
        public MeshGhData(Mesh mesh)
        {
            Value = mesh;
        }

        /// <summary>
        /// Returns true if object is valid.
        /// TODO: Currently it ALWAYS returns true.
        /// </summary>
        public override bool IsValid => true;

        public override string TypeName => "Half-Edge Mesh";

        public override string TypeDescription => "Half-edge Mesh data structure";

        public override IGH_Goo Duplicate() => new MeshGhData(Value);

        public override string ToString() => "Half-Edge Mesh";

        public override object ScriptVariable() => Value;

        public override bool CastFrom(object source)
        {
            Rhino.Geometry.Mesh mesh = (source as GH_Mesh)?.Value;
            if (mesh == null)
                return false;
            if (RhinoConvert.FromRhinoMesh(mesh, out Mesh hE) != RhinoConvert.RhinoMeshResult.OK)
                return false;
            Value = hE;
            return true;
        }

        public override bool CastTo<Q>(ref Q target)
        {
            if (!(target is GH_Mesh))
                return false;
            var result = RhinoConvert.ToRhinoMesh(Value, out var mesh);
            if (result != RhinoConvert.RhinoMeshResult.OK)
                return false;
            target = (Q) (object) new GH_Mesh(mesh);
            return true;
        }
    }
}