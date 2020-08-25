using System;
using CustomUrlShortener.WebApp.Validation;

namespace CustomUrlShortener.WebApp.Models
{
    public class UrlViewModel
    {
        public int Id { get; set; }

        [ValidUrl]
        public string LongURL { get; set; }

        public string URLToShow { get; set; }

        public string ShortURL { get; set; }

        public DateTime Created { get; set; }

        public int ClickNumber { get; set; }
    }
}
