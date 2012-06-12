using RestSharp;
using System;

namespace TwitterQueryer.Twitter
{

    public class Querier
    {
        public static RestClient client = new RestClient("http://search.twitter.com/");

        public static void QueryTwitter(string queryString)
        {
            IRestRequest tqRequest = new RestRequest("search.json", Method.POST);
            tqRequest.AddParameter("q", queryString);

            try
            {
                var tqResponse = client.Execute<TweetResponse>(tqRequest);

                if (tqResponse != null)
                {
                    Console.WriteLine(tqResponse.Data);

                    // Now put all that into PI!
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}
