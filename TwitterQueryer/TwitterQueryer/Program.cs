using Nancy.Hosting.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TwitterQueryer
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseUrl = "http://localhost:1111/";

            Console.WriteLine(String.Format("We're operating on a different port! {0}", baseUrl));

            var host = new WebServiceHost(new NancyWcfGenericService(),
                                          new Uri(baseUrl));
            host.AddServiceEndpoint(typeof (NancyWcfGenericService), new WebHttpBinding(), "");
            host.Open();

            Console.ReadLine();
        }
    }
}
