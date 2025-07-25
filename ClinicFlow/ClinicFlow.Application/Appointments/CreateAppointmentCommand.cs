using MediatR;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Application.Appointments;

public record CreateAppointmentCommand(int PatientId, int DoctorId, DateTime ScheduledAt) : IRequest<Appointment>;
