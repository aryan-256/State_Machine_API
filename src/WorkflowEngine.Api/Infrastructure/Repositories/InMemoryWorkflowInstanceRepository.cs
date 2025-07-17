using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowEngine.Core.Exceptions;
using WorkflowEngine.Core.Models;
using WorkflowEngine.Core.Repositories;

namespace WorkflowEngine.Api.Infrastructure.Repositories
{
    public class InMemoryWorkflowInstanceRepository : IWorkflowInstanceRepository
    {
        private readonly List<WorkflowInstance> _instances = new();

        public Task<WorkflowInstance> CreateAsync(WorkflowInstance instance)
        {
            _instances.Add(instance);
            return Task.FromResult(instance);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(_instances.Any(i => i.Id == id));
        }

        public Task<List<WorkflowInstance>> GetAllAsync()
        {
            return Task.FromResult(_instances);
        }

        public Task<List<WorkflowInstance>> GetByDefinitionIdAsync(Guid definitionId)
        {
            return Task.FromResult(_instances.Where(i => i.DefinitionId == definitionId).ToList());
        }

        public Task<WorkflowInstance> GetByIdAsync(Guid id)
        {
            var instance = _instances.FirstOrDefault(i => i.Id == id);
            if (instance == null)
            {
                throw new WorkflowInstanceNotFoundException(id);
            }
            
            return Task.FromResult(instance);
        }

        public Task<WorkflowInstance> UpdateAsync(WorkflowInstance instance)
        {
            var existingInstance = _instances.FirstOrDefault(i => i.Id == instance.Id);
            if (existingInstance == null)
            {
                throw new WorkflowInstanceNotFoundException(instance.Id);
            }

            // Since we're working with in-memory objects, the reference is already updated
            // In a real database scenario, we would update the entity here
            
            return Task.FromResult(instance);
        }
    }
}
