using Nancy.Hosting.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NancyCSharpWORK
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                // Connect to my AF Server
                NancyCSharpWORK.PI_AF.PIConnection.Connect(args[0], args[1], args[2], args[3]);
                if (PI_AF.PIConnection.afDB.PISystem.ConnectionInfo.IsConnected)
                {
                    Console.WriteLine("PI is connected");
                }

                // Start the WCF host for the web application
                var host = new WebServiceHost(new NancyWcfGenericService(),
                                              new Uri("http://localhost:1234/"));
                host.AddServiceEndpoint(typeof(NancyWcfGenericService), new WebHttpBinding(), "");
                host.Open();

                Console.WriteLine("Serving on http://localhost:1234/");
            }
            else
            {
                Console.WriteLine("i need connection information for PI AF");
            }

            Console.ReadKey();
        }
    }
}
