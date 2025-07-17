using System;
using System.Collections.Generic;

namespace WorkflowEngine.Core.Models
{
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

    public class WorkflowInstance
    {
        public Guid Id { get; set; }
        public Guid DefinitionId { get; set; }
        public Guid CurrentStateId { get; set; }
        public List<StateTransition> History { get; set; } = new List<StateTransition>();
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        
        // We don't want EF Core to track this
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

        public void UpdateState(Guid actionId, Guid newStateId)
        {
            var transition = new StateTransition(CurrentStateId, newStateId, actionId);
            History.Add(transition);
            CurrentStateId = newStateId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
