using MediatR;
using ClinicFlow.Domain;

namespace ClinicFlow.Application.Patients;

public record GetPatientQuery(Guid Id) : IRequest<Patient?>;
