using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Repositories
{
    public interface IWorkflowDefinitionRepository
    {
        Task<WorkflowDefinition> GetByIdAsync(Guid id);
        Task<List<WorkflowDefinition>> GetAllAsync();
        Task<WorkflowDefinition> CreateAsync(WorkflowDefinition definition);
        Task<bool> ExistsAsync(Guid id);
    }
}
