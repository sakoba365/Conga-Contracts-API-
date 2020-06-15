using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Conga_Projects_API_Integration
{
    class Utils
    {
        public static String postRequest(String url, String content, List<KeyValuePair<String, String>> headers=null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/xml";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = false;
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;
            httpWebRequest.UserAgent = "PostmanRuntime/7.25.0";
            if (headers != null)
            {
                WebHeaderCollection header_c = new WebHeaderCollection();
                foreach (KeyValuePair<String, String> header_map in headers)
                {
                    header_c.Add(header_map.Key, header_map.Value);
                }
                httpWebRequest.Headers= header_c;
            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(content);
                //Console.WriteLine(content);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                String result = streamReader.ReadToEnd();
                return result;
            }
        }
    }
}
