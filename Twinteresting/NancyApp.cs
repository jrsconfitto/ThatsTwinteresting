using Nancy;
using Nancy.Diagnostics;
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
            Get["/"] = _ => "hello world";

            Get["/{what}"] = parameters =>
            {
                var what = parameters.what;
                return "hello there " + what + "!";
            };
        }
    }

    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"Hackathon" }; }
        }
    }
}
