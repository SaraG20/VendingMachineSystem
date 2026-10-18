using System;
using VendingMachineSystem.Domain.Enums;
using VendingMachineSystem.Domain.Exceptions;

namespace VendingMachineSystem.Domain.Entities
{
    public class Shelf
    {
        public Guid Id { get; private set; }
        public ProductType ProductType { get; private set; }
        public int MaxCapacity { get; private set; }
        public int CurrentQuantity { get; private set; }

        public Shelf(ProductType productType, int maxCapacity)
        {
            ValidateProductType(productType);
            ValidateCapacity(maxCapacity);

            Id = Guid.NewGuid();
            ProductType = productType;
            MaxCapacity = maxCapacity;
            CurrentQuantity = 0;
        }

        private void ValidateProductType(ProductType productType)
        {
            if (!Enum.IsDefined(typeof(ProductType), productType))
                throw new ArgumentException("Invalid product type provided.", nameof(productType));
        }

        private void ValidateCapacity(int maxCapacity)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("Max capacity must be greater than zero.", nameof(maxCapacity));
        }

        public void LoadInventory(ProductType productType, int quantity)
        {
            if (ProductType != productType)
                throw new InvalidProductTypeException(ProductType.ToString(), productType.ToString());

            if (CurrentQuantity + quantity > MaxCapacity)
                throw new ShelfCapacityExceededException(MaxCapacity, CurrentQuantity, quantity);

            CurrentQuantity += quantity;
        }

        public void Dispense()
        {
            if (CurrentQuantity <= 0)
                throw new ShelfOutOfStockException(Id);

            CurrentQuantity--;
        }
    }
}
