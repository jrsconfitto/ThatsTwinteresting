using Nancy.Hosting.Wcf;
using System;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

using TwitterQueryer.PI_AF;

namespace TwitterQueryer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                var baseUrl = "http://localhost:1111/";

                Console.WriteLine(String.Format("We're operating on a different port! {0}", baseUrl));

                // Connect to my AF Server
                string piSystemName = args[0];
                string afDBName = args[1];

                if (args.Length == 4)
                {
                    string username = (args.Length == 4) ? args[2] : "";
                    string password = (args.Length == 4) ? args[3] : "";
                    PIUtilities.Connect(piSystemName, afDBName, username, password);
                }
                else
                {
                    PIUtilities.Connect(piSystemName, afDBName);
                }

                // Verify the connection on the console
                Console.WriteLine(String.Format("AF Database {0} is connected!", PI_AF.PIUtilities.afDB.Name));

                // Start up the Nancy web api for my project
                var host = new WebServiceHost(new NancyWcfGenericService(),
                                              new Uri(baseUrl));
                host.AddServiceEndpoint(typeof(NancyWcfGenericService), new WebHttpBinding(), "");
                host.Open();

                // Start up a thread that will watch for any new queries
                var timer = new System.Timers.Timer(20000);

                // Start up a timer to continuously query Twitter
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Start();
            }
            else
            {
                Console.WriteLine("Please provide arguments for the PI System");
            }

            Console.ReadLine();
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Grab all the Elements that represent Twitter Queries. i'm assuming 500 is way too much for now. Kinda like 1 Gb ethernet...
            AFNamedCollectionList<AFElement> twitterQueryElements = AFElement.FindElementsByTemplate(
                PIUtilities.afDB,
                null,
                PIUtilities.afQueryElementTemplate,
                true,
                AFSortField.Name,
                AFSortOrder.Ascending,
                500
            );

            // Run each query against Twitter again
            foreach (AFElement queryElement in twitterQueryElements)
            {
                var active = Boolean.Parse(queryElement.Attributes["active"].GetValue().Value.ToString());

                if (active)
                {
                    Console.WriteLine("My service is checking Twitter for the {0} query.", queryElement.Name);

                    var location_query_string = queryElement.Attributes["Location Query"].GetValue().Value.ToString();
                    TwitterQueryer.Twitter.Querier.QueryTwitter(queryElement);
                }
                else
                {
                    Console.WriteLine("{0} query is inactive.", queryElement.Name);
                }

                // Formatting
                Console.WriteLine();
            }
        }
    }
}
