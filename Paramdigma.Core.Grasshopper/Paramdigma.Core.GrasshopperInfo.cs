using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace Paramdigma.Core.Grasshopper
{
    public class ParamdigmaCoreGrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name => "Paramdigma.Core.Grasshopper Info";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "Components based on Paramdigma.Core geometry library";

        public override Guid Id => new Guid("78D40297-5FAA-433B-B2F9-A7E0E6DC7DF4");

        //Return a string identifying you or your company.
        public override string AuthorName => "Paramdigma";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "info@paramdigma.com";
    }
}