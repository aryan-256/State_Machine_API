using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkflowEngine.Core.Models
{
    // Represents a workflow definition, which is a collection of states and actions
    public class WorkflowDefinition
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<State> States { get; set; } = new List<State>();
        public List<Action> Actions { get; set; } = new List<Action>();
        public string? Description { get; set; }
        public DateTime CreatedAt { get; private set; }

        public WorkflowDefinition()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public WorkflowDefinition(string name, List<State> states, List<Action> actions, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            States = states;
            Actions = actions;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        // Returns the initial state of the workflow (should be exactly one)
        public State? GetInitialState()
        {
            return States.FirstOrDefault(s => s.IsInitial);
        }

        // Returns a state by its ID
        public State? GetState(Guid stateId)
        {
            return States.FirstOrDefault(s => s.Id == stateId);
        }

        // Returns an action by its ID
        public Action? GetAction(Guid actionId)
        {
            return Actions.FirstOrDefault(a => a.Id == actionId);
        }
    }
}
