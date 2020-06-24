using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Paramdigma.Core.Grasshopper
{
    public class MeshGHData : GH_Goo<HalfEdgeMesh.Mesh>
    {
        /// <summary>
        /// Constructs an empty MeshGHData object
        /// </summary>
        public MeshGHData()
        {
            this.Value = null;
        }

        /// <summary>
        /// Constructs a MeshGHData object from a Rhino.Geometry.Mesh
        /// </summary>
        /// <param name="mesh">A Rhino Mesh</param>
        public MeshGHData(Rhino.Geometry.Mesh mesh)
        {
            Paramdigma.Core.HalfEdgeMesh.Mesh hE;
            RhinoConvert.FromRhinoMesh(mesh, out hE);
            this.Value = hE;
        }

        /// <summary>
        /// Constructs a MeshGHData object from an AR_Lib HalfEdgeMesh
        /// </summary>
        /// <param name="hE_mesh">A Half-Edge Mesh</param>
        public MeshGHData(HalfEdgeMesh.Mesh hE_mesh)
        {
            this.Value = hE_mesh;
        }

        /// <summary>
        /// Returns true if object is valid.
        /// TODO: Currently it ALWAYS returns true.
        /// </summary>
        public override bool IsValid => true;

        public override string TypeName => "Half-Edge Mesh";

        public override string TypeDescription => "Half-edge Mesh data structure";

        public override IGH_Goo Duplicate() => new MeshGHData(Value);

        public override string ToString() => "Half-Edge Mesh";

        public override object ScriptVariable()
        {
            return Value;
        }

        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }
            if (source is GH_Mesh)
            {
                Rhino.Geometry.Mesh mesh = (source as GH_Mesh).Value;
                if (mesh != null)
                {
                    if (RhinoConvert.FromRhinoMesh(mesh, out HalfEdgeMesh.Mesh hE) == RhinoConvert.RhinoMeshResult.OK)
                    {
                        Value = hE;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
        public override bool CastTo<Q>(ref Q target)
        {
            if (target is GH_Mesh)
            {
                Rhino.Geometry.Mesh mesh;
                RhinoConvert.RhinoMeshResult result = RhinoConvert.ToRhinoMesh(Value, out mesh);
                object obj = new GH_Mesh(mesh);
                if(result == RhinoConvert.RhinoMeshResult.OK)
                {
                    target = (Q)obj;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Grasshopper GH_Param encapsulating a MeshGHData object.
    /// </summary>
    public class MeshParam: GH_Param<MeshGHData>
    {
        // We need to supply a constructor without arguments that calls the base class constructor.
        public MeshParam() : base("Half-edge mesh", "HE Mesh", "Half-edge mesh", "AR_Lib", "Params", GH_ParamAccess.item)
        {

        }

        public override System.Guid ComponentGuid
        {
            // Always generate a new Guid, but never change it once
            // you've released this parameter to the public.
            get { return new Guid("{d397c3bc-5c0c-4435-88a6-bbc988b1e6ca}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                //return Properties.Resources.AR_Lib_Param_HalfEdgeMesh;
                return null;
            }
        }

    }
}
