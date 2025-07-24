using ClinicFlow.Application.Greetings;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblyContaining<GetGreetingQuery>());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/greeting", async (IMediator mediator) =>
    await mediator.Send(new GetGreetingQuery()));

app.Run();

public partial class Program { }
