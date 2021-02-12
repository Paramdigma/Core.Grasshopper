using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Paramdigma.Core.Conversion.Rhino;

namespace Paramdigma.Core.Grasshopper.Surfaces
{
    public class EvaluateSurfaceComponent : GH_Component
    {

        public EvaluateSurfaceComponent()
          : base("EvaluateSurfaceComponent", "evSurf",
            "Evaluates a point on a surface",
            "Paramdigma", "Surfaces")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new SurfaceParam());
            pManager.AddPointParameter("UV Point", "uv", "2D point on UV space", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Pt", "Point", GH_ParamAccess.item);
            pManager.AddVectorParameter("Normal", "N", "Normal", GH_ParamAccess.item);
            pManager.AddVectorParameter("uDir", "U", "U direction", GH_ParamAccess.item);
            pManager.AddVectorParameter("vDir", "V", "V direction", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Frame", "F", "Frame", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            SurfaceGhData surfData = null;
            Point3d uv = Point3d.Unset;
            if (!DA.GetData(0, ref surfData)) return;
            if (!DA.GetData(1, ref uv)) return;

            var surf = surfData.Value;

            if (uv.Z != 0)
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "UV point was not 2D. Z value was ignored.");

            var pt = surf.PointAt(uv.X, uv.Y);
            //var n = surf.NormalAt(uv.X, uv.Y);
            //var f = surf.FrameAt(uv.X, uv.Y);

            var ders = surf.DerivativesAt(uv.X, uv.Y, 2);
            var u = ders[1, 0];
            var v = ders[0, 1];
            DA.SetData(0, pt.ToRhino());
            DA.SetData(1, u.Cross(v).ToRhino());
            DA.SetData(2, u.ToRhino());
            DA.SetData(3, v.ToRhino());
            DA.SetData(4, new Geometry.Plane(pt,u,v).ToRhino());
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("54dd7866-094c-4da2-ad98-5fbbd25247fa");
    }
}
