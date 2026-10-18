using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachineSystem.Domain.Enums;
using VendingMachineSystem.Domain.Exceptions;

namespace VendingMachineSystem.Domain.Entities
{
    public class VendingMachine
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Location { get; private set; }
        public int MaxShelves { get; private set; }

        private readonly List<Shelf> _shelves = new();
        public IReadOnlyCollection<Shelf> Shelves => _shelves.AsReadOnly();

        public VendingMachine(string name, string? location, int maxShelves)
        {
            ValidateName(name);
            ValidateMaxShelves(maxShelves);

            Id = Guid.NewGuid();
            Name = name;
            Location = location;
            MaxShelves = maxShelves;
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Vending machine name cannot be empty.", nameof(name));
        }

        private void ValidateMaxShelves(int maxShelves)
        {
            if (maxShelves <= 0)
                throw new ArgumentException("Max shelves must be greater than zero.", nameof(maxShelves));
        }

        public Shelf AddShelf(ProductType productType, int maxCapacity)
        {
            if (_shelves.Count >= MaxShelves)
                throw new MaxShelvesExceededException(MaxShelves);

            var shelf = new Shelf(productType, maxCapacity);
            _shelves.Add(shelf);
            return shelf;
        }

        public Shelf GetShelf(Guid shelfId)
        {
            var shelf = _shelves.FirstOrDefault(s => s.Id == shelfId);

            if (shelf == null)
                throw new ShelfNotFoundException(shelfId);

            return shelf;
        }
    }
}
