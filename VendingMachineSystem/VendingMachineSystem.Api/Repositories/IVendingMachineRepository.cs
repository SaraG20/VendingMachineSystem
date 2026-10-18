using VendingMachineSystem.Domain.Entities;

namespace VendingMachineSystem.Api.Repositories;
public interface IVendingMachineRepository
{
    void Add(VendingMachine machine);
    VendingMachine? GetById(Guid id);
    IEnumerable<VendingMachine> GetAll();

}