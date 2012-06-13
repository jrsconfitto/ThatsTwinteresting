using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hackathon.TwitterAPI
{
    public class Params
    {
        public string query { get; set; }
        public string granularity { get; set; }
        public bool trim_place { get; set; }
        public int accuracy { get; set; }
        public bool autocomplete { get; set; }
    }

    public class Query
    {
        public string type { get; set; }
        public string url { get; set; }
        public Params @params { get; set; }
    }

    public class Attributes
    {
    }

    public class Attributes2
    {
    }

    public class BoundingBox
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }

    public class ContainedWithin
    {
        public string name { get; set; }
        public string place_type { get; set; }
        public Attributes2 attributes { get; set; }
        public string full_name { get; set; }
        public string url { get; set; }
        public string country { get; set; }
        public string id { get; set; }
        public string country_code { get; set; }
        public BoundingBox bounding_box { get; set; }
    }

    public class BoundingBox2
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }

    public class Place
    {
        public string place_type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string full_name { get; set; }
        public Attributes attributes { get; set; }
        public List<ContainedWithin> contained_within { get; set; }
        public string country { get; set; }
        public string id { get; set; }
        public BoundingBox2 bounding_box { get; set; }
        public string country_code { get; set; }
    }

    public class Result
    {
        public List<Place> places { get; set; }
    }

    public class TwitterGeoResult
    {
        public Query query { get; set; }
        public Result result { get; set; }
    }
}
