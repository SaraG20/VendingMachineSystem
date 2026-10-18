using System;

namespace VendingMachineSystem.Domain.Exceptions
{
    // Shelf-related exceptions
    public class InvalidProductTypeException : DomainException
    {
        public InvalidProductTypeException(string expected, string actual)
            : base($"Product type mismatch. Expected product type '{expected}', but got '{actual}'.") { }
    }

    public class ShelfCapacityExceededException : DomainException
    {
        public ShelfCapacityExceededException(int maxCapacity, int currentQuantity, int requestedQuantity)
            : base($"Cannot load {requestedQuantity} items. Current quantity is {currentQuantity}, exceeds shelf maximum capacity of {maxCapacity}.") { }
    }

    public class ShelfOutOfStockException : DomainException
    {
        public ShelfOutOfStockException(Guid shelfId)
            : base($"Shelf with ID {shelfId} is out of stock.") { }
    }
}
