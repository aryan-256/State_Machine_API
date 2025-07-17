using WorkflowEngine.Api.Endpoints;
using WorkflowEngine.Api.Infrastructure.Repositories;
using WorkflowEngine.Api.Services;
using WorkflowEngine.Core.Repositories;
using WorkflowEngine.Core.Services;

// Set up the web application builder and register services
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Workflow Engine API",
        Version = "v1",
        Description = "A configurable state machine workflow engine API"
    });
});

// Add repositories as singletons for in-memory persistence (no external DB used)
builder.Services.AddSingleton<IWorkflowDefinitionRepository, InMemoryWorkflowDefinitionRepository>();
builder.Services.AddSingleton<IWorkflowInstanceRepository, InMemoryWorkflowInstanceRepository>();

// Add services
builder.Services.AddScoped<IWorkflowDefinitionService, WorkflowDefinitionService>();
builder.Services.AddScoped<IWorkflowInstanceService, WorkflowInstanceService>();

// Build the app and configure middleware
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map workflow-related endpoints (definitions and instances)
app.MapWorkflowDefinitionEndpoints();
app.MapWorkflowInstanceEndpoints();

app.Run();
