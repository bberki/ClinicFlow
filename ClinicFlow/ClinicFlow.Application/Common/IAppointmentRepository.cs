namespace ClinicFlow.Application.Common;

using ClinicFlow.Domain.Entities;

public interface IAppointmentRepository
{
    Task<int> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
}
