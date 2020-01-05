using System;
using System.Collections.Generic;
using BensonGH.Properties;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace BensonGH
{
    public class lineToSurfaceBenGH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the lineToSurfaceBenGH class.
        /// </summary>
        public lineToSurfaceBenGH()
          : base("lineToSurfaceBenGH", "lineToSrf",
              "Line projected to surface roof in Z direction. To create a flat curtain panel",
              "BensonGHC", "Curtain Panels")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("iCurve", "C", "Input base curve for wall outline", GH_ParamAccess.item);
            pManager.AddBrepParameter("iSurface", "S", "Input Surface for roof", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("oSurface", "S", "Out surface curtain wall", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Curve iLine = null;
            Brep iSurface = null;

            //Rhino.Geometry.Curve curve = Rhino.Geometry.Curve.Unset;

            if (!DA.GetData(0, ref iLine)) return;
            if (!DA.GetData(1, ref iSurface)) return;

            List<Curve> lstCrv = new List<Curve>();

            lstCrv.Add(iLine);

            double tol = 2.0;
            Vector3d iZ = new Vector3d(0, 0, 1);

            Curve[] arrProCrv;

            arrProCrv = Curve.ProjectToBrep(iLine, iSurface, iZ, tol);

            lstCrv.Add(arrProCrv[0]);


            Brep[] oSurace = Brep.CreateFromLoft(lstCrv, Point3d.Unset, Point3d.Unset, LoftType.Straight, false);
            


            DA.SetDataList(0, oSurace);
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
                return Resources._24l;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("54fd10f5-d8dd-413f-b5b6-1e01338c8627"); }
        }
    }
}