using System.Collections.Concurrent;
using VendingMachineSystem.Domain.Entities;
using VendingMachineSystem.Domain.Exceptions;

namespace VendingMachineSystem.Api.Repositories;

public class InMemoryVendingMachineRepository : IVendingMachineRepository
{
    private readonly ConcurrentDictionary<Guid, VendingMachine> _machines = new();

    public void Add(VendingMachine machine)
    {
        _machines[machine.Id] = machine;
    }

    public VendingMachine GetById(Guid id)
    {
        if (_machines.TryGetValue(id, out var machine))
            return machine;

        throw new ShelfNotFoundException(id);
    }

    public IEnumerable<VendingMachine> GetAll()
    {
        return _machines.Values;
    }
}
