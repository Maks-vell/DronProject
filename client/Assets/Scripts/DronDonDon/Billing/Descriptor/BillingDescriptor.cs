using System.Xml.Serialization;
using AgkCommons.Configurations;

namespace DronDonDon.Billing.Descriptor
{
    public class BillingDescriptor
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        
        [XmlAttribute("credits")]
        public int Credits { get; set; } 
        
        [XmlAttribute("price")]
        public double Price { get; set; }
        
        [XmlAttribute("icon")]
        public string Icon { get; set; }
        
        public void Configure(Configuration configItem)
        {
            Id = configItem.GetString("id");
            Credits = configItem.GetInt("credits");
            Price = configItem.GetFloat("price");
            Icon = configItem.GetString("icon");
        }
    }
}