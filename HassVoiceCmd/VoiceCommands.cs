
using System.Xml.Serialization;

namespace HassVoiceCmd
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/voicecommands/1.2")]
    [XmlRootAttribute(Namespace = "http://schemas.microsoft.com/voicecommands/1.2", IsNullable = false)]
    public partial class VoiceCommands
    {

        private VoiceCommandSet commandSetField;

        /// <remarks/>
        public VoiceCommandSet CommandSet
        {
            get
            {
                return this.commandSetField;
            }
            set
            {
                this.commandSetField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/voicecommands/1.2")]
    public partial class VoiceCommandSet
    {

        private string prefixField;
        private string exampleField;
        private VoiceCommand[] commandField;
        private string langField;
        private string nameField;

        /// <remarks/>
        public string CommandPrefix
        {
            get
            {
                return this.prefixField;
            }
            set
            {
                this.prefixField = value;
            }
        }

        /// <remarks/>
        public string Example
        {
            get
            {
                return this.exampleField;
            }
            set
            {
                this.exampleField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("Command")]
        public VoiceCommand[] Command
        {
            get
            {
                return this.commandField;
            }
            set
            {
                this.commandField = value;
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(IsNullable = false)]
    public class VoiceCommand
    {
        private string exampleField;
        private string[] listenForField;
        private string feedbackField;
        private object navigateField;
        private string nameField;
        private string domainField;
        private string entityField;
        private string serviceField;
        private string webAddressField;
        private string friendlyNameField;

        /// <remarks/>
        public string Example
        {
            get
            {
                return this.exampleField;
            }
            set
            {
                this.exampleField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("ListenFor")]
        public string[] ListenFor
        {
            get
            {
                return this.listenForField;
            }
            set
            {
                this.listenForField = value;
            }
        }

        /// <remarks/>
        public string Feedback
        {
            get
            {
                return this.feedbackField;
            }
            set
            {
                this.feedbackField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute(IsNullable = true)]
        public object Navigate
        {
            get
            {
                return this.navigateField;
            }
            set
            {
                this.navigateField = value;
            }
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string Domain
        {
            get
            {
                return this.domainField;
            }
            set
            {
                this.domainField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string Entity
        {
            get
            {
                return this.entityField;
            }
            set
            {
                this.entityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string Service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string WebAddress
        {
            get
            {
                return this.webAddressField;
            }
            set
            {
                this.webAddressField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore]
        public string FriendlyName
        {
            get
            {
                return this.friendlyNameField;
            }
            set
            {
                this.friendlyNameField = value;
            }
        }
    }
}
