using ClinicFlow.Domain;
using System.Collections.Concurrent;

namespace ClinicFlow.Infrastructure.Repositories;

public class InMemoryPatientRepository : IPatientRepository
{
    private readonly ConcurrentDictionary<Guid, Patient> _patients = new();

    public Task AddAsync(Patient patient)
    {
        _patients[patient.Id] = patient;
        return Task.CompletedTask;
    }

    public Task<Patient?> GetAsync(Guid id)
    {
        _patients.TryGetValue(id, out var patient);
        return Task.FromResult(patient);
    }
}
