using System.Threading.Tasks;

namespace ClinicFlow.Domain;

public interface IPatientRepository
{
    Task AddAsync(Patient patient);
    Task<Patient?> GetAsync(Guid id);
}
