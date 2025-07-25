using ClinicFlow.Application.Common;
using ClinicFlow.Domain.Entities;
using MediatR;

namespace ClinicFlow.Application.Appointments;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, int>
{
    private readonly IAppointmentRepository _repository;

    public CreateAppointmentCommandHandler(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ScheduledAt = request.ScheduledAt,
            UserId = 0
        };

        return await _repository.AddAsync(appointment, cancellationToken);
    }
}
