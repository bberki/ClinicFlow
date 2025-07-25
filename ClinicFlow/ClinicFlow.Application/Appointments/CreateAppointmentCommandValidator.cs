using FluentValidation;

namespace ClinicFlow.Application.Appointments;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0);

        RuleFor(x => x.DoctorId)
            .GreaterThan(0);

        RuleFor(x => x.ScheduledAt)
            .Must(d => d > DateTime.Now)
            .WithMessage("Scheduled time must be in the future");

        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}
