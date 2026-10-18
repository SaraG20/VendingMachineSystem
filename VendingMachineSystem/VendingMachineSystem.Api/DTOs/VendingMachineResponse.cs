namespace VendingMachineSystem.Api.DTOs
{
    public class VendingMachineResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public int MaxShelves { get; set; }
        public List<ShelfResponse> Shelves { get; set; } = new();
    }
}
