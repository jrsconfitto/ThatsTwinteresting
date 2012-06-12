using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterQueryer
{
    public class TwitterQuerierNancyApp : NancyModule
    {
        public TwitterQuerierNancyApp()
        {
            Post["/query/{query}"] = parameters =>
            {
                var query = parameters.query;
                
                // Start a Twitter Query for that data
                TwitterQueryer.Twitter.Querier.QueryTwitter(query);

                return null;
            };
        }
    }
}
