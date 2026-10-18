using VendingMachineSystem.Domain.Enums;

namespace VendingMachineSystem.Api.DTOs
{
    public class AddShelfRequest
    {
        public ProductType ProductType { get; set; }
        public int MaxCapacity { get; set; }
    }
}
