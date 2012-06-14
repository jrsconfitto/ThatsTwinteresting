using Nancy.Hosting.Wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Hackathon.PI_AF;

namespace Hackathon
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                // Connect to my AF Server
                string piSystemName = args[0];
                string afDBName = args[1];

                if (args.Length == 4)
                {
                    string username = args[2];
                    string password = args[3];
                    PIConnection.Connect(piSystemName, afDBName, username, password);
                }
                else
                {
                    PIConnection.Connect(piSystemName, afDBName);
                }

                if (PIConnection.afDB.PISystem.ConnectionInfo.IsConnected)
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
