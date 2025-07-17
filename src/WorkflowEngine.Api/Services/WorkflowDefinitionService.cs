using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowEngine.Core.Exceptions;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Core.Repositories;
using WorkflowEngine.Core.Services;
using WorkflowEngine.Core.Validation;

namespace WorkflowEngine.Api.Services
{
    public class WorkflowDefinitionService : IWorkflowDefinitionService
    {
        private readonly IWorkflowDefinitionRepository _repository;

        public WorkflowDefinitionService(IWorkflowDefinitionRepository repository)
        {
            _repository = repository;
        }

        public async Task<WorkflowDefinition> CreateDefinitionAsync(WorkflowDefinition definition)
        {
            // Validate the definition
            var validationResult = WorkflowValidator.ValidateDefinition(definition);
            if (!validationResult.IsValid)
            {
                throw new InvalidWorkflowDefinitionException(
                    $"Invalid workflow definition: {string.Join(", ", validationResult.Errors)}");
            }

            // Create the definition
            return await _repository.CreateAsync(definition);
        }

        public async Task<List<WorkflowDefinition>> GetAllDefinitionsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<WorkflowDefinition> GetDefinitionAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
