using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Paramdigma.Core.Conversion.Rhino;

namespace Paramdigma.Core.Grasshopper.Surfaces
{
    public class NurbsSurfaceByPointsWeightComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public NurbsSurfaceByPointsWeightComponent()
          : base("NurbsSurfaceByPointsWeightComponent", "Nickname",
            "NurbsSurfaceByPointsWeightComponent description",
            "Paramdigma", "Surfaces")
        {
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Control Points", "cPts", "Control Points", GH_ParamAccess.list);
            pManager.AddNumberParameter("Weights", "w", "Control point weights", GH_ParamAccess.list);
            pManager.AddIntegerParameter("uCount", "uC", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("degreeU", "dU", "Degree in the U direction", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("degreeV", "dV", "Degree in the V direction", GH_ParamAccess.item, 3);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new SurfaceParam());
        }

        Geometry.NurbsSurface surface = null;

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Placeholder props
            var points = new List<Point3d>();
            var weights = new List<double>();
            var degreeU = 0;
            var degreeV = 0;
            var countU = 0;

            bool invalidData = false;
            // Get the input data
            if (!DA.GetDataList(0, points)) invalidData = true;
            if (!DA.GetDataList(1, weights)) invalidData = true;
            if (!DA.GetData(2, ref countU)) invalidData = true;
            if (!DA.GetData(3, ref degreeU)) invalidData = true;
            if (!DA.GetData(4, ref degreeV)) invalidData = true;

            if (invalidData)
            {
                surface = null;
                return;
            }

            // Validate inputs
            if (points.Count % countU != 0)
            {
                surface = null;
                throw new Exception("Points length has to be divisible by countU");
            }
            if (weights.Count % countU != 0)
            {
                surface = null;
                throw new Exception("Weights length has to be divisible by countU");
            }

            var countV = points.Count / countU;

            // Construct control point matrix
            var controlPoints = new Collections.Matrix<Geometry.Point4d>(countU, countV);
            for (int i = 0; i < countU; i++)
            {
                for (int j = 0; j < countV; j++)
                {
                    var cPt = new Geometry.Point4d(points[i * countU + j].ToCore(), weights[i * countU + j]);
                    controlPoints[i, j] = cPt;
                }
            }

            // Create surface
            surface = new Geometry.NurbsSurface(controlPoints, degreeU, degreeV);

            // Assign output data
            DA.SetData(0, new SurfaceGhData(surface));
        }


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("0316c6bb-3544-4284-accc-e7ba8394ff84");
    }
}
