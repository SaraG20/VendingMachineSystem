using Xunit;
using VendingMachineSystem.Domain.Entities;
using VendingMachineSystem.Domain.Exceptions;
using VendingMachineSystem.Domain.Enums;

namespace VendingMachineSystem.Tests;

public class VendingMachineTests
{
    // -------------------------
    // VENDING MACHINE TESTS
    // -------------------------

    [Fact]
    public void CreateMachine_WithValidData_ShouldSucceed()
    {
        var machine = new VendingMachine("Test Machine", "Location", 3);

        Assert.Equal("Test Machine", machine.Name);
        Assert.Equal("Location", machine.Location);
        Assert.Equal(3, machine.MaxShelves);
        Assert.Empty(machine.Shelves);
    }

    [Fact]
    public void CreateMachine_WithInvalidName_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new VendingMachine("", null, 3));
    }

    [Fact]
    public void CreateMachine_WithInvalidMaxShelves_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new VendingMachine("Test", null, 0));
    }

    [Fact]
    public void AddShelf_WithinLimit_ShouldAddShelf()
    {
        var machine = new VendingMachine("Test Machine", "Location", 3);

        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        Assert.Single(machine.Shelves);
        Assert.Equal(ProductType.Bottle, shelf.ProductType);
        Assert.Equal(10, shelf.MaxCapacity);
    }

    [Fact]
    public void AddShelf_ExceedingLimit_ShouldThrowMaxShelvesExceededException()
    {
        var machine = new VendingMachine("Test Machine", null, 1);
        machine.AddShelf(ProductType.Bottle, 10);

        Assert.Throws<MaxShelvesExceededException>(() => machine.AddShelf(ProductType.Snack, 5));
    }

    [Fact]
    public void GetShelf_WhenNotExists_ShouldThrowShelfNotFoundException()
    {
        var machine = new VendingMachine("Test Machine", null, 2);

        Assert.Throws<ShelfNotFoundException>(() => machine.GetShelf(Guid.NewGuid()));
    }

    // -------------------------
    // SHELF TESTS
    // -------------------------

    [Fact]
    public void LoadInventory_WithValidData_ShouldIncreaseQuantity()
    {
        var machine = new VendingMachine("Test Machine", null, 2);
        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        shelf.LoadInventory(ProductType.Bottle, 5);

        Assert.Equal(5, shelf.CurrentQuantity);
    }

    [Fact]
    public void LoadInventory_InMultipleSteps_ShouldAccumulateQuantity()
    {
        var machine = new VendingMachine("Test Machine", null, 2);
        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        shelf.LoadInventory(ProductType.Bottle, 3);
        shelf.LoadInventory(ProductType.Bottle, 2);

        Assert.Equal(5, shelf.CurrentQuantity);
    }

    [Fact]
    public void LoadInventory_WrongProductType_ShouldThrowInvalidProductTypeException()
    {
        var machine = new VendingMachine("Test Machine", null, 2);
        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        Assert.Throws<InvalidProductTypeException>(() => shelf.LoadInventory(ProductType.Snack, 5));
    }

    [Fact]
    public void LoadInventory_ExceedingCapacity_ShouldThrowShelfCapacityExceededException()
    {
        var machine = new VendingMachine("Test Machine", null, 2);
        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        Assert.Throws<ShelfCapacityExceededException>(() => shelf.LoadInventory(ProductType.Bottle, 15));
    }

    [Fact]
    public void Dispense_WhenProductAvailable_ShouldDecreaseQuantity()
    {
        var machine = new VendingMachine("Test Machine", null, 2);
        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        shelf.LoadInventory(ProductType.Bottle, 3);
        shelf.Dispense();

        Assert.Equal(2, shelf.CurrentQuantity);
    }

    [Fact]
    public void Dispense_UntilEmpty_ShouldReachZero()
    {
        var machine = new VendingMachine("Test Machine", null, 2);
        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        shelf.LoadInventory(ProductType.Bottle, 2);
        shelf.Dispense();
        shelf.Dispense();

        Assert.Equal(0, shelf.CurrentQuantity);
    }

    [Fact]
    public void Dispense_WhenOutOfStock_ShouldThrowShelfOutOfStockException()
    {
        var machine = new VendingMachine("Test Machine", null, 2);
        var shelf = machine.AddShelf(ProductType.Bottle, 10);

        Assert.Throws<ShelfOutOfStockException>(() => shelf.Dispense());
    }
}
