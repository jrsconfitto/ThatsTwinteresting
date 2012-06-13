using Nancy;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// So i can access my bad global variables more easily
using NancyCSharpWORK.PI_AF;

namespace NancyCSharpWORK
{
    public class NancyApp : NancyModule
    {
        public NancyApp()
        {
            Get["/"] = _ => View["Views/index"];

            //todo: this will have to take some extra parameters for going through query results
            Get["/query/{id}/{count}"] = parameters => {
                var id = parameters.id;
                var count = parameters.count;

                return getQueryTweets(id, count);
            };

            Get["/query/{id}"] = parameters =>
            {
                var id = parameters.id;

                return getQueryTweets(id, 25);
            };

            Get["/results"] = _ =>
            {
                // Get the queries from the PI AF database
                AFNamedCollectionList<AFElement> twitterQueryElements = AFElement.FindElementsByTemplate(PIConnection.afDB,
                    null,
                    PIConnection.afQueryElementTemplate,
                    true,
                    AFSortField.Name,
                    AFSortOrder.Ascending,
                    12);

                List<TwitterQueryModel> elementsModel = new List<TwitterQueryModel>();

                foreach (AFElement queryElement in twitterQueryElements)
                {
                    var queryActive = queryElement.Attributes["Active"].GetValue().Value.ToString();
                    var timestamp = queryElement.Attributes["Query Start Time"].GetValue().ToString();

                    elementsModel.Add(
                        new TwitterQueryModel() {
                            id = queryElement.ID.ToString(),
                            name = queryElement.Name,
                            active = bool.Parse(queryActive),
                            queryTime = DateTime.Parse(timestamp)
                        }
                    ); 
                }

                return Response.AsJson(elementsModel);
            };
        }

        public Response getQueryTweets(string id, int count) {

                // Get the Element with that Guid
                Guid queryID = new Guid(id);

                AFElement twitterQuery = AFElement.FindElement(PIConnection.afDB.PISystem, queryID);
                AFNamedCollectionList<AFEventFrame> tweets = twitterQuery.GetEventFrames(
                    DateTime.Now,
                    0,
                    count,
                    AFEventFrameSearchMode.BackwardFromStartTime,
                    "",
                    null,
                    PIConnection.tweetEFTemplate
                );

                // Build up the model that i'm going to use
                List<TweetModel> tweetModelList = new List<TweetModel>();
                foreach (AFEventFrame ef in tweets) {
                    tweetModelList.Add(new TweetModel() {
                        id = ef.Attributes["id"].GetValue().Value.ToString(),
                        user_name = ef.Attributes["User name"].GetValue().Value.ToString(),
                        profile_url = ef.Attributes["profile_image_url"].GetValue().Value.ToString(),
                        text = ef.Attributes["Text"].GetValue().ToString()
                    });
                }

                TwitterQueryModel twitterQueryModel = new TwitterQueryModel()
                {
                    id = queryID.ToString(),
                    name = twitterQuery.Attributes["Query"].GetValue().ToString(),
                    active = bool.Parse(twitterQuery.Attributes["Active"].GetValue().Value.ToString()),
                    queryTime = DateTime.Now,
                    tweets = tweetModelList
                };

                return View["Views/query", twitterQueryModel];
        }
    }
}
