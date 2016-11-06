using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.UI.Popups;
using System.Xml.Serialization;
using System.IO;
using Windows.Storage;
using Windows.ApplicationModel.VoiceCommands;
using System.Text;

namespace HassVoiceCmd.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public static MainPageViewModel Instance { get; private set; }
        public List<VoiceCommand> _VoiceCmdList { get; private set; }
        public List<VoiceCommand> VoiceCmdList { get { return _VoiceCmdList; } private set { _VoiceCmdList = value; base.RaisePropertyChanged(); } }
        private bool _bootstrapped;
        public bool Bootstrapped { get { return _bootstrapped; } set { _bootstrapped = value; base.RaisePropertyChanged(); } }

        private string _ricardo;
        public string ricardo { get { return _ricardo; } set { _ricardo = value; base.RaisePropertyChanged(); } }

        public MainPageViewModel()
        {
            Instance = this;
            Bootstrapped = false;
        }

        public async Task BootstrapHass()
        {
            try
            {
                ricardo = "Bootstrap started...";
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey("WebAddress"))
                {
                    string uri = (string)localSettings.Values["WebAddress"];

                    Hass hass = new Hass(new Uri(uri));
                    await hass.Bootstrap();

                    string filter;
                    if (localSettings.Values.ContainsKey("EntityFilter"))
                        filter = (string)localSettings.Values["EntityFilter"];
                    else
                        filter = String.Empty;

                    VoiceCmdList = hass.GetCommandList(uri, filter);
                    if (VoiceCmdList.Count < 100)
                        InstallVoiceCommands(VoiceCmdList);
                    ricardo = String.Format("Bootstrap succeeded");
                    Bootstrapped = true;
                }
            }
            catch (Exception ex)
            {
                ricardo = "Bootstrap failed because: " + ex.Message;
            }
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        private async void InstallVoiceCommands(List<VoiceCommand> lst)
        {
            VoiceCommands vcd = new VoiceCommands();
            vcd.CommandSet = new VoiceCommandSet();
            vcd.CommandSet.lang = "en-us";
            vcd.CommandSet.Name = "HASS";

            vcd.CommandSet.Example = "Read command as shown";

            string prefix = "Please";
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("VoicePrefix"))
                prefix = (string)localSettings.Values["VoicePrefix"];


            vcd.CommandSet.CommandPrefix = prefix;
            vcd.CommandSet.Command = lst.ToArray();

            // Serialize to xml 
            XmlSerializer ser = new XmlSerializer(typeof(VoiceCommands));
            string xml;
            //StringWriter writer = new StringWriter();
            using (StringWriter writer = new Utf8StringWriter())
            {
                ser.Serialize(writer, vcd);
                xml = writer.ToString();
            }
            //xml = xml.Replace("utf-16", "utf-8");
            xml = xml.Replace("xsi:nil=\"true\"", string.Empty);
            
            // save XML in a Temp file
            StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
            StorageFile vcdFile = await tempFolder.CreateFileAsync(@"VoiceCommands.xml", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(vcdFile, xml, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            // Load temp file into cortana
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdFile);
        }

        public object TappedCmd
        {
            get { return null; }
            set
            {
                ricardo = "Service call started...";
                VoiceCommand vc = _VoiceCmdList[Int32.Parse(value.ToString())];
                try
                {
                    Hass.CallApiServiceAsync(vc.WebAddress, vc.Domain, vc.Service, vc.Entity);
                    /*
                    string state = async Hass.CallApiServiceAsync(vc.WebAddress, vc.Domain, vc.Service, vc.Entity);
                    if (state != null)
                    {
                        var userMessage = new VoiceCommandUserMessage();
                        userMessage.SpokenMessage = state;
                        VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(userMessage);

                        //AppServiceTriggerDetails triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
                        //VoiceCommandServiceConnection voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails()
                        //await voiceServiceConnection.ReportSuccessAsync(response);
                    }
                    */
                    ricardo = "Service call succeeded";
                }
                catch (Exception ex)
                {
                    ricardo = "Service call failed because: " + ex.Message;
                }
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            ricardo = "Ready";
            if (!Bootstrapped)
            {
                await BootstrapHass();
            }
            else if (parameter != null)
            {
                VoiceCommand vc = VoiceCmdList.Find(x => x.Name == parameter.ToString());
                if (vc != null)
                    try
                    {
                        Hass.CallApiServiceAsync(vc.WebAddress, vc.Domain, vc.Service, vc.Entity);
                        /*
                        string state = async Hass.CallApiServiceAsync(vc.WebAddress, vc.Domain, vc.Service, vc.Entity);
                        if (state != null)
                        {
                            var userMessage = new VoiceCommandUserMessage();
                            userMessage.SpokenMessage = state;
                            VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(userMessage);

                            //AppServiceTriggerDetails triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
                            //VoiceCommandServiceConnection voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails()
                            //await voiceServiceConnection.ReportSuccessAsync(response);
                        }
                        */
                        ricardo = "Service call succeeded";
                    }
                    catch (Exception ex)
                    {
                        ricardo = "Service call failed because: " + ex.Message;
                    }
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}

