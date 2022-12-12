using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace FileUplaodAz_Core.Services
{
    public class ReCaptchaResponse
    {
        public bool success
        {
            get;
            set;
        }
        public string challenge_ts
        {
            get;
            set;
        }
        public string hostname
        {
            get;
            set;
        }
        [JsonProperty(PropertyName = "error-codes")]
        public List<string> error_codes
        {
            get;
            set;
        }
    }
    public class RecaptchaService: IRecaptchaService
    {
        private readonly IConfiguration _config;

        public RecaptchaService(IConfiguration config)
        {
            _config=config;
        }
        public bool RecaptchaRequest(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return false;
            }
            var serverKey = _config.GetSection("Logging:SecretKey:recaptchaServer").Value;
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(serverKey, response);
            if (!reCaptchaResponse.success)
            {
                return false;
            }
            return true;
        }
        public ReCaptchaResponse VerifyCaptcha(string secret, string response)
        {

                using (System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient())
                {
                    var values = new Dictionary<string,
                        string> {
                        {
                            "secret",
                            secret
                        },
                        {
                            "response",
                            response
                        }
                    };
                    var content = new System.Net.Http.FormUrlEncodedContent(values);
                    var Response = hc.PostAsync("https://www.google.com/recaptcha/api/siteverify", content).Result;  
                    var responseString = Response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        ReCaptchaResponse rspns = JsonConvert.DeserializeObject<ReCaptchaResponse>(responseString);
                        return rspns;
                    }
                    else
                        return new ReCaptchaResponse
                        {
                            success = false
                        }; 
                }
        }
        
    }
}
