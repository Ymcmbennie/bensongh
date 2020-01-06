using System;
using System.Collections.Generic;
using BensonGH.Properties;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace BensonGH
{
    public class surfaceSplit : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the surfaceSplit class.
        /// </summary>
        public surfaceSplit()
          : base("surfaceSplit", "split",
              "Splits a surface into multiple grid surfaces",
               "BensonGHC", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("iSurface", "S", "Input Surface for roof", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Horizontal members", "iU", "Number of splits in X", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("Vertical members", "iV", "Number of splits in Y", GH_ParamAccess.item, 5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("oSurfaces", "S", "Out surface curtain wall", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Surface iSurface = null;
            int iU = 5;
            int iV = 5;

            if (!DA.GetData(0, ref iSurface)) return;
            if (!DA.GetData(1, ref iU)) return;
            if (!DA.GetData(2, ref iV)) return;


            List<Surface> oSurfaces = new List<Surface>();

            for (int i = 1; i < iV + 1; i++)
            {
                for (int j = 1; j < iV + 1; j++)
                {
                    double domainV = iSurface.Domain(0).T1 * i / iV;
                    double domainU = iSurface.Domain(1).T1 * j / iU;
                    double gapV = iSurface.Domain(0).T1 * 1 / iV;
                    double gapU = iSurface.Domain(1).T1 * 1 / iU;

                    Interval intervalV = new Interval(domainV - gapV, domainV);
                    Interval intervalU = new Interval(domainU - gapU, domainU);

                    oSurfaces.Add(iSurface.Trim(intervalV, intervalU));
                }
            }



            DA.SetDataList(0, oSurfaces);
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
                return Resources._24y;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1ea4c027-4136-4615-9b5e-531888eafe2c"); }
        }
    }
}