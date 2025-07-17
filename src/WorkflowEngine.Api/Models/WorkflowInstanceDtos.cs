using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkflowEngine.Api.Models
{
    public class StateTransitionDto
    {
        public Guid FromStateId { get; set; }
        public string FromStateName { get; set; } = string.Empty;
        public Guid ToStateId { get; set; }
        public string ToStateName { get; set; } = string.Empty;
        public Guid ActionId { get; set; }
        public string ActionName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class CreateWorkflowInstanceDto
    {
        [Required]
        public Guid DefinitionId { get; set; }
    }

    public class WorkflowInstanceDto
    {
        public Guid Id { get; set; }
        public Guid DefinitionId { get; set; }
        public string DefinitionName { get; set; } = string.Empty;
        public Guid CurrentStateId { get; set; }
        public string CurrentStateName { get; set; } = string.Empty;
        public bool IsInFinalState { get; set; }
        public List<StateTransitionDto> History { get; set; } = new List<StateTransitionDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ExecuteActionDto
    {
        [Required]
        public Guid ActionId { get; set; }
    }
}
