using VendingMachineSystem.Domain.Enums;

namespace VendingMachineSystem.Api.DTOs
{
    public class ShelfResponse
    {
        public Guid Id { get; set; }
        public ProductType ProductType { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentQuantity { get; set; }
    }
}
