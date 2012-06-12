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

            // Connect to my AF Server
            TwitterQueryer.PI_AF.PIConnection.Connect(args[0], args[1], args[2], args[3]);

            // Verify the connection on the console
            Console.WriteLine(String.Format("AF Database {0} is connected!", PI_AF.PIConnection.afDB.Database.Name));

            // Start up the Nancy web api for my project
            var host = new WebServiceHost(new NancyWcfGenericService(),
                                          new Uri(baseUrl));
            host.AddServiceEndpoint(typeof (NancyWcfGenericService), new WebHttpBinding(), "");
            host.Open();

            Console.ReadLine();
        }
    }
}
