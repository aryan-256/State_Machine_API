using System;
using System.Collections.Generic;

namespace WorkflowEngine.Core.Models
{
    // Represents a record of a state transition in a workflow instance
    public class StateTransition
    {
        public Guid FromStateId { get; set; }
        public Guid ToStateId { get; set; }
        public Guid ActionId { get; set; }
        public DateTime Timestamp { get; set; }
        
        public StateTransition(Guid fromStateId, Guid toStateId, Guid actionId)
        {
            FromStateId = fromStateId;
            ToStateId = toStateId;
            ActionId = actionId;
            Timestamp = DateTime.UtcNow;
        }
    }

    // Represents a running instance of a workflow definition
    public class WorkflowInstance
    {
        public Guid Id { get; set; }
        public Guid DefinitionId { get; set; }
        public Guid CurrentStateId { get; set; }
        public List<StateTransition> History { get; set; } = new List<StateTransition>();
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        
        // Reference to the workflow definition (not persisted)
        public WorkflowDefinition? Definition { get; set; }

        public WorkflowInstance()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public WorkflowInstance(Guid definitionId, Guid initialStateId)
        {
            Id = Guid.NewGuid();
            DefinitionId = definitionId;
            CurrentStateId = initialStateId;
            CreatedAt = DateTime.UtcNow;
        }

        // Updates the current state and records the transition
        public void UpdateState(Guid actionId, Guid newStateId)
        {
            var transition = new StateTransition(CurrentStateId, newStateId, actionId);
            History.Add(transition);
            CurrentStateId = newStateId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
