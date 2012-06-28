using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TwitterQueryer.PI_AF
{
    public class PIUtilities
    {
        public static AFDatabase afDB;
        public static AFElementTemplate afQueryElementTemplate;
        public static AFElementTemplate tweetEFTemplate;

        /// <summary>
        /// Connects to the PI AF machine
        /// </summary>
        public static void Connect(string systemName, string databaseName, string user = "", string pass = "")
        {
            // Get the PI System
            PISystems pisystems = new PISystems();
            PISystem pisystem = pisystems[systemName];

            // Connect
            if (user != "" && pass != "")
            {
                pisystem.Connect(new NetworkCredential(user, pass));
            }
            else
            {
                pisystem.Connect();
            }

            // Grab the database
            afDB = pisystem.Databases[databaseName];

            // Grab the Twitter Query's Element Template
            afQueryElementTemplate = afDB.ElementTemplates["Twitter Query"];
            tweetEFTemplate = afDB.ElementTemplates["Tweet"];
        }

        /// <summary>
        /// Create a new AF Element for the query with a location
        /// </summary>
        /// <param name="queryString">The query that will be sent to Twitter's API</param>
        /// <param name="place">The location string sent to Twitter's API</param>
        public static AFElement CreateQueryElement(string queryString, string place = "")
        {
            //todo: make sure the element doesnt already exist under a certain name
            //todo: allow for similar queries but with different locations
            AFElement queryElement = new AFElement(queryString.Replace("%20", " "), PIUtilities.afQueryElementTemplate);
            queryElement.Attributes["Query"].SetValue(new AFValue(queryString.Replace("%20", " ")));
            queryElement.Attributes["Query Start Time"].SetValue(new AFValue(DateTime.Now));

            // Add in the location if it's active
            if (place != "")
            {
                queryElement.Attributes["place_id"].SetValue(new AFValue(place));
            }

            // Add the element into the AF Database
            AddElement(queryElement);

            return queryElement;
        }

        /// <summary>
        /// Adds the passed element into the AF Database. This method performs a CheckIn to finalize the addition.
        /// </summary>
        /// <param name="afElement">The element to add into the AF Database</param>
        private static void AddElement(AFElement afElement)
        {
            PIUtilities.afDB.Elements.Add(afElement);
            PIUtilities.afDB.CheckIn();
        }
    }
}
