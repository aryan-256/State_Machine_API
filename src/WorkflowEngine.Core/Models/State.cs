using System;

namespace WorkflowEngine.Core.Models
{
    public class State
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsInitial { get; set; }
        public bool IsFinal { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string? Description { get; set; }

        public State()
        {
            Id = Guid.NewGuid();
        }

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
