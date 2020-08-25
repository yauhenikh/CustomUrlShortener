using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using CustomUrlShortener.WebApp.Models;

namespace CustomUrlShortener.WebApp.Validation
{
    public class ValidUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var url = (UrlViewModel)validationContext.ObjectInstance;

            if (string.IsNullOrEmpty(url.LongURL))
            {
                return new ValidationResult("URL is required");
            }

            if (!url.LongURL.StartsWith("http://") && !url.LongURL.StartsWith("https://"))
            {
                return new ValidationResult("URL must begin with \"http://\" or \"https://\"");
            }

            Uri urlCheck = new Uri(url.LongURL);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
            request.Timeout = 10000;

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception)
            {
                return new ValidationResult("URL is invalid");
            }

            return ValidationResult.Success;
        }
    }
}
