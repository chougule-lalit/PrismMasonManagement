using PrismMasonManagement.Core.PrismMasonManagementCoreBase;

namespace PrismMasonManagement.Core.Entities
{
    public class Item : FullAuditedEntity<int>
    {
        public string Name { get; set; }
    }
}