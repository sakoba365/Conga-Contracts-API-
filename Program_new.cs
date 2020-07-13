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
                String username = "it@oacsvcs.com";
                String password = "Lk882008";
                Conga cong = new Conga("https://app1.congacontracts.com/Contracts/wsapi/v1/");
                String session_id = cong.login("Session", username, password);
                Console.WriteLine("session_id: " + session_id);
                /*List<UserFields> res = cong1.get_list_users("User", session_id);
                foreach(UserFields user in res)
                {
                    Console.WriteLine(user.firstName);
                }
                cong1.createCompany(session_id1, "address1", "address2", "category", "city", "country", "dunsNumber",
                                    "faxAreaCode", "faxCountryCode", "faxExtension", "faxNumber", "group", "name", "number",
                                    "phoneAreaCode", "phoneCountryCode", "phoneExtension", "phoneNumber", "postalCode", "state",
                                    "status", "taxId", "type", "url", new List<Dictionary<String, String>>());
                cong1.updateCompany(session_id1, "CompanyId", "address1", "address2", "category", "city", "country", "dunsNumber",
                    "faxAreaCode", "faxCountryCode", "faxExtension", "faxNumber", "group", "name", "number",
                    "phoneAreaCode", "phoneCountryCode", "phoneExtension", "phoneNumber", "postalCode", "state",
                    "status", "taxId", "type", "url", new List<Dictionary<String, String>>());
                //Console.WriteLine(res);
                Console.ReadKey();
                if (1 < 2)
                    return;
                */

                Console.WriteLine("##################### WELCOM To Conga CSV Uploader ################################################");
                Console.WriteLine("1 - To upload Group Project");
                Console.WriteLine("2 - To upload Team Project");
                Console.WriteLine("3 - List Users");
                Console.WriteLine("4 - Create Company");
                Console.WriteLine("5 - Update Company");
                Console.Write("Enter (1, 2, 3, 4, or 5): ");
                String entry = Console.ReadLine();
                while(entry !="1" && entry != "2" && entry != "3" && entry != "4" && entry != "5")
                {
                    Console.WriteLine("Wrong Operation!=> '"+entry+"'");
                    Console.Write("Enter (1, 2, 3, 4, or 5): ");
                    entry = Console.ReadLine();
                }
                Console.WriteLine("*********************************");
                if (entry == "1" || entry == "2")
                {
                    Console.WriteLine("1 - To Create Project");
                    Console.WriteLine("2 - To Update Project");
                    Console.Write("Enter (1 or 2): ");
                    String entry2 = Console.ReadLine();
                    while (entry2 != "1" && entry2 != "2")
                    {
                        Console.WriteLine("Wrong Operation!=> '" + entry2 + "'");
                        Console.Write("Enter (1 or 2): ");
                        entry2 = Console.ReadLine();
                    }
                    Console.WriteLine("*********************************");

                    Console.Write("Enter CSV full path: ");
                    String csv_path = Console.ReadLine();

                    Console.WriteLine("*********************************");
                    Console.WriteLine("---------------------------------------------------------------------");
                    if (entry == "1")
                    {
                        if (entry2 == "1")
                        {
                            String create_result_group = cong.createProject_group("Project", session_id, csv_path);
                        }
                        else
                        {
                            String update_result_grup = cong.updateProject_group("Project", session_id, csv_path);
                        }
                    }
                    else
                    {
                        if (entry2 == "1")
                        {
                            String update_result_team = cong.createProject_team("Project", session_id, csv_path);

                        }
                        else
                        {
                            String create_result_team = cong.updateProject_team("Project", session_id, csv_path);
                        }
                    }
                }else if(entry == "3")
                {
                    List<UserFields> res = cong.get_list_users("User", session_id);
                    foreach (UserFields user in res)
                    {
                        Console.WriteLine(user.firstName);
                    }

                }
                else if (entry == "4")
                {
                    String res = cong.createCompany(session_id, "address1", "address2", "category", "city", "country", "dunsNumber",
                                        "faxAreaCode", "faxCountryCode", "faxExtension", "faxNumber", "group", "name", "number",
                                        "phoneAreaCode", "phoneCountryCode", "phoneExtension", "phoneNumber", "postalCode", "state",
                                        "status", "taxId", "type", "url", new List<Dictionary<String, String>>());
                    Console.WriteLine(res);
                }
                else if (entry == "5")
                {
                    String res = cong.updateCompany(session_id, "CompanyId", "address1", "address2", "category", "city", "country", "dunsNumber",
                        "faxAreaCode", "faxCountryCode", "faxExtension", "faxNumber", "group", "name", "number",
                        "phoneAreaCode", "phoneCountryCode", "phoneExtension", "phoneNumber", "postalCode", "state",
                        "status", "taxId", "type", "url", new List<Dictionary<String, String>>());
                    Console.WriteLine(res);
                }
                Console.WriteLine("All done!");
                Console.ReadKey();

            }
            catch (Exception err)
            {
                Console.WriteLine("Error writing to the server: " + err.Message);
                Console.ReadKey();
            }
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
                Console.WriteLine("value " + value.Count());
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
        public String createProject_group(String path, String sessionid, String csv_path)
        {
            String result = "";
            var records = Utils.readCSV(csv_path);
            //Console.WriteLine(records);
            foreach (Fields rc in records)
            {
                Console.WriteLine("Creating Project - "+rc.projectName+"-");
                try
                {
                    String url = this.endpoint + path;
                    StringBuilder content_bulder = new StringBuilder();

                    content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:proj=\"http://novatuscontracts.com/api/v1/project\">");
                    content_bulder.Append("<soapenv:Header/>");
                    content_bulder.Append("<soapenv:Body>");
                    content_bulder.Append("<proj:create>");
                    content_bulder.Append("<project>");
                    content_bulder.Append("<description>"+ rc.projectDesc+ "</description>");
                    content_bulder.Append("<dynamics>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c180775b4956496a95de45f9f1330141</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectNumber + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_4a46ed6e3986430ab63c15bff76fd145</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.federalProject + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_36623ba9ab7b47838d64fb95ee8a3d5f</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectManagerId+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_3e9b0a21862240c98b8f8f932871c38e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectCoordinatorId+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5308cec9a58b45c9bedbe36db9787af1</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectZip+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_6ba320b9ed7e4d75b1c9217b5a5034dd</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectAddress1+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_8822846af50a41fe9dea25d01de0420d</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectAccountantId+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_d4d350e3020547629e06eda06967564a</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectCity+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_1a773db5dfa24430b84537ad47c598dc</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\"></value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c23ad17d4fb84b9d846bf5775490824e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectAddress2+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5b9dbad4fe314dd7822295d7ccee4489</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">"+rc.projectState+"</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("</dynamics>");
                    content_bulder.Append("<group>"+rc.projectGroup+"</group>");
                    content_bulder.Append("<name>"+rc.projectName+"</name>");
                    content_bulder.Append("<status>"+rc.projectStatus+"</status>");
                    content_bulder.Append("<type>"+rc.projectType+"</type>");
                    content_bulder.Append("<startDate>" + rc.projectStartDate + "</startDate>");
                    content_bulder.Append("<endDate>" + rc.projectEndDate + "</endDate>");
                    content_bulder.Append("</project>");
                    content_bulder.Append("</proj:create>");
                    content_bulder.Append("</soapenv:Body>");
                    content_bulder.Append("</soapenv:Envelope>");

                    List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                        new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                        new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                    };
                    result = Utils.postRequest(url, content_bulder.ToString(), headers);
                    Console.WriteLine("Project Successfully created: " + result);
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
                    result = err.Message;
                }
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            }
            return result;
        }
        public String updateProject_group(String path, String sessionid, String csv_path)
        {
            String result = "";
            var records = Utils.readCSV(csv_path);
            //Console.WriteLine(records);
            foreach (Fields rc in records)
            {
                Console.WriteLine("Updating Project - " + rc.projectName + "-");
                try
                {
                    String url = this.endpoint + path;
                    StringBuilder content_bulder = new StringBuilder();

                    content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:proj=\"http://novatuscontracts.com/api/v1/project\">");
                    content_bulder.Append("<soapenv:Header/>");
                    content_bulder.Append("<soapenv:Body>");
                    content_bulder.Append("<proj:update>");
                    content_bulder.Append("<project>");
                    content_bulder.Append("<id>" + rc.projectID + "</id>");
                    content_bulder.Append("<description>" + rc.projectDesc + "</description>");
                    content_bulder.Append("<dynamics>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c180775b4956496a95de45f9f1330141</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectNumber + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_4a46ed6e3986430ab63c15bff76fd145</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.federalProject + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_36623ba9ab7b47838d64fb95ee8a3d5f</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectManagerId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_3e9b0a21862240c98b8f8f932871c38e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectCoordinatorId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5308cec9a58b45c9bedbe36db9787af1</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectZip + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_6ba320b9ed7e4d75b1c9217b5a5034dd</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAddress1 + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_8822846af50a41fe9dea25d01de0420d</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAccountantId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_d4d350e3020547629e06eda06967564a</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectCity + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_1a773db5dfa24430b84537ad47c598dc</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\"></value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c23ad17d4fb84b9d846bf5775490824e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAddress2 + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5b9dbad4fe314dd7822295d7ccee4489</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectState + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("</dynamics>");
                    content_bulder.Append("<group>" + rc.projectGroup + "</group>");
                    content_bulder.Append("<name>" + rc.projectName + "</name>");
                    content_bulder.Append("<status>" + rc.projectStatus + "</status>");
                    content_bulder.Append("<type>" + rc.projectType + "</type>");
                    content_bulder.Append("<startDate>" + rc.projectStartDate + "</startDate>");
                    content_bulder.Append("<endDate>" + rc.projectEndDate + "</endDate>");
                    content_bulder.Append("</project>");
                    content_bulder.Append("</proj:update>");
                    content_bulder.Append("</soapenv:Body>");
                    content_bulder.Append("</soapenv:Envelope>");

                    List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                        new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                        new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                    };
                    result = Utils.postRequest(url, content_bulder.ToString(), headers);
                    Console.WriteLine("Project Successfully updated: " + result);
                }
                catch (WebException err)
                {
                    //Console.WriteLine("Create Project Error: " + err);
                    //Console.WriteLine("Result: " + err.Response.ContentType);
                    using (var streamReader = new StreamReader(err.Response.GetResponseStream()))
                    {
                        String result_text = streamReader.ReadToEnd();
                        Console.WriteLine("Error Updating Project: " + result_text);
                        //Console.WriteLine("Message: " + err.Message);
                    }
                    result = err.Message;
                }
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            }
            return result;
        }
        public String createProject_team(String path, String sessionid, String csv_path)
        {
            String result = "";
            var records = Utils.readCSV2(csv_path);
            //Console.WriteLine(records);
            foreach (Fields2 rc in records)
            {
                Console.WriteLine("Creating Team Project - " + rc.projectName + "-");
                try
                {
                    String url = this.endpoint + path;
                    StringBuilder content_bulder = new StringBuilder();

                    content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:proj=\"http://novatuscontracts.com/api/v1/project\">");
                    content_bulder.Append("<soapenv:Header/>");
                    content_bulder.Append("<soapenv:Body>");
                    content_bulder.Append("<proj:create>");
                    content_bulder.Append("<project>");
                    content_bulder.Append("<description>" + rc.projectDesc + "</description>");
                    content_bulder.Append("<dynamics>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c180775b4956496a95de45f9f1330141</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectNumber + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_4a46ed6e3986430ab63c15bff76fd145</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.federalProject + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_36623ba9ab7b47838d64fb95ee8a3d5f</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectManagerId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_3e9b0a21862240c98b8f8f932871c38e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectCoordinatorId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5308cec9a58b45c9bedbe36db9787af1</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectZip + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_6ba320b9ed7e4d75b1c9217b5a5034dd</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAddress1 + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_8822846af50a41fe9dea25d01de0420d</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAccountantId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_d4d350e3020547629e06eda06967564a</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectCity + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_1a773db5dfa24430b84537ad47c598dc</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\"></value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c23ad17d4fb84b9d846bf5775490824e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAddress2 + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5b9dbad4fe314dd7822295d7ccee4489</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectState + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("</dynamics>");                    
                    content_bulder.Append("<name>" + rc.projectName + "</name>");
                    content_bulder.Append("<status>" + rc.projectStatus + "</status>");
                    content_bulder.Append("<type>" + rc.projectType + "</type>");
                    content_bulder.Append("<startDate>" + rc.projectStartDate + "</startDate>");
                    content_bulder.Append("<endDate>" + rc.projectEndDate + "</endDate>");

                    content_bulder.Append("<teamMembers>");
                    content_bulder.Append("<documentUpload>" + rc.docUpload + "</documentUpload>");
                    content_bulder.Append("<documents>" + rc.documents + "</documents>");
                    content_bulder.Append("<modify>" + rc.modify + "</modify>");
                    content_bulder.Append("<user>");
                    content_bulder.Append("<id>" + rc.teamID + "</id>");
                    content_bulder.Append("</user>");
                    content_bulder.Append("</teamMembers>");

                    content_bulder.Append("</project>");
                    content_bulder.Append("</proj:create>");
                    content_bulder.Append("</soapenv:Body>");
                    content_bulder.Append("</soapenv:Envelope>");

                    List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                        new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                        new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                    };
                    //Console.WriteLine(content_bulder.ToString());
                    result = Utils.postRequest(url, content_bulder.ToString(), headers);
                    Console.WriteLine("Project Successfully created: " + result);
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
                    result = err.Message;
                }
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            }
            return result;
        }
        public String updateProject_team(String path, String sessionid, String csv_path)
        {
            String result = "";
            var records = Utils.readCSV2(csv_path);
            //Console.WriteLine(records);
            foreach (Fields2 rc in records)
            {
                Console.WriteLine("Updating Project - " + rc.projectName + "-");
                try
                {
                    String url = this.endpoint + path;
                    StringBuilder content_bulder = new StringBuilder();

                    content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:proj=\"http://novatuscontracts.com/api/v1/project\">");
                    content_bulder.Append("<soapenv:Header/>");
                    content_bulder.Append("<soapenv:Body>");
                    content_bulder.Append("<proj:update>");
                    content_bulder.Append("<project>");
                    content_bulder.Append("<id>" + rc.projectID + "</id>");
                    content_bulder.Append("<description>" + rc.projectDesc + "</description>");
                    content_bulder.Append("<dynamics>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c180775b4956496a95de45f9f1330141</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectNumber + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_4a46ed6e3986430ab63c15bff76fd145</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.federalProject + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_36623ba9ab7b47838d64fb95ee8a3d5f</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectManagerId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_3e9b0a21862240c98b8f8f932871c38e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectCoordinatorId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5308cec9a58b45c9bedbe36db9787af1</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectZip + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_6ba320b9ed7e4d75b1c9217b5a5034dd</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAddress1 + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_8822846af50a41fe9dea25d01de0420d</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAccountantId + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_d4d350e3020547629e06eda06967564a</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectCity + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_1a773db5dfa24430b84537ad47c598dc</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\"></value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_c23ad17d4fb84b9d846bf5775490824e</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectAddress2 + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("<entry>");
                    content_bulder.Append("<key>do_5b9dbad4fe314dd7822295d7ccee4489</key>");
                    content_bulder.Append("<value xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:type=\"xs:string\">" + rc.projectState + "</value>");
                    content_bulder.Append("</entry>");
                    content_bulder.Append("</dynamics>");
                    content_bulder.Append("<name>" + rc.projectName + "</name>");
                    content_bulder.Append("<status>" + rc.projectStatus + "</status>");
                    content_bulder.Append("<type>" + rc.projectType + "</type>");
                    content_bulder.Append("<startDate>" + rc.projectStartDate + "</startDate>");
                    content_bulder.Append("<endDate>" + rc.projectEndDate + "</endDate>");

                    content_bulder.Append("<teamMembers>");
                    content_bulder.Append("<documentUpload>" + rc.docUpload + "</documentUpload>");
                    content_bulder.Append("<documents>" + rc.documents + "</documents>");
                    content_bulder.Append("<modify>" + rc.modify + "</modify>");
                    content_bulder.Append("<user>");
                    content_bulder.Append("<id>" + rc.teamID + "</id>");
                    content_bulder.Append("</user>");
                    content_bulder.Append("</teamMembers>");

                    content_bulder.Append("</project>");
                    content_bulder.Append("</proj:update>");
                    content_bulder.Append("</soapenv:Body>");
                    content_bulder.Append("</soapenv:Envelope>");

                    List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                        new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                        new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                    };
                    result = Utils.postRequest(url, content_bulder.ToString(), headers);
                    Console.WriteLine("Project Successfully updated: " + result);
                }
                catch (WebException err)
                {
                    //Console.WriteLine("Create Project Error: " + err);
                    //Console.WriteLine("Result: " + err.Response.ContentType);
                    using (var streamReader = new StreamReader(err.Response.GetResponseStream()))
                    {
                        String result_text = streamReader.ReadToEnd();
                        Console.WriteLine("Error Updating Project: " + result_text);
                        //Console.WriteLine("Message: " + err.Message);
                    }
                    result = err.Message;
                }
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
            }
            return result;
        }
        public List<UserFields> get_list_users(String path, String sessionid, int pageSize=500, int page=0 )
        {
            List<UserFields> result = new List<UserFields>();
            
            try
            {
                String url = this.endpoint + path;
                StringBuilder content_bulder = new StringBuilder();
                content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:user=\"http://novatuscontracts.com/api/v1/user\">");
                content_bulder.Append("<soapenv:Header/>");
                content_bulder.Append("<soapenv:Body>");
                content_bulder.Append("<user:query>");
                content_bulder.Append("<query></query>");
                content_bulder.Append("<pageSize>"+pageSize+"</pageSize>");
                content_bulder.Append("<page>"+page+"</page>");
                content_bulder.Append("</user:query>");
                content_bulder.Append("</soapenv:Body>");
                content_bulder.Append("</soapenv:Envelope>");

                //Console.WriteLine(content_bulder.ToString());

                List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                        new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                        new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                    };
                String reqresult = Utils.postRequest(url, content_bulder.ToString(), headers);
                //Console.WriteLine("Project Successfully updated: " + result);

                XDocument doc = XDocument.Parse(reqresult);
                XNamespace xmlns = "http://novatuscontracts.com/api/v1/user";

                var queryResultNode = doc.Descendants(xmlns + "queryResponse").Elements("queryResult").Elements("userList");//("totalCount");
                var value = from o in queryResultNode select o;
                foreach (XElement elem in value)
                {
                    var record = new UserFields
                    {
                        id = elem.Element("id").Value,
                        addressLine1 = elem.Element("addressLine1").Value,
                        addressLine2 = elem.Element("addressLine2").Value,
                        city = elem.Element("city").Value,
                        disabled = elem.Element("disabled").Value,
                        emailAddress = elem.Element("emailAddress").Value,
                        firstName = elem.Element("firstName").Value,
                        forcePasswordChange = elem.Element("forcePasswordChange").Value,
                        functions = elem.Element("functions").Value,
                        lastName = elem.Element("lastName").Value,
                        loginName = elem.Element("loginName").Value,
                        mailStop = elem.Element("mailStop").Value,
                        middleName = elem.Element("middleName").Value,
                        notes = elem.Element("notes").Value,
                        organization = elem.Element("organization").Value,
                        personId = elem.Element("personId").Value,
                        postalCode = elem.Element("postalCode").Value,
                        prefix = elem.Element("prefix").Value,
                        roles = elem.Element("roles").Value,
                        title = elem.Element("title").Value


                    };
                    result.Add(record);
                }
            }
            catch (WebException err)
            {
                using (var streamReader = new StreamReader(err.Response.GetResponseStream()))
                {
                    String result_text = streamReader.ReadToEnd();
                    Console.WriteLine("Error loading userst: " + result_text);
                    //Console.WriteLine("Message: " + err.Message);
                }
                //result = err.Message;
            }
            return result;
        }
        public String createCompany(String sessionid, String address1, String address2, String category, String city, String country, String dunsNumber,
                                    String faxAreaCode, String faxCountryCode, String faxExtension, String faxNumber, String group, String name, String number,
                                    String phoneAreaCode, String phoneCountryCode, String phoneExtension, String phoneNumber, String postalCode, String state,
                                    String status, String taxId, String type, String url, List<Dictionary<String, String>> dynamics)
        {
            String reqresult = "";
            try
            {
                String apiURL = this.endpoint + "Company";
                StringBuilder content_bulder = new StringBuilder();
                content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:com=\"http://novatuscontracts.com/api/v1/company\">");
                content_bulder.Append("   <soapenv:Header/>");
                content_bulder.Append("   <soapenv:Body>");
                content_bulder.Append("      <com:create>");
                content_bulder.Append("         <company>");
                content_bulder.Append("            <address1>"+address1+"</address1>");
                content_bulder.Append("            <address2>" + address2 + "</address2>");
                content_bulder.Append("            <category>" + category + "</category>");
                content_bulder.Append("            <city>" + city + "</city>");
                content_bulder.Append("            <country>" + country + "</country>");
                content_bulder.Append("            <dunsNumber>" + dunsNumber + "</dunsNumber>");
                if (dynamics.Count() > 0)
                {
                    content_bulder.Append("            <dynamics>");
                    foreach (Dictionary<String, String> entry in dynamics)
                    {
                        content_bulder.Append("               <entry>");
                        content_bulder.Append("                  <key>" + entry["key"] + "</key>");
                        content_bulder.Append("                  <value>" + entry["value"] + " </value>");
                        content_bulder.Append("               </entry>");
                    }
                    content_bulder.Append("            </dynamics>");
                }
                content_bulder.Append("            <faxAreaCode>" + faxAreaCode + "</faxAreaCode>");
                content_bulder.Append("            <faxCountryCode>" + faxCountryCode + "</faxCountryCode>");
                content_bulder.Append("            <faxExtension>" + faxExtension + "</faxExtension>");
                content_bulder.Append("            <faxNumber>" + faxNumber + "</faxNumber>");
                content_bulder.Append("            <group>" + group + "</group>");
                content_bulder.Append("            <name>" + name + "</name>");
                content_bulder.Append("            <number>" + number + "</number>");
                content_bulder.Append("            <phoneAreaCode>" + phoneAreaCode + "</phoneAreaCode>");
                content_bulder.Append("            <phoneCountryCode>" + phoneCountryCode + "</phoneCountryCode>");
                content_bulder.Append("            <phoneExtension>" + phoneExtension + "</phoneExtension>");
                content_bulder.Append("            <phoneNumber>" + phoneNumber + "</phoneNumber>");
                content_bulder.Append("            <postalCode>" + postalCode + "</postalCode>");
                content_bulder.Append("            <state>" + state + "</state>");
                content_bulder.Append("            <status>" + status + "</status>");
                content_bulder.Append("            <taxId>" + taxId + "</taxId>");
                content_bulder.Append("            <type>" + type + "</type>");
                content_bulder.Append("            <url>" + url + "</url>");
                content_bulder.Append("         </company>");
                content_bulder.Append("      </com:create>");
                content_bulder.Append("   </soapenv:Body>");
                content_bulder.Append("</soapenv:Envelope>");

                //Console.WriteLine(content_bulder.ToString());

                List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                        new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                        new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                    };
                reqresult = Utils.postRequest(apiURL, content_bulder.ToString(), headers);
            }
            catch (WebException err)
            {
                using (var streamReader = new StreamReader(err.Response.GetResponseStream()))
                {
                    String result_text = streamReader.ReadToEnd();
                    //Console.WriteLine("Error Creating Company : " + result_text);
                    //Console.WriteLine("Message: " + err.Message);
                    reqresult = result_text;
                }
                
            }
            return reqresult;
        }
        public String updateCompany(String sessionid, String companyId, String address1, String address2, String category, String city, String country, String dunsNumber,
                                 String faxAreaCode, String faxCountryCode, String faxExtension, String faxNumber, String group, String name, String number,
                                 String phoneAreaCode, String phoneCountryCode, String phoneExtension, String phoneNumber, String postalCode, String state,
                                 String status, String taxId, String type, String url, List<Dictionary<String, String>> dynamics)
        {
            String reqresult = "";
            try
            {
                String apiURL = this.endpoint + "Company";
                StringBuilder content_bulder = new StringBuilder();
                content_bulder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:com=\"http://novatuscontracts.com/api/v1/company\">");
                content_bulder.Append("   <soapenv:Header/>");
                content_bulder.Append("   <soapenv:Body>");
                content_bulder.Append("      <com:create>");
                content_bulder.Append("         <company>");
                content_bulder.Append("            <id>"+ companyId + "</id>");
                content_bulder.Append("            <address1>" + address1 + "</address1>");
                content_bulder.Append("            <address2>" + address2 + "</address2>");
                content_bulder.Append("            <category>" + category + "</category>");
                content_bulder.Append("            <city>" + city + "</city>");
                content_bulder.Append("            <country>" + country + "</country>");
                content_bulder.Append("            <dunsNumber>" + dunsNumber + "</dunsNumber>");
                if (dynamics.Count() > 0)
                {
                    content_bulder.Append("            <dynamics>");
                    foreach (Dictionary<String, String> entry in dynamics)
                    {
                        content_bulder.Append("               <entry>");
                        content_bulder.Append("                  <key>" + entry["key"] + "</key>");
                        content_bulder.Append("                  <value>" + entry["value"] + " </value>");
                        content_bulder.Append("               </entry>");
                    }
                    content_bulder.Append("            </dynamics>");
                }
                content_bulder.Append("            <faxAreaCode>" + faxAreaCode + "</faxAreaCode>");
                content_bulder.Append("            <faxCountryCode>" + faxCountryCode + "</faxCountryCode>");
                content_bulder.Append("            <faxExtension>" + faxExtension + "</faxExtension>");
                content_bulder.Append("            <faxNumber>" + faxNumber + "</faxNumber>");
                content_bulder.Append("            <group>" + group + "</group>");            
                content_bulder.Append("            <name>" + name + "</name>");
                content_bulder.Append("            <number>" + number + "</number>");
                content_bulder.Append("            <phoneAreaCode>" + phoneAreaCode + "</phoneAreaCode>");
                content_bulder.Append("            <phoneCountryCode>" + phoneCountryCode + "</phoneCountryCode>");
                content_bulder.Append("            <phoneExtension>" + phoneExtension + "</phoneExtension>");
                content_bulder.Append("            <phoneNumber>" + phoneNumber + "</phoneNumber>");
                content_bulder.Append("            <postalCode>" + postalCode + "</postalCode>");
                content_bulder.Append("            <state>" + state + "</state>");
                content_bulder.Append("            <status>" + status + "</status>");
                content_bulder.Append("            <taxId>" + taxId + "</taxId>");
                content_bulder.Append("            <type>" + type + "</type>");
                content_bulder.Append("            <url>" + url + "</url>");
                content_bulder.Append("         </company>");
                content_bulder.Append("      </com:create>");
                content_bulder.Append("   </soapenv:Body>");
                content_bulder.Append("</soapenv:Envelope>");

                //Console.WriteLine(content_bulder.ToString());

                List<KeyValuePair<String, String>> headers = new List<KeyValuePair<String, String>>() {
                        new KeyValuePair<String, String>("Cookie", "NOVATUSSID=" + sessionid),
                        new KeyValuePair<String, String>("NOVATUSSID", sessionid)
                    };
                reqresult = Utils.postRequest(apiURL, content_bulder.ToString(), headers);
            }
            catch (WebException err)
            {
                using (var streamReader = new StreamReader(err.Response.GetResponseStream()))
                {
                    String result_text = streamReader.ReadToEnd();
                    //Console.WriteLine("Error Creating Company : " + result_text);
                    //Console.WriteLine("Message: " + err.Message);
                    reqresult = result_text;
                }
                
            }
            return reqresult;
        }
    }

    class UserFields
    {
        public String id { get; set; }
        public String addressLine1 { get; set; }
        public String addressLine2 { get; set; }
        public String city { get; set; }
        public String disabled { get; set; }
        public String emailAddress { get; set; }
        public String firstName { get; set; }
        public String forcePasswordChange { get; set; }
        public String functions { get; set; }
        public String lastName { get; set; }
        public String loginName { get; set; }
        public String mailStop { get; set; }
        public String middleName { get; set; }
        public String notes { get; set; }
        public String organization { get; set; }
        public String personId { get; set; }
        public String postalCode { get; set; }
        public String prefix { get; set; }
        public String roles { get; set; }
        public String title { get; set; }
    }

}


