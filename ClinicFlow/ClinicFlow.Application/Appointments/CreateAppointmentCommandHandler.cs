using MediatR;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Application.Appointments;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Appointment>
{
    public Task<Appointment> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ScheduledAt = request.ScheduledAt
        };

        return Task.FromResult(appointment);
    }
}
