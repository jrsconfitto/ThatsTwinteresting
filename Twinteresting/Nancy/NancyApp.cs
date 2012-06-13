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
using Hackathon.PI_AF;

namespace Hackathon
{
    public class NancyApp : NancyModule
    {
        public NancyApp()
        {
            Get["/"] = _ => View["Views/index"];

            Get["/locations/{query}"] = parameters =>
            {
                var query = parameters.query;

                // Create the request for Twitter's API
                RestClient twitterLocationClient = new RestClient("http://api.twitter.com/1/geo/");
                IRestRequest geoRequest = new RestRequest("search.json", Method.POST);
                geoRequest.AddParameter("query", query);

                IRestResponse<Hackathon.TwitterAPI.TwitterGeoResult> geoResponse = twitterLocationClient.Execute<Hackathon.TwitterAPI.TwitterGeoResult>(geoRequest);

                if (geoResponse.Data != null && geoResponse.Data.result != null)
                {
                    return Response.AsJson(geoResponse.Data.result.places);
                }
                else
                {
                    return Response.AsJson("");
                }
            };

            //todo: this will have to take some extra parameters for going through query results
            Get["/query/{id}/{count}"] = parameters =>
            {
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
                    100);

                List<TwitterQueryModel> elementsModel = new List<TwitterQueryModel>();

                foreach (AFElement queryElement in twitterQueryElements)
                {
                    var queryActive = queryElement.Attributes["Active"].GetValue().Value.ToString();
                    var timestamp = queryElement.Attributes["Query Start Time"].GetValue().ToString();

                    elementsModel.Add(
                        new TwitterQueryModel()
                        {
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

        public Response getQueryTweets(string id, int count)
        {
            // Get the Element with that Guid
            Guid queryID = new Guid(id);

            // If this is a location based tweet, grab only tweets with coordinates (for now)

            AFElement twitterQuery = AFElement.FindElement(PIConnection.afDB.PISystem, queryID);

            var location_based = twitterQuery.Attributes["Location Query"].GetValue().Value.ToString() != "0";

            AFNamedCollectionList<AFEventFrame> tweets;
            if (location_based)
            {
                AFAttributeTemplate latitudeTemplate = PIConnection.tweetEFTemplate.AttributeTemplates["Latitude"];

                tweets = AFEventFrame.FindEventFramesByAttribute(
                    null,
                    AFSearchMode.Inclusive,
                    DateTime.Now.AddDays(-30),
                    DateTime.Now,
                    "",
                    twitterQuery.Name,
                    new AFDurationQuery[] { new AFDurationQuery(OSIsoft.AF.Search.AFSearchOperator.NotEqual, new TimeSpan(24,0,0)) },
                    new AFAttributeValueQuery[] { new AFAttributeValueQuery(latitudeTemplate, OSIsoft.AF.Search.AFSearchOperator.NotEqual, "")},
                    true,
                    AFSortField.StartTime,
                    AFSortOrder.Descending,
                    0,
                    count
                );
            }
            else
            {
            tweets = twitterQuery.GetEventFrames(
               DateTime.Now,
               0,
               count,
               AFEventFrameSearchMode.BackwardFromStartTime,
               "",
               null,
               PIConnection.tweetEFTemplate
            );
            }

            // Build up the model that i'm going to use
            List<TweetModel> tweetModelList = new List<TweetModel>();
            foreach (AFEventFrame ef in tweets)
            {
                string loc_link;
                string lat = ef.Attributes["Latitude"].GetValue().Value.ToString();
                string longi = ef.Attributes["Longitude"].GetValue().Value.ToString();

                if (lat != "" && longi != "") {
                    loc_link = string.Format(@"<a href=""https://maps.google.com/maps?q=loc:{0},{1}"">(Location)</a>", lat, longi);
                }
                else {
                    loc_link = "";
                }

                tweetModelList.Add(new TweetModel() {
                    id = ef.Attributes["id"].GetValue().Value.ToString(),
                    user_name = ef.Attributes["User name"].GetValue().Value.ToString(),
                    profile_url = ef.Attributes["profile_image_url"].GetValue().Value.ToString(),
                    text = ef.Attributes["Text"].GetValue().ToString(),
                    latitude = lat,
                    longitude = longi,
                    location_link = loc_link
                });
            }

            TwitterQueryModel twitterQueryModel = new TwitterQueryModel()
            {
                id = queryID.ToString(),
                name = twitterQuery.Attributes["Query"].GetValue().ToString(),
                active = bool.Parse(twitterQuery.Attributes["Active"].GetValue().Value.ToString()),
                location = location_based,
                queryTime = DateTime.Now,
                tweets = tweetModelList
            };

            return View["Views/query", twitterQueryModel];
        }
    }
}
