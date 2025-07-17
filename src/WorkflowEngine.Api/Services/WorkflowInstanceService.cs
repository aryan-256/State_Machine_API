using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowEngine.Core.Exceptions;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Core.Repositories;
using WorkflowEngine.Core.Services;
using WorkflowEngine.Core.Validation;

namespace WorkflowEngine.Api.Services
{
    public class WorkflowInstanceService : IWorkflowInstanceService
    {
        private readonly IWorkflowInstanceRepository _instanceRepository;
        private readonly IWorkflowDefinitionRepository _definitionRepository;

        public WorkflowInstanceService(
            IWorkflowInstanceRepository instanceRepository,
            IWorkflowDefinitionRepository definitionRepository)
        {
            _instanceRepository = instanceRepository;
            _definitionRepository = definitionRepository;
        }

        public async Task<WorkflowInstance> CreateInstanceAsync(Guid definitionId)
        {
            // Get the definition
            var definition = await _definitionRepository.GetByIdAsync(definitionId);
            
            // Get the initial state
            var initialState = definition.GetInitialState();
            if (initialState == null)
            {
                throw new InvalidWorkflowDefinitionException("Workflow definition does not have an initial state");
            }

            // Create a new instance
            var instance = new WorkflowInstance(definitionId, initialState.Id);
            
            // Save and return the instance
            return await _instanceRepository.CreateAsync(instance);
        }

        public async Task<WorkflowInstance> ExecuteActionAsync(Guid instanceId, Guid actionId)
        {
            // Get the instance
            var instance = await _instanceRepository.GetByIdAsync(instanceId);
            
            // Get the definition
            var definition = await _definitionRepository.GetByIdAsync(instance.DefinitionId);
            
            // Get the action
            var action = definition.GetAction(actionId);
            if (action == null)
            {
                throw new ActionNotFoundException(actionId);
            }

            // Validate the action
            var validationResult = WorkflowValidator.ValidateAction(instance, definition, action);
            if (!validationResult.IsValid)
            {
                throw new InvalidActionExecutionException(
                    $"Cannot execute action: {string.Join(", ", validationResult.Errors)}");
            }

            // Update the instance state
            instance.UpdateState(actionId, action.ToStateId);
            
            // Save and return the updated instance
            return await _instanceRepository.UpdateAsync(instance);
        }

        public async Task<List<WorkflowInstance>> GetAllInstancesAsync()
        {
            return await _instanceRepository.GetAllAsync();
        }

        public async Task<WorkflowInstance> GetInstanceAsync(Guid id)
        {
            var instance = await _instanceRepository.GetByIdAsync(id);
            instance.Definition = await _definitionRepository.GetByIdAsync(instance.DefinitionId);
            return instance;
        }

        public async Task<List<WorkflowInstance>> GetInstancesByDefinitionIdAsync(Guid definitionId)
        {
            return await _instanceRepository.GetByDefinitionIdAsync(definitionId);
        }
    }
}
