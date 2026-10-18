using Microsoft.AspNetCore.Mvc;
using VendingMachineSystem.Api.DTOs;
using VendingMachineSystem.Api.Repositories;
using VendingMachineSystem.Domain.Entities;

namespace VendingMachineSystem.Api.Controllers;

[ApiController]
[Route("api/vending-machines")]
public class VendingMachinesController : ControllerBase
{
    private readonly IVendingMachineRepository _repository;

    public VendingMachinesController(IVendingMachineRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Create new vending machine
    /// </summary>
    [HttpPost]
    public IActionResult CreateMachine([FromBody] CreateVendingMachineRequest request)
    {
        var machine = new VendingMachine(request.Name, request.Location, request.MaxShelves);
        _repository.Add(machine);

        var response = MapToResponse(machine);
        return CreatedAtAction(nameof(GetMachineById), new { id = machine.Id }, response);
    }

    /// <summary>
    /// Get vending machine by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public IActionResult GetMachineById(Guid id)
    {
        var machine = _repository.GetById(id); // אם לא קיים — ייזרק Exception
        return Ok(MapToResponse(machine));
    }

    /// <summary>
    /// Add shelf to vending machine
    /// </summary>
    [HttpPost("{id:guid}/shelves")]
    public IActionResult AddShelf(Guid id, [FromBody] AddShelfRequest request)
    {
        var machine = _repository.GetById(id);
        var shelf = machine.AddShelf(request.ProductType, request.MaxCapacity);

        return Ok(MapToResponse(machine));
    }

    /// <summary>
    /// Load inventory to shelf
    /// </summary>
    [HttpPost("{id:guid}/shelves/{shelfId:guid}/inventory")]
    public IActionResult LoadInventory(Guid id, Guid shelfId, [FromBody] LoadInventoryRequest request)
    {
        var machine = _repository.GetById(id);
        var shelf = machine.GetShelf(shelfId);

        shelf.LoadInventory(request.ProductType, request.Quantity);

        return Ok(MapToResponse(machine));
    }

    /// <summary>
    /// Dispense product from shelf
    /// </summary>
    [HttpPost("{id:guid}/shelves/{shelfId:guid}/dispense")]
    public IActionResult Dispense(Guid id, Guid shelfId)
    {
        var machine = _repository.GetById(id);
        var shelf = machine.GetShelf(shelfId);

        shelf.Dispense();

        return Ok(MapToResponse(machine));
    }

    /// <summary>
    /// Get all vending machines
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<VendingMachineResponse>> GetAllMachines()
    {
        var machines = _repository.GetAll();

        var response = machines.Select(m => new VendingMachineResponse
        {
            Id = m.Id,
            Name = m.Name,
            Location = m.Location,
            MaxShelves = m.MaxShelves,
            Shelves = m.Shelves.Select(s => new ShelfResponse
            {
                Id = s.Id,
                ProductType = s.ProductType,
                MaxCapacity = s.MaxCapacity,
                CurrentQuantity = s.CurrentQuantity
            }).ToList()
        });

        return Ok(response);
    }

    private static VendingMachineResponse MapToResponse(VendingMachine machine)
    {
        return new VendingMachineResponse
        {
            Id = machine.Id,
            Name = machine.Name,
            Location = machine.Location,
            MaxShelves = machine.MaxShelves,
            Shelves = machine.Shelves.Select(s => new ShelfResponse
            {
                Id = s.Id,
                ProductType = s.ProductType,
                MaxCapacity = s.MaxCapacity,
                CurrentQuantity = s.CurrentQuantity
            }).ToList()
        };
    }
}
