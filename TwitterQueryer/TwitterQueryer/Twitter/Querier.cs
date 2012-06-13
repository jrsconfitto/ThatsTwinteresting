using RestSharp;
using System;
using OSIsoft.AF;
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
            // Create the request for Twitter's API
            IRestRequest tqRequest = new RestRequest("search.json", Method.POST);
            tqRequest.AddParameter("q", queryString);

            // Create a PI Twitter Query Element for the query if it's not already present
            OSIsoft.AF.Asset.AFElement queryElement = PIConnection.afDB.Elements[queryString];

            if (queryElement == null)
            {
                queryElement = new AFElement(queryString.Replace("%20", " "), PIConnection.afQueryElementTemplate);
                queryElement.Attributes["Query"].SetValue(new AFValue(queryString.Replace("%20", " ")));
                queryElement.Attributes["Query Start Time"].SetValue(new AFValue(DateTime.Now));

                PIConnection.afDB.Elements.Add(queryElement);
                PIConnection.afDB.CheckIn();
                Console.WriteLine("New AF Element created for this Twitter query");
            }
            else
            {
                // Try to use the last query found
                var max_id = queryElement.Attributes["max_id"].GetValue().ToString();
                tqRequest.AddParameter("since_id", max_id);
            }


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

                    //todo: add a page for each

                    // Build an EF for each resulting tweet
                    foreach (Result tweet in result.results)
                    {
                        // Create a new EventFrame for each tweet and fill in its attributes
                        //todo: make sure i'm not duplicating a tweet
                        AFEventFrame tweetEF = new AFEventFrame(PIConnection.afDB, "tweet #" + tweet.id_str, PIConnection.tweetEFTemplate);
                        tweetEF.PrimaryReferencedElement = queryElement;

                        // Timing
                        tweetEF.SetStartTime(tweet.created_at);
                        tweetEF.SetEndTime(tweet.created_at);
                        tweetEF.Description = String.Format("{0} tweet from {1}", queryElement.Name, tweet.from_user);

                        // Attributes
                        tweetEF.Attributes["id"].SetValue(new AFValue(tweet.id_str));
                        tweetEF.Attributes["profile_image_url"].SetValue(new AFValue(tweet.profile_image_url));
                        tweetEF.Attributes["Text"].SetValue(new AFValue(tweet.text));
                        tweetEF.Attributes["User id"].SetValue(new AFValue(tweet.from_user_id_str));
                        tweetEF.Attributes["User name"].SetValue(new AFValue(tweet.from_user));

                        // Get the coordinates into PI somehow
                        if (tweet.geo != null && tweet.geo.coordinates.Count == 2)
                        {
                            tweetEF.Attributes["Latitude"].SetValue(new AFValue(tweet.geo.coordinates[0]));
                            tweetEF.Attributes["Longitude"].SetValue(new AFValue(tweet.geo.coordinates[1]));
                        }
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
            finally
            {
                Console.WriteLine();
            }
        }
    }
}
