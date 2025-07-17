using System;
using System.Collections.Generic;

namespace WorkflowEngine.Core.Models
{
    // Represents a transition/action between workflow states
    public class Action
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public List<Guid> FromStateIds { get; set; } = new List<Guid>();
        public Guid ToStateId { get; set; }
        public string? Description { get; set; }

        // Default constructor assigns a new Guid
        public Action()
        {
            Id = Guid.NewGuid();
        }

        // Parameterized constructor for convenience
        public Action(string name, IEnumerable<Guid> fromStateIds, Guid toStateId, bool isEnabled = true, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            FromStateIds = new List<Guid>(fromStateIds);
            ToStateId = toStateId;
            IsEnabled = isEnabled;
            Description = description;
        }
    }
}
