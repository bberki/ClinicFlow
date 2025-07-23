using MediatR;

namespace ClinicFlow.Application.Patients;

public record AddPatientCommand(Guid Id, string Name) : IRequest;
