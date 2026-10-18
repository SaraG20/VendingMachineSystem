using System.ComponentModel.DataAnnotations;

public class CreateVendingMachineRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }

    [Range(1, int.MaxValue)]
    public int MaxShelves { get; set; }
}
