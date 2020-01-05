using System;
using System.Collections.Generic;
using BensonGH.Properties;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace BensonGH
{
    public class GridSystemGH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GridSystemGH class.
        /// </summary>
        public GridSystemGH()
          : base("GridSystemGH", "UVCurtain",
              "Generates UV lines for curtain wall grids on surface",
              "BensonGHC", "Curtain Panels")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("iSurface", "S", "Input Surface for roof", GH_ParamAccess.item);
            pManager.AddNumberParameter("Horizontal members", "iU", "Number of Pattern in X", GH_ParamAccess.item, 5.0);
            pManager.AddNumberParameter("Vertical members", "iV", "Number of Pattern in Y", GH_ParamAccess.item, 5.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("uLines", "U", "Horitzontal Lines from pattern", GH_ParamAccess.list);
            pManager.AddLineParameter("vLine", "v", "Lines from pattern in v direction", GH_ParamAccess.list);
            pManager.AddBrepParameter("oPipes", "P", "pipes output as breps", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Surface iSurface = null;
            double iV = 0.0;
            double iU = 0.0;

            //Rhino.Geometry.Curve curve = Rhino.Geometry.Curve.Unset;

            if (!DA.GetData(0, ref iSurface)) return;
            if (!DA.GetData(1, ref iV)) return;
            if (!DA.GetData(2, ref iU)) return;

            //Convert surface to brep
            Brep bp = iSurface.ToBrep();

            //List of curves
            List<Curve> crvs = new List<Curve>();
            List<Curve> crvsV = new List<Curve>();

            //thr loop
            for (double i = 0; i < 1; i += 1 / iV)
            {
                //Make the plane
                Point3d pt = new Point3d(0, 0, i * iSurface.IsoCurve(0, 0.5).GetLength());
                Vector3d vZ = new Vector3d(0, 0, 1);
                Plane pln = new Plane(pt, vZ);

                //Declare the lists
                Curve[] crvs01;
                Point3d[] pts;

                //Intersect geometry
                Rhino.Geometry.Intersect.Intersection.BrepPlane(bp, pln, 2.0, out crvs01, out pts);

                //Add to curves
                crvs.Add(crvs01[0]);
                

            }
            //vertical ones
            var bs = iSurface.IsoCurve(1, 0.5);
            for (double i = 0; i < 1; i += 1 / iU)
            {
                crvsV.Add(iSurface.IsoCurve(0, i * bs.GetLength()));
            }

            List<Brep> breps = new List<Brep>();
            for (int d = 0; d < crvs.Count; d++)
            {
                breps.Add(Brep.CreatePipe(crvs[d], 0.2, true, 0, true, 2.00, 0.01)[0]);
            }

            for (int d = 0; d < crvsV.Count; d++)
            {
                breps.Add(Brep.CreatePipe(crvsV[d], 0.2, true, 0, true, 2.00, 0.01)[0]);
            }


            DA.SetDataList(0, crvs);
            DA.SetDataList(1, crvsV);
            DA.SetDataList(2, breps);

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
            get { return new Guid("2ab604b1-ab1c-44ee-afa1-f6f268dca241"); }
        }
    }
}