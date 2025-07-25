namespace ClinicFlow.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateTime ScheduledAt { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
}
