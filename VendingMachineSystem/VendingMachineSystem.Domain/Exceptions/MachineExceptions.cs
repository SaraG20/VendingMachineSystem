using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineSystem.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }

    public class MaxShelvesExceededException : DomainException
    {
        public MaxShelvesExceededException(int maxShelves)
            : base($"Cannot add more shelves. Maximum allowed shelves is {maxShelves}.") { }
    }

    public class ShelfNotFoundException : DomainException
    {
        public ShelfNotFoundException(Guid shelfId)
            : base($"Shelf with ID {shelfId} was not found.") { }
    }
}
