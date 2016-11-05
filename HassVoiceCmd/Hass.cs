using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;

namespace HassVoiceCmd
{
    public class Hass
    {
        private Uri _uri;
        private Bootstrap _bootstrap;

        public List<Entity> GetEntities(string filter)
        {
            List<Entity> lst = new List<Entity>();

            foreach (State st in _bootstrap.states)
            {
                Entity ent = new Entity(st.entity_id, st.attributes.friendly_name);
                
                // Apply Filter
                if (filter.Contains(ent.domain)) continue;

                // Remove Entities without friendly name
                if (ent.name == null) continue;

                ent.services = GetServices(ent);

                lst.Add(ent);
            }


            // Remove services named as entitites
            foreach (Entity ent in lst)
                ent.services.RemoveAll(x => lst.Exists(y => y.entity == x));

            return lst;
        }

        private List<string> GetServices(Entity ent)
        {
            List<string> lst = new List<string>();

            // Add Services for the domain
            foreach (Service svc in _bootstrap.services)
                if (svc.domain == ent.domain)
                    foreach (KeyValuePair<string, ServiceDetail> kvp in svc.services)
                        if ((lst.Count == 0) || (!lst.Exists(x => x == kvp.Key)))
                            lst.Add(kvp.Key);

            lst.Sort();

            return lst;
        }

        private async Task<string> CallApiBootstrapAsync()
        {
            WebRequest request = WebRequest.Create(new Uri(_uri, "/api/bootstrap"));
            request.Method = "GET";
            request.ContentType = "application/json";
            WebResponse response = await request.GetResponseAsync();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return await reader.ReadToEndAsync();
        }

        public Hass(Uri uri)
        {
            _uri = uri;
        }

        public async Task Bootstrap()
        {
            string data = await CallApiBootstrapAsync();
            _bootstrap = (Bootstrap)JsonConvert.DeserializeObject<Bootstrap>(data);
        }

        public static async Task<string> CallApiServiceAsync(string url, string domain, string service, string entity)
        {
            string fullURL;
            string verb;

            if (service == "state")
            {
                fullURL = url + "/api/states/" + domain + "." + entity;
                verb = "GET";
            }
            else
            {
                fullURL = url + "/api/services/" + domain + "/" + service;
                verb = "POST";
            }
            WebRequest request = WebRequest.Create(new Uri(fullURL));
            request.Method = verb;
            request.ContentType = "application/json";
            if (entity != null && service != "state")
            {
                Stream stream = await request.GetRequestStreamAsync();
                string json = "{\"entity_id\":\"" + domain + "." + entity + "\"}";
                await stream.WriteAsync(System.Text.Encoding.ASCII.GetBytes(json), 0, json.Length);
            }
            WebResponse response = await request.GetResponseAsync();

            StreamReader reader = new StreamReader(response.GetResponseStream());
            string data = await reader.ReadToEndAsync();

            if (data != "[]")
            {
                State st = (State)JsonConvert.DeserializeObject<State>(data);
                return st.state;
            }
            else
                return "";
        }

        public List<VoiceCommand> GetCommandList(string webAddress, string filter)
        {
            List<VoiceCommand> lstCmd = new List<VoiceCommand>();
            VoiceCommand cmd;
            string s;

            foreach (Entity ent in GetEntities(filter))
            {
                // Add a dummy service to check state
                cmd = new VoiceCommand();
                s = "state" + " [the] " + ent.name;
                s = s.Replace("_", " ");
                s = s.Replace("-", " ");
                s = s.ToLower();
                cmd.Example = s;
                cmd.ListenFor = new string[] { s };
                cmd.Name = "state" + ent.domain + ent.entity;
                cmd.Domain = ent.domain;
                cmd.Entity = ent.entity;
                cmd.Service = "state";
                cmd.Feedback = "Home is checking " + ent.name;
                cmd.WebAddress = webAddress;
                cmd.FriendlyName = ent.name;
                //lstCmd.Add(cmd);

                foreach (string svc in ent.services)
                {
                    
                    cmd = new VoiceCommand();
                    s = svc + " [the] " + ent.name;
                    s = s.Replace("_", " ");
                    s = s.Replace("-", " ");
                    s = s.ToLower();
                    cmd.Example = s;
                    cmd.ListenFor = new string[] { s };
                    cmd.Name = svc + ent.domain + ent.entity;
                    cmd.Domain = ent.domain;
                    cmd.Entity = ent.entity;
                    cmd.Service = svc;
                    cmd.Feedback = "Home is adjusting the " + ent.name;
                    cmd.WebAddress = webAddress;
                    cmd.FriendlyName = ent.name;

                    lstCmd.Add(cmd);
                }
            }
            lstCmd.Sort((x, y) => String.Compare(x.Domain+x.Entity+x.Service, y.Domain + y.Entity + y.Service));

            return lstCmd;
        }

    }

    public class Bootstrap
    {
        public List<Service> services { get; set; }
        public List<State> states { get; set; }
    }

    public class Service
    {
        public string domain { get; set; }
        public Dictionary<string, ServiceDetail> services { get; set; }
    }

    public class ServiceDetail
    {
        public string description { get; set; }
    }

    public class State
    {
        public Attributes attributes { get; set; }
        public string entity_id { get; set; }
        public string state { get; set; }
    }

    public class Attributes
    {
        public string friendly_name { get; set; }
    }

    public class Entity
    {
        public string domain { get; set; }
        public string entity { get; set; }
        public string name { get; set; }
        public List<string> services;

        public Entity(string entity_id, string friendly_name)
        {
            string[] s = entity_id.Split('.');
            domain = s[0];
            entity = s[1];
            name = friendly_name;
        }
    }
}
