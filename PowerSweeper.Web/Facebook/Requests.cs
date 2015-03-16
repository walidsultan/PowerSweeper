using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace PowerSweeper.Web.Facebook
{
    public class Requests
    {
        public static string GetResponse(string url)
        {
            try
            {
                string responseString;

                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";

                // Get the response.
                using (WebResponse response = request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    Stream responseStream = response.GetResponseStream();

                    // Read the response...
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        responseString = reader.ReadToEnd();
                    }
                }

                return responseString;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetResponseUri(string url)
        {
            string responseString;

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";

            // Get the response.
            using (WebResponse response = request.GetResponse())
            {
                responseString = response.ResponseUri.AbsoluteUri;
            }

            return responseString;
        }
    }
}