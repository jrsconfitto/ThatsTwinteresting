using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyCSharpWORK.PI_AF
{
    public class TwitterQueryModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public DateTime queryTime { get; set; }

        public List<TweetModel> tweets { get; set; }
    }

    public class TweetModel
    {
        public string id { get; set; }
        public string profile_url { get; set; }
        public string text { get; set; }
        public string user_name { get; set; }
    }
}
