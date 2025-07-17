using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Services
{
    public interface IWorkflowDefinitionService
    {
        Task<WorkflowDefinition> CreateDefinitionAsync(WorkflowDefinition definition);
        Task<WorkflowDefinition> GetDefinitionAsync(Guid id);
        Task<List<WorkflowDefinition>> GetAllDefinitionsAsync();
    }
}
