using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using CsvHelper;
using System.Globalization;

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
        public static IEnumerable<Fields> readCSV(String csv_file)
        {

            using (var reader = new StreamReader(csv_file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                /*var records = csv.GetRecords<Fields>();
                return records;
                */
                var records = new List<Fields>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Fields
                    {
                        projectName = csv.GetField<String>("projectName"),
                        projectDesc = csv.GetField<String>("projectDesc"),
                        projectGroup = csv.GetField<String>("projectGroup"),
                        projectStatus = csv.GetField<String>("projectStatus"),
                        projectType = csv.GetField<String>("projectType"),
                        projectStartDate = csv.GetField<String>("projectStartDate"),
                        projectEndDate = csv.GetField<String>("projectEndDate"),
                        projectNumber = csv.GetField<String>("projectNumber"),
                        projectManagerId = csv.GetField<String>("projectManagerId"),
                        projectCoordinatorId = csv.GetField<String>("projectCoordinatorId"),
                        projectAccountantId = csv.GetField<String>("projectAccountantId"),
                        federalProject = csv.GetField<String>("federalProject"),
                        projectAddress1 = csv.GetField<String>("projectAddress1"),
                        projectAddress2 = csv.GetField<String>("projectAddress2"),
                        projectCity = csv.GetField<String>("projectCity"),
                        projectState = csv.GetField<String>("projectState"),
                        projectZip = csv.GetField<String>("projectZip"),
                        projectID = csv.GetField<String>("projectID"),
                    };
                    records.Add(record);
                }
                return records;
            }
            
        }
    }
    public class Fields
    {
        public String projectName { get; set; }
        public String projectDesc { get; set; }
        public String projectGroup { get; set; }
        public String projectStatus { get; set; }
        public String projectType { get; set; }
        public String projectStartDate { get; set; }
        public String projectEndDate { get; set; }
        public String projectNumber { get; set; }
        public String projectManagerId { get; set; }
        public String projectCoordinatorId { get; set; }
        public String projectAccountantId { get; set; }
        public String federalProject { get; set; }
        public String projectAddress1 { get; set; }
        public String projectAddress2 { get; set; }
        public String projectCity { get; set; }
        public String projectState { get; set; }
        public String projectZip { get; set; }
        public String projectID { get; set; }

    }
}
