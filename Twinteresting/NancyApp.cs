using Nancy;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyCSharpWORK
{
    public class NancyApp : NancyModule
    {
        public NancyApp()
        {
            Get["/"] = _ => View["Views/index"];

            Get["/{query}"] = parameters =>
            {
                var query = parameters.query;

                // Send a message through the web api to my little Twitter reading service
                var client = new RestClient("http://localhost:1111");
                RestRequest queryRequest = new RestRequest(query, Method.POST);
                client.Execute(queryRequest);

                return "cool";
            };
        }
    }
}
