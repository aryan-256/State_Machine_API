using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowEngine.Api.Models;
using WorkflowEngine.Core.Exceptions;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Core.Services;

namespace WorkflowEngine.Api.Endpoints
{
    public static class WorkflowInstanceEndpoints
    {
        public static void MapWorkflowInstanceEndpoints(this WebApplication app)
        {
            app.MapGet("/api/workflow-instances", GetAllInstances);
            app.MapGet("/api/workflow-instances/{id}", GetInstanceById);
            app.MapPost("/api/workflow-instances", CreateInstance);
            app.MapPost("/api/workflow-instances/{instanceId}/actions/{actionId}", ExecuteAction);
        }

        private static async Task<IResult> GetAllInstances(IWorkflowInstanceService service)
        {
            var instances = await service.GetAllInstancesAsync();
            var dtos = await Task.WhenAll(instances.Select(async i => await MapToDtoAsync(i, service)));
            return Results.Ok(dtos);
        }

        private static async Task<IResult> GetInstanceById(Guid id, IWorkflowInstanceService service)
        {
            try
            {
                var instance = await service.GetInstanceAsync(id);
                var dto = await MapToDtoAsync(instance, service);
                return Results.Ok(dto);
            }
            catch (WorkflowInstanceNotFoundException)
            {
                return Results.NotFound();
            }
        }

        private static async Task<IResult> CreateInstance(
            [FromBody] CreateWorkflowInstanceDto createDto,
            IWorkflowInstanceService service)
        {
            try
            {
                var instance = await service.CreateInstanceAsync(createDto.DefinitionId);
                var dto = await MapToDtoAsync(instance, service);
                
                return Results.Created($"/api/workflow-instances/{dto.Id}", dto);
            }
            catch (WorkflowDefinitionNotFoundException)
            {
                return Results.BadRequest(new { message = $"Workflow definition with ID '{createDto.DefinitionId}' was not found" });
            }
            catch (InvalidWorkflowDefinitionException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<IResult> ExecuteAction(
            Guid instanceId,
            Guid actionId,
            IWorkflowInstanceService service)
        {
            try
            {
                var instance = await service.ExecuteActionAsync(instanceId, actionId);
                var dto = await MapToDtoAsync(instance, service);
                
                return Results.Ok(dto);
            }
            catch (WorkflowInstanceNotFoundException)
            {
                return Results.NotFound();
            }
            catch (ActionNotFoundException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (InvalidActionExecutionException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        }

        private static async Task<WorkflowInstanceDto> MapToDtoAsync(WorkflowInstance instance, IWorkflowInstanceService service)
        {
            // Get the complete instance with definition
            if (instance.Definition == null)
            {
                instance = await service.GetInstanceAsync(instance.Id);
            }

            var currentState = instance.Definition?.States.FirstOrDefault(s => s.Id == instance.CurrentStateId);
            
            var dto = new WorkflowInstanceDto
            {
                Id = instance.Id,
                DefinitionId = instance.DefinitionId,
                DefinitionName = instance.Definition?.Name ?? "",
                CurrentStateId = instance.CurrentStateId,
                CurrentStateName = currentState?.Name ?? "",
                IsInFinalState = currentState?.IsFinal ?? false,
                CreatedAt = instance.CreatedAt,
                UpdatedAt = instance.UpdatedAt
            };

            // Map history
            if (instance.History != null && instance.Definition != null)
            {
                foreach (var transition in instance.History)
                {
                    var fromState = instance.Definition.States.FirstOrDefault(s => s.Id == transition.FromStateId);
                    var toState = instance.Definition.States.FirstOrDefault(s => s.Id == transition.ToStateId);
                    var action = instance.Definition.Actions.FirstOrDefault(a => a.Id == transition.ActionId);

                    dto.History.Add(new StateTransitionDto
                    {
                        FromStateId = transition.FromStateId,
                        FromStateName = fromState?.Name ?? "",
                        ToStateId = transition.ToStateId,
                        ToStateName = toState?.Name ?? "",
                        ActionId = transition.ActionId,
                        ActionName = action?.Name ?? "",
                        Timestamp = transition.Timestamp
                    });
                }
            }

            return dto;
        }
    }
}
