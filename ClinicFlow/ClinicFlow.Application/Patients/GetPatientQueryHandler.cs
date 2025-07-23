using MediatR;
using ClinicFlow.Domain;

namespace ClinicFlow.Application.Patients;

public class GetPatientQueryHandler : IRequestHandler<GetPatientQuery, Patient?>
{
    private readonly IPatientRepository _repository;

    public GetPatientQueryHandler(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<Patient?> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAsync(request.Id);
    }
}
