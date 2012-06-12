using RestSharp;
using System;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;

using TwitterQueryer.PI_AF;


namespace TwitterQueryer.Twitter
{

    public class Querier
    {
        public static RestClient client = new RestClient("http://search.twitter.com/");

        public static void QueryTwitter(string queryString)
        {
            // Create a PI Twitter Query Element for the query
            OSIsoft.AF.Asset.AFElement queryElement = new AFElement(queryString, PIConnection.afQueryElementTemplate);
            queryElement.Attributes["Query"].SetValue(new AFValue(queryString));
            queryElement.Attributes["Query Start Time"].SetValue(new AFValue(DateTime.Now));
            PIConnection.afDB.Elements.Add(queryElement);
            PIConnection.afDB.CheckIn();
            Console.WriteLine("New AF Element created for this Twitter query");

            // Create the request for Twitter's API
            IRestRequest tqRequest = new RestRequest("search.json", Method.POST);
            tqRequest.AddParameter("q", queryString);

            try
            {
                // Log the request to Console
                Console.WriteLine(String.Format("Querying Twitter for {0}...", queryString));

                var queryResponse = client.Execute<TweetResponse>(tqRequest);

                // Query finished
                if (queryResponse != null)
                {
                    Console.WriteLine(queryResponse.Data);

                    TweetResponse result = queryResponse.Data;

                    // Feed in the AFElement's attributes from the results of the Query
                    queryElement.Attributes["Completed in"].SetValue(new AFValue(result.completed_in));
                    queryElement.Attributes["max_id"].SetValue(new AFValue(result.max_id));

                    // Build an EF for each resulting tweet
                    foreach (Result tweet in result.results)
                    {
                        // Create a new EventFrame for each tweet
                        AFEventFrame tweetEF = new AFEventFrame(PIConnection.afDB, "tweet #" + tweet.id_str, PIConnection.tweetEFTemplate);
                    }

                    Console.WriteLine("Wrote {0} tweets to PI", result.results.Count);

                    // Now put all that into PI!
                    PIConnection.afDB.CheckIn();

                }
                else
                {
                    Console.WriteLine("Query responded with null results!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}
