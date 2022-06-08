// Decompiled with JetBrains decompiler
// Type: Roku_SSharp.Roku
// Assembly: Roku SSharp, Version=1.0.0.34586, Culture=neutral, PublicKeyToken=null
// MVID: E4B5925D-8C34-4CFE-B7EA-59A65434D500
// Assembly location: C:\Users\Nick\Downloads\Roku 3 Series Release v1.1 (1)\Roku 3 Series Release\Roku SSHarp\Roku SSharp.dll

using System;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.CrestronXml;
using Crestron.SimplSharp.Net.Http;
using System.Collections.Generic;


namespace Roku_SSharp
{
    public class Roku
    {

        public Roku(string ipAddress)  //ND Overload lets add IP 
        { 
            this.ipAddress = ipAddress;
        }

        public Roku()
        {
        }

        public string[] appID;
        public SimplSharpString appIcon = new SimplSharpString();
        public SimplSharpString appName = new SimplSharpString();
        public List<App> appList = new List<App>();
        public static bool isWindows;  //ND
        public static bool isCrestron; //ND
        public string ipAddress;

        public Roku.CallbackHandler CallbackEvent { set; get; }

        public void GetAppsReturnToSIMPLPluse(string ipAddress)
        {
            StreamReader streamReader = new StreamReader(new HttpClient().Dispatch(new HttpClientRequest()
            {
                Url = new UrlParser("http://" + ipAddress + ":8060/query/apps"),
                RequestType = RequestType.Get
            }).ContentStream);
            string end = streamReader.ReadToEnd();
            streamReader.Close();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(end);
            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("app");
            this.appList.Clear();
            foreach (XmlNode xmlNode in elementsByTagName)
            {
                if (!(xmlNode.Attributes["type"].Value == "menu"))
                {
                    App app = new App()
                    {
                        Name = xmlNode.InnerXml,
                        ID = xmlNode.Attributes["id"].InnerText
                    };
                    app.Icon = "http://" + ipAddress + ":8060/query/icon/" + app.ID;
                    this.appList.Add(app);
                }
            }
            ushort count = (ushort)this.appList.Count;
            for (ushort index = 0; (int)index < (int)count; ++index)
            {
                this.CallbackEvent((SimplSharpString)this.appList[(int)index].Icon, (SimplSharpString)this.appList[(int)index].Name, index, count);
            }
        }


        public void SetApp(string ipAddress, ushort appValue) => new HttpClient().Dispatch(new HttpClientRequest()
        {
            Url = new UrlParser("http://" + ipAddress + ":8060/launch/" + this.appList[(int)appValue - 1].ID),
            RequestType = RequestType.Post
        });

        public void Key(string ipAddress, string keyValue) => new HttpClient().Dispatch(new HttpClientRequest()
        {
            Url = new UrlParser("http://" + ipAddress + ":8060/" + keyValue),
            RequestType = RequestType.Post
        });


        //#####################//// NICK Changes for S#

        public void GetOS()
        {
            OperatingSystem operatingSystem = Environment.OSVersion;
            if (operatingSystem.Platform.ToString() == "Win32NT")
            {
                isWindows = true;
                Console.WriteLine("{0}", "Platform is Windows");
            }
            else
            {
                isCrestron = true;
                CrestronConsole.PrintLine("{0}", "Platform is Crestron"); ;
            }
        }

        public void GetApps()
        {
            StreamReader streamReader = new StreamReader(new HttpClient().Dispatch(new HttpClientRequest()
            {
                Url = new UrlParser("http://" + ipAddress + ":8060/query/apps"),
                RequestType = RequestType.Get
            }).ContentStream);
            string end = streamReader.ReadToEnd();
            streamReader.Close();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(end);
            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("app");
            this.appList.Clear();
            foreach (XmlNode xmlNode in elementsByTagName)
            {
                if (!(xmlNode.Attributes["type"].Value == "menu"))
                {
                    App app = new App()
                    {
                        Name = xmlNode.InnerXml,
                        ID = xmlNode.Attributes["id"].InnerText
                    };
                    app.Icon = "http://" + ipAddress + ":8060/query/icon/" + app.ID;
                    this.appList.Add(app);
                }
            }
            ushort count = (ushort)this.appList.Count;
            for (ushort index = 0; (int)index < (int)count; ++index)
            {
                //   this.CallbackEvent((SimplSharpString)this.appList[(int)index].Icon, (SimplSharpString)this.appList[(int)index].Name, index, count);
                OperatingSystem operatingSystem = Environment.OSVersion;
                if (isWindows)
                {
                    System.Console.WriteLine(this.appList[(int)index].Name);
                }
                else
                {
                    CrestronConsole.PrintLine(this.appList[(int)index].Name);
                }
            }
        }
        
        
        public void SetApp(string ID) => new HttpClient().Dispatch(new HttpClientRequest()   //SET App Directly through ID instead of having to look it up, REACT has the ID
        {
            Url = new UrlParser("http://" + ipAddress + ":8060/launch/" + ID),
            RequestType = RequestType.Post
        });


        public delegate void CallbackHandler(
          SimplSharpString appIcon,
          SimplSharpString appName,
          ushort index,
          ushort numItems);
    }
}
