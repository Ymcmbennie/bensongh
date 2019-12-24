using System;
using System.Collections.Generic;
using BensonGH.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace BensonGH
{
    public class ShigerubanPattern : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ShigerubanPattern class.
        /// </summary>
        public ShigerubanPattern()
          : base("ShigerubanPattern", "Patt01",
              "Shigeru ban like pattern 2D line output",
              "BensonGHC", "Geometry")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "Plane for start of pattern", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddIntegerParameter("xCount", "X", "Number of units in the X", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("yCount", "Y", "Number of units in the Y", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("xSize", "iX", "Size of Pattern in X", GH_ParamAccess.item, 2.0);
            pManager.AddNumberParameter("ySize", "iY", "Size of Pattern in Y", GH_ParamAccess.item, 2.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("oLines", "L", "Lines from pattern", GH_ParamAccess.list);
            pManager.AddLineParameter("vLine", "v", "Lines from pattern in v direction", GH_ParamAccess.list);
            pManager.AddLineParameter("uLine", "u", "Lines from pattern in the y direction", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane origin = new Plane(0,0,0,0);
            double xSize = 0.0;
            double ySize = 0.0;
            int xCount = 0;
            int yCount = 0;

            if (!DA.GetData(0, ref origin)) return;

            if (!DA.GetData(3, ref xSize)) return;
            if (!DA.GetData(4, ref ySize)) return;

            if (!DA.GetData(1, ref xCount)) return;
            if (!DA.GetData(2, ref yCount)) return;



            double xStart = origin.OriginX;
            double yStart = origin.OriginY;

            List<Point3d> oPoints = new List<Point3d>();

            List<Rectangle3d> oRect = new List<Rectangle3d>();

            List<Line> oLines0 = new List<Line>();
            List<Line> oLines1 = new List<Line>();
            List<Line> oLines2 = new List<Line>();


            for (double i = xStart; i < xCount + xStart; i += xSize)
            {
                for (double j = yStart; j < yCount + yStart; j += ySize)
                {
                    Vector3d zV = new Vector3d(0, 0, 1);
                    Point3d pt = new Point3d(i, j, 0);
                    Plane pl = new Plane(pt, zV);
                    Rectangle3d rect = new Rectangle3d(pl, xSize, ySize);
                    oRect.Add(rect);
                    oPoints.Add(pt);
                    Point3d pt1 = rect.Corner(0);
                    Point3d pt2 = rect.Corner(1);
                    Point3d pt3 = rect.Corner(2);
                    Point3d pt4 = rect.Corner(3);
                    Line ln1 = new Line(pt1, pt2);
                    Line ln2 = new Line(pt3, pt4);
                    Line ln3 = new Line(pt2, pt3);
                    Line ln4 = new Line(pt1, pt4);
                    Point3d pt5 = ln1.PointAt(0.5);
                    Point3d pt6 = ln2.PointAt(0.5);
                    Point3d pt7 = ln3.PointAt(0.5);
                    Point3d pt8 = ln4.PointAt(0.5);

                    Point3d pt9 = ln1.PointAt(0.3);
                    Point3d pt10 = ln2.PointAt(0.7);
                    Point3d pt11 = ln1.PointAt(0.7);
                    Point3d pt12 = ln2.PointAt(0.3);

                    Line ln9 = new Line(pt9, pt10);
                    oLines0.Add(ln9);
                    oLines2.Add(ln9);
                    Line ln10 = new Line(pt11, pt12);
                    oLines0.Add(ln10);
                    oLines2.Add(ln10);

                    Line ln5 = new Line(pt5, pt7);
                    oLines1.Add(ln5);
                    oLines2.Add(ln5);
                    Line ln6 = new Line(pt6, pt8);
                    oLines1.Add(ln6);
                    oLines2.Add(ln6);
                    Line ln7 = new Line(pt6, pt7);
                    oLines1.Add(ln7);
                    oLines2.Add(ln7);
                    Line ln8 = new Line(pt5, pt8);
                    oLines1.Add(ln8);
                    oLines2.Add(ln8);



                }
            }

            DA.SetDataList(0, oLines2);
            DA.SetDataList(1, oLines0);
            DA.SetDataList(2, oLines1);

        
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
                return Resources._24p;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d293abe6-91fc-4ad6-946c-71d7ad62ae52"); }
        }
    }
}