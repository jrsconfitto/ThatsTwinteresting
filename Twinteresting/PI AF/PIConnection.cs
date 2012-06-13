using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.EventFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hackathon.PI_AF
{
    public class PIConnection
    {
        public static AFDatabase afDB;
        public static AFElementTemplate afQueryElementTemplate;
        public static AFElementTemplate tweetEFTemplate;

        /// <summary>
        /// Connects to the PI AF machine
        /// </summary>
        public static void Connect(string systemName, string databaseName, string user, string pass)
        {
            // Get the PI System
            PISystems pisystems = new PISystems();
            PISystem pisystem = pisystems[systemName];

            // Connect
            pisystem.Connect(new NetworkCredential(user, pass, "vcampus"));

            // Grab the database
            afDB = pisystem.Databases[databaseName];

            // Grab the Twitter Query's Element Template
            afQueryElementTemplate = afDB.ElementTemplates["Twitter Query"];
            tweetEFTemplate = afDB.ElementTemplates["Tweet"];
        }
    }
}
