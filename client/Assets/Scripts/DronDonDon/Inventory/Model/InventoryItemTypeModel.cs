using System.Xml.Serialization;

namespace DronDonDon.Inventory.Model
{
    public enum InventoryItemTypeModel
    {
        [XmlEnum("dron")]
        DRON,
    }
}