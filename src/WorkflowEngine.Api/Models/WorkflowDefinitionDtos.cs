using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkflowEngine.Api.Models
{
    public class StateDto
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public bool IsInitial { get; set; }
        public bool IsFinal { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string? Description { get; set; }
    }

    public class ActionDto
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public bool IsEnabled { get; set; } = true;
        
        [Required]
        public List<Guid> FromStateIds { get; set; } = new List<Guid>();
        
        [Required]
        public Guid ToStateId { get; set; }
        
        public string? Description { get; set; }
    }

    public class CreateWorkflowDefinitionDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public List<StateDto> States { get; set; } = new List<StateDto>();
        
        [Required]
        public List<ActionDto> Actions { get; set; } = new List<ActionDto>();
        
        public string? Description { get; set; }
    }

    public class WorkflowDefinitionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StateDto> States { get; set; } = new List<StateDto>();
        public List<ActionDto> Actions { get; set; } = new List<ActionDto>();
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
