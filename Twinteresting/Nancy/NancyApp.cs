using Nancy;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
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
                            name = queryElement.Name,
                            active = bool.Parse(queryActive),
                            queryTime = DateTime.Parse(timestamp)
                        }
                    ); 
                }

                return Response.AsJson(elementsModel);
            };

            //Get["/allresults"] = _ =>
            //    {

            //    };
        }
    }
}
