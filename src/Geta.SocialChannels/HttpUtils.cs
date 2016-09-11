using System.IO;
using System.Net;

namespace Geta.SocialChannels
{
    public static class HttpUtils
    {
        private const string JsonContentType = "application/json; charset=utf-8";
        private const string PostMethod = "POST";
        private const string Authorization = "Authorization";

        public static string Get(string uri, string authHeader = null, string contentType = JsonContentType)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);

            webRequest.ContentType = JsonContentType;

            if (!string.IsNullOrEmpty(authHeader))
            {
                webRequest.Headers.Add(Authorization, authHeader);
            }

            return GetRequestData(webRequest);
        }

        public static string Post(string uri, string data, string authHeader = null, string contentType = JsonContentType)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);

            webRequest.ContentType = contentType;
            webRequest.Method = PostMethod;

            if (!string.IsNullOrEmpty(authHeader))
            {
                webRequest.Headers.Add(Authorization, authHeader);
            }

            if (string.IsNullOrEmpty(data)) return GetRequestData(webRequest);

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
            }

            return GetRequestData(webRequest);
        }

        private static string GetRequestData(WebRequest request)
        {
            var webResponse = (HttpWebResponse)request.GetResponse();

            if (webResponse.StatusCode != HttpStatusCode.OK) return string.Empty;

            using (var stream = webResponse.GetResponseStream())
            {
                if (stream == null) return string.Empty;

                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    return text;
                }
            }
        }
    }
}