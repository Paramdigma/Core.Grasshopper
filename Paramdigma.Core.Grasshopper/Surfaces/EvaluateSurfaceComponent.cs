using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Paramdigma.Core.Grasshopper.Surfaces
{
    public class EvaluateSurfaceComponent : GH_Component
    {
        internal EvaluateSurfaceComponent() : base("Evaluate Surface", "EvSurf", "Evaluates a point on a surface",
            "Paramdigma", "Surfaces")
        {
        }

        public override Guid ComponentGuid { get; }

        protected override Bitmap Icon => null;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            throw new NotImplementedException();
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            throw new NotImplementedException();
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            throw new NotImplementedException();
        }
    }
}