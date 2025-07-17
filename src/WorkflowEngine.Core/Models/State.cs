using System;

namespace WorkflowEngine.Core.Models
{
    // Represents a state in the workflow (e.g., Draft, Approved)
    public class State
    {
        // Unique identifier for the state
        public Guid Id { get; set; }
        // Name of the state (for display)
        public string Name { get; set; } = string.Empty;
        // True if this is the initial state of the workflow
        public bool IsInitial { get; set; }
        // True if this is a final state (no further transitions)
        public bool IsFinal { get; set; }
        // True if the state is currently enabled/active
        public bool IsEnabled { get; set; } = true;
        // Optional description for the state
        public string? Description { get; set; }

        // Default constructor assigns a new Guid
        public State()
        {
            Id = Guid.NewGuid();
        }

        // Parameterized constructor for convenience
        public State(string name, bool isInitial = false, bool isFinal = false, bool isEnabled = true, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsInitial = isInitial;
            IsFinal = isFinal;
            IsEnabled = isEnabled;
            Description = description;
        }
    }
}
