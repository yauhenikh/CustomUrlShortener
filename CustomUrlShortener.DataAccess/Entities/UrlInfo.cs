using System;

namespace CustomUrlShortener.DataAccess.Entities
{
    public class UrlInfo
    {
        public int Id { get; set; }

        public string LongUrl { get; set; }

        public string Token { get; set; }

        public DateTime Created { get; set; }

        public int ClickNumber { get; set; }
    }
}
