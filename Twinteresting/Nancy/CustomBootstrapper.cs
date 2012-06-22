using Nancy;
using Nancy.Conventions;
using Nancy.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using TinyIoC;

namespace Hackathon
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            this.Conventions.ViewLocationConventions.Add((viewName, model, context) =>
            {
                return string.Concat("Web/", viewName);
            });
        }

        protected override Type RootPathProvider
        {
            get
            {
                return typeof(CustomRootPathProvider);
            }
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("Web/Content")
            );
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"Hackathon" }; }
        }
    }

    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
#if DEBUG
            return Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
#else
            return AppDomain.CurrentDomain.BaseDirectory;
#endif
        }
    }
}
