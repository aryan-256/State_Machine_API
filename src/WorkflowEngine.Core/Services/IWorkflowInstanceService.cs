using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Services
{
    public interface IWorkflowInstanceService
    {
        Task<WorkflowInstance> CreateInstanceAsync(Guid definitionId);
        Task<WorkflowInstance> ExecuteActionAsync(Guid instanceId, Guid actionId);
        Task<WorkflowInstance> GetInstanceAsync(Guid id);
        Task<List<WorkflowInstance>> GetAllInstancesAsync();
        Task<List<WorkflowInstance>> GetInstancesByDefinitionIdAsync(Guid definitionId);
    }
}
