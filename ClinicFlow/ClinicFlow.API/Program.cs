using ClinicFlow.Application;
using ClinicFlow.Application.Patients;
using ClinicFlow.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapPost("/patients", async (AddPatientCommand command, IMediator mediator) =>
{
    await mediator.Send(command);
    return Results.Ok();
});

app.MapGet("/patients/{id:guid}", async (Guid id, IMediator mediator) =>
{
    var patient = await mediator.Send(new GetPatientQuery(id));
    return patient is not null ? Results.Ok(patient) : Results.NotFound();
});

app.MapGet("/", () => "Hello World!");

app.Run();
