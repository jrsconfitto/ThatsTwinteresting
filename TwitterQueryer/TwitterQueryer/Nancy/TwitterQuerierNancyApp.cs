using Nancy;
using OSIsoft.AF.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// So i can use my global (yikes!) afDatabase object
using TwitterQueryer.PI_AF;

namespace TwitterQueryer
{
    public class TwitterQuerierNancyApp : NancyModule
    {
        public TwitterQuerierNancyApp()
        {
            Post["/query/{query}"] = parameters =>
            {
                var query = parameters.query;
                
                Console.WriteLine(String.Format("Request to search for {0} received", query));

                // Start a Twitter Query for that data
                TwitterQueryer.Twitter.Querier.QueryTwitter(query);

                return null;
            };
        }
    }
}
