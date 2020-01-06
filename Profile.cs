using System;
using System.Collections.Generic;
using BensonGH.Properties;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace BensonGH
{
    public class Profile : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Profile class.
        /// </summary>
        public Profile()
          : base("Profile", "Pr",
              "Create a profile swipe through curves",
              "BensonGHC", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("iCurve", "C", "Input curve to use as rail", GH_ParamAccess.item);
            pManager.AddCurveParameter("iProfile", "P", "Optional Input curve to use as Section profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("iX", "x","dimension of default rectanglar profile in x", GH_ParamAccess.item, 0.2);
            pManager.AddNumberParameter("iY", "y", "dimension of default rectanglar profile in x", GH_ParamAccess.item, 0.2);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("oBreps", "B", "Output geometry in form of breps", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve iRail = null;
            Curve iProfile = null;
            double iX = 0.0;
            double iY = 0.0;

            if (!DA.GetData(0, ref iRail)) return;
            if (!DA.GetData(0, ref iProfile)) return;
            if (!DA.GetData(1, ref iX)) return;
            if (!DA.GetData(2, ref iY)) return;

            Plane pln = new Plane();
            iRail.PerpendicularFrameAt(0.0, out pln);
            var rect = new Rectangle3d(pln, iX, iY);

            Plane Orig = new Plane(0, 0, 1, 0);
            var xform1 = Rhino.Geometry.Transform.PlaneToPlane(Orig, pln);


            var rectC = rect.ToNurbsCurve();

            if (iProfile != null)
            {
                iProfile.Transform(xform1);
                rectC = iProfile.ToNurbsCurve();
            }

            var sweep = new Rhino.Geometry.SweepOneRail();
            sweep.AngleToleranceRadians = 0.017453;
            sweep.ClosedSweep = false;
            sweep.SweepTolerance =0.001;
            sweep.SetToRoadlikeTop();
            var breps = sweep.PerformSweep(iRail, rectC);

            DA.SetDataList(0, breps);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources._24a;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ff8a74de-abaf-48e8-bafe-24326e091ea2"); }
        }
    }
}