using RestSharp;
using System;

namespace TwitterQueryer
{

    public class Querier
    {
        public static RestClient client = new RestClient("http://search.twitter.com/");

        public static void QueryTwitter(string queryString)
        {
            IRestRequest tqRequest = new RestRequest("search.json", Method.POST);
            tqRequest.AddParameter("q", queryString);

            IRestResponse tqResponse = client.Execute(tqRequest);

            if (tqResponse != null)
            {
                Console.WriteLine(tqResponse.Content);
            }
        }
    }
}
