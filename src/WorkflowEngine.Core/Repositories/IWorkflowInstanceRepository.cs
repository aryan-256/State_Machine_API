using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Repositories
{
    public interface IWorkflowInstanceRepository
    {
        Task<WorkflowInstance> GetByIdAsync(Guid id);
        Task<List<WorkflowInstance>> GetAllAsync();
        Task<List<WorkflowInstance>> GetByDefinitionIdAsync(Guid definitionId);
        Task<WorkflowInstance> CreateAsync(WorkflowInstance instance);
        Task<WorkflowInstance> UpdateAsync(WorkflowInstance instance);
        Task<bool> ExistsAsync(Guid id);
    }
}
