using Nancy.Hosting.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NancyCSharpWORK
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Serving on http://localhost:1234/");

            var host = new WebServiceHost(new NancyWcfGenericService(),
                                          new Uri("http://localhost:1234/"));
            host.AddServiceEndpoint(typeof (NancyWcfGenericService), new WebHttpBinding(), "");
            host.Open();

            Console.ReadKey();
        }
    }
}
