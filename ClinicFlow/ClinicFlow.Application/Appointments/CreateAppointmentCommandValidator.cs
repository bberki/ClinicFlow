using FluentValidation;

namespace ClinicFlow.Application.Appointments;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        // İş Kuralı 1: Hasta ID kontrolü
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage("Hasta ID'si sıfırdan büyük olmalıdır");

        // İş Kuralı 2: Doktor ID kontrolü
        RuleFor(x => x.DoctorId)
            .GreaterThan(0)
            .WithMessage("Doktor ID'si sıfırdan büyük olmalıdır");

        // İş Kuralı 3: Geçmiş tarih kontrolü (Önemli İş Kuralı!)
        RuleFor(x => x.ScheduledAt)
            .Must(d => d > DateTime.Now)
            .WithMessage("Randevu tarihi gelecekte olmalıdır. Geçmiş tarihe randevu oluşturulamaz.");

        // İş Kuralı 4: Çalışma saatleri kontrolü (Yeni!)
        RuleFor(x => x.ScheduledAt)
            .Must(BeWithinWorkingHours)
            .WithMessage("Randevu sadece 08:00 - 18:00 saatleri arasında oluşturulabilir");

        // İş Kuralı 5: Hafta sonu kontrolü (Yeni!)
        RuleFor(x => x.ScheduledAt)
            .Must(NotBeWeekend)
            .WithMessage("Randevu hafta sonuna oluşturulamaz");

        // İş Kuralı 6: Kullanıcı ID kontrolü
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("Kullanıcı ID'si sıfırdan büyük olmalıdır");

        // İş Kuralı 7: Maksimum ileri tarih kontrolü (Yeni!)
        RuleFor(x => x.ScheduledAt)
            .Must(d => d <= DateTime.Now.AddMonths(3))
            .WithMessage("Randevu en fazla 3 ay sonrasına kadar oluşturulabilir");
    }

    // Yardımcı metod: Çalışma saatleri kontrolü
    private bool BeWithinWorkingHours(DateTime scheduledAt)
    {
        var hour = scheduledAt.Hour;
        return hour >= 8 && hour < 18;
    }

    // Yardımcı metod: Hafta sonu kontrolü
    private bool NotBeWeekend(DateTime scheduledAt)
    {
        var dayOfWeek = scheduledAt.DayOfWeek;
        return dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday;
    }
}
