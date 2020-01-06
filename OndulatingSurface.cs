using System;
using System.Collections.Generic;
using BensonGH.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace BensonGH
{
    public class OndulatingSurface : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public OndulatingSurface()
          : base("Ondulating Surface", "oSrf",
              "Creates an ondulating Surface for the use of rapid protyping",
              "BensonGHC", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("iSize", "iS", "Size of surface based on coordinates of Pi", GH_ParamAccess.item, 10.0);
            pManager.AddNumberParameter("iScale", "iSc", "Scallar value of the size of surface", GH_ParamAccess.item, 5);
            pManager.AddPointParameter("iOrigin", "O", "Origin point to put the surface, if not set default origin", GH_ParamAccess.item, new Point3d(0, 0, 0));
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("oSurface", "S", "Output Surface", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double iSize = 0.0;
            double iScale = 0.0;
            double iRes = 0.5;
            Point3d iOrigin = new Point3d(0,0,0);

            if (!DA.GetData(0, ref iSize)) return;
            if (!DA.GetData(1, ref iScale)) return;
            if (!DA.GetData(2, ref iOrigin)) return;

            if (iSize < 0.0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Size needs t be a non zero positive number");
                return;
            }
            if (iScale <0.0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Size needs t be a non zero positive number");
                return;
            }

            List<Point3d> oPoints = new List<Point3d>();

            for (double i = -iSize; i < iSize; i += iRes)
            {
                for (double j = -iSize; j < iSize; j += iRes)
                {
                    Double z = Math.Sin(Math.Sqrt(i * i + j * j));
                    oPoints.Add(new Point3d(iOrigin.X + i * iScale, iOrigin.Y +  j * iScale, iOrigin.Z + z * iScale));
                }
            }

          //int uCount = Convert.ToInt32(Math.Sqrt(oPoints.Count));
          //int vCount = Convert.ToInt32(Math.Sqrt(oPoints.Count));
            
            int uCount = Convert.ToInt32(iSize / 0.5 * 2);
            int vCount = Convert.ToInt32(iSize / 0.5 * 2);
            int uDegree = 5;
            int vDegree = 5;


            Surface oSurface = NurbsSurface.CreateThroughPoints(oPoints, uCount, vCount, uDegree, vDegree, false, false);

            DA.SetData(0, oSurface);
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
                return Resources._24o;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("27f92cd9-9f66-4dd5-88ac-dc0f0eab6396"); }
        }
    }
}