using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterQueryer.Twitter
{
  public class Geo
  {
      public List<double> coordinates { get; set; }
      public string type { get; set; }
  }

  public class Metadata
  {
      public string result_type { get; set; }
  }

  public class Result
  {
      public string created_at { get; set; }
      public string from_user { get; set; }
      public int from_user_id { get; set; }
      public string from_user_id_str { get; set; }
      public string from_user_name { get; set; }
      public Geo geo { get; set; }
      public string id { get; set; }
      public string id_str { get; set; }
      public string iso_language_code { get; set; }
      public Metadata metadata { get; set; }
      public string profile_image_url { get; set; }
      public string profile_image_url_https { get; set; }
      public string source { get; set; }
      public string text { get; set; }
      public string to_user { get; set; }
      public int to_user_id { get; set; }
      public string to_user_id_str { get; set; }
      public string to_user_name { get; set; }
      public long in_reply_to_status_id { get; set; }
      public string in_reply_to_status_id_str { get; set; }
  }

  public class TweetResponse
  {
      public double completed_in { get; set; }
      public long max_id { get; set; }
      public string max_id_str { get; set; }
      public string next_page { get; set; }
      public int page { get; set; }
      public string query { get; set; }
      public string refresh_url { get; set; }
      public List<Result> results { get; set; }
      public int results_per_page { get; set; }
      public int since_id { get; set; }
      public string since_id_str { get; set; }
  }
}
