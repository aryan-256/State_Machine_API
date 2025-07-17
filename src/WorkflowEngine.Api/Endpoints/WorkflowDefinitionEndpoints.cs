using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowEngine.Api.Models;
using WorkflowEngine.Core.Exceptions;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Core.Services;
using Action = WorkflowEngine.Core.Models.Action;

namespace WorkflowEngine.Api.Endpoints
{
    public static class WorkflowDefinitionEndpoints
    {
        public static void MapWorkflowDefinitionEndpoints(this WebApplication app)
        {
            app.MapGet("/api/workflow-definitions", GetAllDefinitions);
            app.MapGet("/api/workflow-definitions/{id}", GetDefinitionById);
            app.MapPost("/api/workflow-definitions", CreateDefinition);
        }

        private static async Task<IResult> GetAllDefinitions(IWorkflowDefinitionService service)
        {
            var definitions = await service.GetAllDefinitionsAsync();
            var dtos = definitions.Select(MapToDto).ToList();
            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetDefinitionById(Guid id, IWorkflowDefinitionService service)
        {
            try
            {
                var definition = await service.GetDefinitionAsync(id);
                var dto = MapToDto(definition);
                return Results.Ok(dto);
            }
            catch (WorkflowDefinitionNotFoundException)
            {
                return Results.NotFound();
            }
        }

        private static async Task<IResult> CreateDefinition(
            [FromBody] CreateWorkflowDefinitionDto createDto,
            IWorkflowDefinitionService service)
        {
            try
            {
                var definition = new WorkflowDefinition
                {
                    Name = createDto.Name,
                    Description = createDto.Description,
                    States = createDto.States.Select(s => new State
                    {
                        Id = s.Id == Guid.Empty ? Guid.NewGuid() : s.Id,
                        Name = s.Name,
                        IsInitial = s.IsInitial,
                        IsFinal = s.IsFinal,
                        IsEnabled = s.IsEnabled,
                        Description = s.Description
                    }).ToList(),
                    Actions = createDto.Actions.Select(a => new Action
                    {
                        Id = a.Id == Guid.Empty ? Guid.NewGuid() : a.Id,
                        Name = a.Name,
                        IsEnabled = a.IsEnabled,
                        FromStateIds = a.FromStateIds,
                        ToStateId = a.ToStateId,
                        Description = a.Description
                    }).ToList()
                };

                var createdDefinition = await service.CreateDefinitionAsync(definition);
                var dto = MapToDto(createdDefinition);
                
                return Results.Created($"/api/workflow-definitions/{dto.Id}", dto);
            }
            catch (InvalidWorkflowDefinitionException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static WorkflowDefinitionDto MapToDto(WorkflowDefinition definition)
        {
            return new WorkflowDefinitionDto
            {
                Id = definition.Id,
                Name = definition.Name,
                Description = definition.Description,
                CreatedAt = definition.CreatedAt,
                States = definition.States.Select(s => new StateDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsInitial = s.IsInitial,
                    IsFinal = s.IsFinal,
                    IsEnabled = s.IsEnabled,
                    Description = s.Description
                }).ToList(),
                Actions = definition.Actions.Select(a => new ActionDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsEnabled = a.IsEnabled,
                    FromStateIds = a.FromStateIds,
                    ToStateId = a.ToStateId,
                    Description = a.Description
                }).ToList()
            };
        }
    }
}
