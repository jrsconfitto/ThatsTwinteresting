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

        public static void QueryTwitter(AFElement queryElement)
        {
            try
            {
                // Create the request for Twitter's API
                IRestRequest tqRequest = new RestRequest("search.json", Method.POST);

                // Get query information from the element
                string query = queryElement.Attributes["Query"].GetValue().Value.ToString();
                string place_id = queryElement.Attributes["place_id"].GetValue().Value.ToString();
                string max_id = queryElement.Attributes["max_id"].GetValue().ToString();

                tqRequest.AddParameter("q", query);
                tqRequest.AddParameter("since_id", max_id);

                if (place_id  != "" && place_id == "")
                {
                    tqRequest.AddParameter("geocode", place_id);
                }

                try
                {
                    // Log the request to Console
                    Console.WriteLine(String.Format("Querying Twitter for {0}...", query));

                    var queryResponse = client.Execute<TweetResponse>(tqRequest);

                    // Query finished
                    if (queryResponse != null)
                    {
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
                            AFAttributeTemplate tweetIdTemplate = PIUtilities.tweetEFTemplate.AttributeTemplates["id"];

                            AFNamedCollectionList<AFEventFrame> frames = AFEventFrame.FindEventFramesByAttribute(
                                null,
                                AFSearchMode.None,
                                DateTime.Now.AddDays(-15),
                                DateTime.Now,
                                "",
                                queryElement.Name,
                                new AFDurationQuery[] { new AFDurationQuery(OSIsoft.AF.Search.AFSearchOperator.Equal, new TimeSpan(0, 10, 0)) },
                                new AFAttributeValueQuery[] {new AFAttributeValueQuery(tweetIdTemplate, OSIsoft.AF.Search.AFSearchOperator.Equal,tweet.id_str)}, 
                                false,
                                AFSortField.ID,
                                AFSortOrder.Ascending,
                                0,
                                10000);

                            if (frames.Count == 0)
                            {
                                AFEventFrame tweetEF = new AFEventFrame(PIUtilities.afDB, "tweet #" + tweet.id_str, PIUtilities.tweetEFTemplate);
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
                            else
                            {
                                foreach (AFEventFrame efTweet in frames) {
                                  Console.WriteLine("Duplicate found for id: " + efTweet.Attributes["id"]);
                                }
                            }
                        }

                        Console.WriteLine("Wrote {0} tweets to PI", result.results.Count);

                        // Now put all that into PI!
                        PIUtilities.afDB.CheckIn();

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
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }
}
