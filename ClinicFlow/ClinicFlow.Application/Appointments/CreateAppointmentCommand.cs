using MediatR;

namespace ClinicFlow.Application.Appointments;

public record CreateAppointmentCommand(int PatientId, int DoctorId, DateTime ScheduledAt, int UserId) : IRequest<int>;
