using VendingMachineSystem.Domain.Enums;

namespace VendingMachineSystem.Api.DTOs
{
    public class LoadInventoryRequest
    {
        public ProductType ProductType { get; set; }
        public int Quantity { get; set; }
    }
}
