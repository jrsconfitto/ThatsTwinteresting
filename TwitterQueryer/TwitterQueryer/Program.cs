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
            var baseUrl = "http://localhost:1111/";

            Console.WriteLine(String.Format("We're operating on a different port! {0}", baseUrl));

            // Connect to my AF Server
            TwitterQueryer.PI_AF.PIConnection.Connect(args[0], args[1], args[2], args[3]);

            // Verify the connection on the console
            Console.WriteLine(String.Format("AF Database {0} is connected!", PI_AF.PIConnection.afDB.Name));

            // Start up the Nancy web api for my project
            var host = new WebServiceHost(new NancyWcfGenericService(),
                                          new Uri(baseUrl));
            host.AddServiceEndpoint(typeof (NancyWcfGenericService), new WebHttpBinding(), "");
            host.Open();

            // Start up a thread that will watch for any new queries
            var timer = new System.Timers.Timer(30000);

            // Start up a timer to continuously query Twitter
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Console.ReadLine();
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Grab all the Elements that represent Twitter Queries. i'm assuming 500 is way too much for now. Kinda like 1 Gb ethernet...
            AFNamedCollectionList<AFElement> twitterQueryElements = AFElement.FindElementsByTemplate(
                PIConnection.afDB,
                null,
                PIConnection.afQueryElementTemplate,
                true,
                AFSortField.Name,
                AFSortOrder.Ascending,
                500
            );
    
            // Run each query against Twitter again
            foreach (AFElement queryElement in twitterQueryElements) 
            {
                Console.WriteLine("My service is checking Twitter for the {0} query.", queryElement.Name);
                TwitterQueryer.Twitter.Querier.QueryTwitter(queryElement.Name);
            }
        }
    }}
