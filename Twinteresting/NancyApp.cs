using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyCSharpWORK
{
    public class NancyApp : NancyModule
    {
        public NancyApp()
        {
            Get["/"] = _ => View["Views/index"];
        }
    }
}
