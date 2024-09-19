namespace CloudSavior.Objects
{
    public class BaseRAWGResponse
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<RAWGGame> results { get; set; }
    }

    public class BaseRAWGObject
    {
        public int id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
    }

    public class RAWGGame : BaseRAWGObject
    {
        
        public string released { get; set; }
        public string background_image { get; set; }
        public string rating { get; set; }
        public string rating_top { get; set; }
        public List<RAWGPlatform> platforms { get; set; }
    }

    public class RAWGPlatform : BaseRAWGObject
    {
        public int platform { get; set; }
        public string platform_slug { get; set; }
        public string platform_name { get; set; }
    }
}