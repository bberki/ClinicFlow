using ClinicFlow.Application.Common;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Infrastructure.Data;

namespace ClinicFlow.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicFlowDbContext _context;

    public AppointmentRepository(ClinicFlowDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment.Id;
    }
}
