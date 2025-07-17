using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowEngine.Core.Exceptions;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Core.Repositories;

namespace WorkflowEngine.Api.Infrastructure.Repositories
{
    public class InMemoryWorkflowDefinitionRepository : IWorkflowDefinitionRepository
    {
        private readonly List<WorkflowDefinition> _definitions = new();

        public Task<WorkflowDefinition> CreateAsync(WorkflowDefinition definition)
        {
            _definitions.Add(definition);
            return Task.FromResult(definition);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(_definitions.Any(d => d.Id == id));
        }

        public Task<List<WorkflowDefinition>> GetAllAsync()
        {
            return Task.FromResult(_definitions);
        }

        public Task<WorkflowDefinition> GetByIdAsync(Guid id)
        {
            var definition = _definitions.FirstOrDefault(d => d.Id == id);
            if (definition == null)
            {
                throw new WorkflowDefinitionNotFoundException(id);
            }
            
            return Task.FromResult(definition);
        }
    }
}
