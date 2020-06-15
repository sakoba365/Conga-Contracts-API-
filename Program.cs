using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Conga_Projects_API_Integration
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                String username = "";
                String password = "";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://app1.congacontracts.com/Contracts/wsapi/v1/Session");
                httpWebRequest.ContentType = "text/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.ProtocolVersion = HttpVersion.Version11;
                httpWebRequest.UserAgent = "PostmanRuntime/7.25.0";
                WebHeaderCollection header_c = new WebHeaderCollection();
                header_c.Add("Content-Type", "text/xml");
                //httpWebRequest.Headers.Add("Content-Type", "text/xml");// headers = header_c;

                /*var content = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ses=\"http://novatuscontracts.com/api/v1/session\">" +
                                "< soapenv:Header />" +
                                "< soapenv:Body >" +
                                    "< ses:login >" +
                                         "< username > it@oacsvcs.com </ username >" +
                                         "< password > Lk882008 </ password >" +
                                    "</ ses:login >" +
                                "</ soapenv:Body >" +
              "</ soapenv:Envelope > ";*/
                var content = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ses=\"http://novatuscontracts.com/api/v1/session\">\r\n   <soapenv:Header/>\r\n   <soapenv:Body>\r\n      <ses:login>\r\n         <!--Optional:-->\r\n         <username>it@oacsvcs.com</username>\r\n         <!--Optional:-->\r\n         <password>Lk882008</password>\r\n      </ses:login>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";
                StringBuilder content_bulder = new StringBuilder();
                content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ses=\"http://novatuscontracts.com/api/v1/session\">");
                content_bulder.Append("<soapenv:Header/>");
                content_bulder.Append("<soapenv:Body>");
                content_bulder.Append("<ses:login>");
                content_bulder.Append("<username>it@oacsvcs.com</username>");
                content_bulder.Append("<password>Lk882008</password>");
                content_bulder.Append("</ses:login>");
                content_bulder.Append("</soapenv:Body>");
                content_bulder.Append("</soapenv:Envelope>");
                //content_bulder.Append("");
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(content.ToString());//(content);
                    //Console.WriteLine(content.ToString());//(content);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //Console.WriteLine("Received from server: " + result);
                    XElement envelope = XElement.Parse(result);
                    //Console.WriteLine("envelope " + envelope);
                    //Console.WriteLine("Elem " + envelope.Descendants("loginResponse"));

                    XDocument doc = XDocument.Parse(result);
                    XNamespace xmlns = "http://novatuscontracts.com/api/v1/session";

                    var orderNode = doc.Descendants(xmlns + "loginResponse").Elements("sessionId");

                    /*var value = from o in orderNode.Attributes("OrderNumber")
                                select o.Value;*/
                    var value = from o in orderNode select o.Value;
                    //Console.WriteLine("value " + value.Count());
                    foreach (String id in value)
                    {

                        Console.WriteLine("id " + id);
                    }

                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error writing to the server: "+ err);
            }

            Conga cong = new Conga("https://app1.congacontracts.com/Contracts/wsapi/v1/");
            String session_id = cong.login("Session", "it@oacsvcs.com", "Lk882008");
            Console.WriteLine("session_id: " + session_id);
            /*(String path, String sessionid, String description, List<KeyValuePair<string, String>> dynamics, String endDate, String group
                                    , String id, String name, String startDate, String status, bool documentUpload, bool document, bool modify, String addressLine1
            , String addressLine2, String cellAreaCode, String cellCountryCode, String cellExtension, String cellNumber, String city, String country, bool disabled
            , String emailAddress, String faxAreaCode, String faxCountryCode, String faxExtension, String faxNumber, String firstName, bool forcePasswordChange
            , List<String> functions, String homeAreaCode, String homeCountryCode, String homeExtension, String homeNumber, String lastName, String loginName
            , String mailStop, String middleName, String notes, String organization, String password, String postalCode, String prefix, List<String> roles
            , String state, String title, String workAreaCode, String workCountryCode, String workExtension, String workNumber, String type)*/
            String create_result = cong.createProject("Project", session_id, "Project Description", new List<KeyValuePair<String, String>>(), "2020-09-30", "Sako Group"
                                                     , "058fc186-be30-4828-83db-c909e24954bDFD", "Sako", "2020-06-30", "Started", true, true, true, "Address line 1", "Address Line2"
                                                     , "001", "1", "69", "01524525", "NY", "USA", true, "sako@email.com", "1", "01", "01", "052525", "Sako", true
                                                     , new List<String>(), "001", "1", "6", "2541525", "Adams", "testUser", "mailStop", "middleName", "notes"
                                                     , "organization", "Demo#2020", "WS205", "prefix", new List<String>(), "state", "Test Project 2"
                                                     , "0001", "102", "85", "01", "Pending");
            Console.WriteLine("create_result: " + create_result);

            Console.ReadKey();

        }
    }
    class Conga
    {
        String endpoint;
        public Conga(String endpoint)
        {
            this.endpoint = endpoint;
        }
        public String login(String path, String user_name, String password)
        {
            String session_id = "";
            try
            {
                String url = this.endpoint + path;
                StringBuilder content_bulder = new StringBuilder();
                content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ses=\"http://novatuscontracts.com/api/v1/session\">");
                content_bulder.Append("<soapenv:Header/>");
                content_bulder.Append("<soapenv:Body>");
                content_bulder.Append("<ses:login>");
                content_bulder.Append("<username>"+user_name+"</username>");
                content_bulder.Append("<password>"+password+"</password>");
                content_bulder.Append("</ses:login>");
                content_bulder.Append("</soapenv:Body>");
                content_bulder.Append("</soapenv:Envelope>");

                String result = Utils.postRequest(url, content_bulder.ToString());
                XDocument doc = XDocument.Parse(result);
                XNamespace xmlns = "http://novatuscontracts.com/api/v1/session";

                var loginResponseNode = doc.Descendants(xmlns + "loginResponse").Elements("sessionId");

                /*var value = from o in orderNode.Attributes("OrderNumber")
                            select o.Value;*/
                var value = from o in loginResponseNode select o.Value;
                //Console.WriteLine("value " + value.Count());
                foreach (String id in value)
                {

                    //Console.WriteLine("id " + id);
                    session_id = id;
                }
            }
            catch(Exception err)
            {
                Console.WriteLine("Login Error: " + err);
            }
            return session_id;
        }
        public String createProject(String path, String sessionid, String description, List<KeyValuePair<string, String>> dynamics, String endDate, String group
                                    , String id, String name, String startDate, String status, bool documentUpload, bool document, bool modify, String addressLine1
            , String addressLine2, String cellAreaCode, String cellCountryCode, String cellExtension, String cellNumber, String city, String country, bool disabled
            , String emailAddress, String faxAreaCode, String faxCountryCode, String faxExtension, String faxNumber, String firstName, bool forcePasswordChange
            , List<String> functions, String homeAreaCode, String homeCountryCode, String homeExtension, String homeNumber, String lastName, String loginName
            , String mailStop, String middleName,String notes, String organization, String password, String postalCode, String prefix, List<String> roles
            , String state, String title, String workAreaCode, String workCountryCode, String workExtension, String workNumber, String type)
        {
            String result = "";
            try
            {
                String url = this.endpoint + path;
                StringBuilder content_bulder = new StringBuilder();
                content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:proj=\"http://novatuscontracts.com/api/v1/project\">");
                content_bulder.Append("<soapenv:Header/>");
                content_bulder.Append("<soapenv:Body>");
                content_bulder.Append("<proj:create>");
                content_bulder.Append("<project>");
                content_bulder.Append("<description>" + description + "</description>");
                content_bulder.Append("<dynamics>");
                content_bulder.Append("<!--Zero or more repetitions:-->");
                foreach (KeyValuePair<string, String> entry in dynamics)
                {
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>" + entry.Key + "</key>");
                    content_bulder.Append("<value>" + entry.Value + " </value>");
                    content_bulder.Append("</entry>");
                }
                content_bulder.Append("</dynamics>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<endDate>" + endDate + "</endDate>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<group>" + group + "</group>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<id>" + id + "</id>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<name>" + name + "</name>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<startDate>" + startDate + "</startDate>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<status>" + status + "</status>");
                content_bulder.Append("<!--Zero or more repetitions:-->");
                content_bulder.Append("<teamMembers>");
                content_bulder.Append("<documentUpload>" + documentUpload + "</documentUpload>");
                content_bulder.Append("<documents>" + document + "</documents>");
                content_bulder.Append("<modify>" + modify + "</modify>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<user>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<addressLine1>" + addressLine1 + "</addressLine1>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<addressLine2>" + addressLine2 + "</addressLine2>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<cellAreaCode>" + cellAreaCode + "</cellAreaCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<cellCountryCode>" + cellCountryCode + "</cellCountryCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<cellExtension>" + cellExtension + "</cellExtension>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<cellNumber>" + cellNumber + "</cellNumber>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<city>" + city + "</city>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<country>" + country + "</country>");
                content_bulder.Append("<disabled>" + disabled + "</disabled>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<emailAddress>" + emailAddress + "</emailAddress>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<faxAreaCode>" + faxAreaCode + "</faxAreaCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<faxCountryCode>" + faxCountryCode + "</faxCountryCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<faxExtension>" + faxExtension + "</faxExtension>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<faxNumber>" + faxNumber + "</faxNumber>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<firstName>" + firstName + "</firstName>");
                content_bulder.Append("<forcePasswordChange>" + forcePasswordChange + "</forcePasswordChange>");
                content_bulder.Append("<!--Zero or more repetitions:-->");
                foreach (String function in functions)
                {
                    content_bulder.Append("<functions>" + function + "</functions>");
                }
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<homeAreaCode>" + homeAreaCode + "</homeAreaCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<homeCountryCode>" + homeCountryCode + "</homeCountryCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<homeExtension>" + homeExtension + "</homeExtension>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<homeNumber>" + homeNumber + "</homeNumber>");
                content_bulder.Append("<!--Optional:-->");
                //content_bulder.Append("<id>?</id>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<lastName>" + lastName + "</lastName>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<loginName>" + loginName + "</loginName>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<mailStop>" + mailStop + "</mailStop>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<middleName>" + middleName + "</middleName>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<notes>" + notes + "</notes>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<organization>" + organization + "</organization>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<password>" + password + "</password>");
                content_bulder.Append("<!--Optional:-->");
                //content_bulder.Append("<personId>?</personId>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<postalCode>" + postalCode + "</postalCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<prefix>" + prefix + "</prefix>");
                content_bulder.Append("<!--Zero or more repetitions:-->");
                foreach (String role in roles)
                {
                    content_bulder.Append("<roles>" + role + "</roles>");
                }
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<state>" + state + "</state>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<title>" + title + "</title>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<workAreaCode>" + workAreaCode + "</workAreaCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<workCountryCode>" + workCountryCode + "</workCountryCode>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<workExtension>" + workExtension + "</workExtension>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<workNumber>" + workNumber + "</workNumber>");
                content_bulder.Append("</user>");
                content_bulder.Append("</teamMembers>");
                content_bulder.Append("<!--Optional:-->");
                content_bulder.Append("<type>" + type + "</type>");
                content_bulder.Append("</project>");
                content_bulder.Append("</proj:create>");
                content_bulder.Append("</soapenv:Body>");
                content_bulder.Append("</soapenv:Envelope>");

                List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                    new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                    new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                };
                result = Utils.postRequest(url, content_bulder.ToString(), headers);

            }
            catch (WebException err)
            {
                //Console.WriteLine("Create Project Error: " + err);
                //Console.WriteLine("Result: " + err.Response.ContentType);
                using (var streamReader = new StreamReader(err.Response.GetResponseStream()))
                {
                    String result_text = streamReader.ReadToEnd();
                    Console.WriteLine("Error Create Project: " + result_text);
                    //Console.WriteLine("Message: " + err.Message);
                }
                return err.Message;
            }
            return result;
        }
    }

}


