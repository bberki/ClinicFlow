using MediatR;
using ClinicFlow.Domain;

namespace ClinicFlow.Application.Patients;

public class AddPatientCommandHandler : IRequestHandler<AddPatientCommand>
{
    private readonly IPatientRepository _repository;

    public AddPatientCommandHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(AddPatientCommand request, CancellationToken cancellationToken)
    {
        var patient = new Patient(request.Id, request.Name);
        await _repository.AddAsync(patient);
        return Unit.Value;
    }
}
