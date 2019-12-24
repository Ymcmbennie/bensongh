using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace BensonGH
{
    public class BensonGHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "BenGHC";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "This library is a brain child of my learning process and experimetaion in grasshopper c#, python and processing t develop custom geometry using rhinoceros as a base for my work";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e62d57d0-30e6-451b-bd85-03a2cf4c4142");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Benson Sanga";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "bensonsanga@hotmail.com";
            }
        }
    }
}
