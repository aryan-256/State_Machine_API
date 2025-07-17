using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowEngine.Core.Models;

namespace WorkflowEngine.Core.Validation
{
    public class WorkflowValidator
    {
        public static ValidationResult ValidateDefinition(WorkflowDefinition definition)
        {
            var errors = new List<string>();

            // Check if there are states
            if (definition.States == null || !definition.States.Any())
            {
                errors.Add("Workflow definition must have at least one state");
            }
            
            // Check if there's exactly one initial state
            var initialStates = definition.States?.Where(s => s.IsInitial).ToList() ?? new List<State>();
            if (initialStates.Count != 1)
            {
                errors.Add("Workflow definition must have exactly one initial state");
            }

            // Check for duplicate state IDs
            var stateIds = definition.States?.Select(s => s.Id).ToList() ?? new List<Guid>();
            if (stateIds.Count != stateIds.Distinct().Count())
            {
                errors.Add("Workflow definition contains duplicate state IDs");
            }

            // Check for duplicate action IDs
            var actionIds = definition.Actions?.Select(a => a.Id).ToList() ?? new List<Guid>();
            if (actionIds.Count != actionIds.Distinct().Count())
            {
                errors.Add("Workflow definition contains duplicate action IDs");
            }

            // Validate actions
            if (definition.Actions != null)
            {
                foreach (var action in definition.Actions)
                {
                    // Check if the target state exists
                    if (!stateIds.Contains(action.ToStateId))
                    {
                        errors.Add($"Action '{action.Name}' references non-existent target state");
                    }

                    // Check if source states exist
                    foreach (var fromStateId in action.FromStateIds)
                    {
                        if (!stateIds.Contains(fromStateId))
                        {
                            errors.Add($"Action '{action.Name}' references non-existent source state");
                        }
                    }
                }
            }

            return new ValidationResult(errors.Count == 0, errors);
        }

        public static ValidationResult ValidateAction(WorkflowInstance instance, WorkflowDefinition definition, Models.Action action)
        {
            var errors = new List<string>();
            
            // Check if the action exists in the definition
            if (action == null)
            {
                errors.Add("The specified action does not exist in the workflow definition");
                return new ValidationResult(false, errors);
            }
            
            // Check if the action is enabled
            if (!action.IsEnabled)
            {
                errors.Add("The specified action is disabled");
            }
            
            // Get current state
            var currentState = definition.States.FirstOrDefault(s => s.Id == instance.CurrentStateId);
            if (currentState == null)
            {
                errors.Add("Current state does not exist in the workflow definition");
                return new ValidationResult(false, errors);
            }
            
            // Check if current state is a final state
            if (currentState.IsFinal)
            {
                errors.Add("Cannot execute an action from a final state");
            }
            
            // Check if the action can be executed from the current state
            if (!action.FromStateIds.Contains(instance.CurrentStateId))
            {
                errors.Add($"Action '{action.Name}' cannot be executed from the current state '{currentState.Name}'");
            }
            
            // Check if the target state exists
            var targetState = definition.States.FirstOrDefault(s => s.Id == action.ToStateId);
            if (targetState == null)
            {
                errors.Add("Target state does not exist in the workflow definition");
            }
            
            return new ValidationResult(errors.Count == 0, errors);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; }
        public List<string> Errors { get; }

        public ValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }
    }
}
