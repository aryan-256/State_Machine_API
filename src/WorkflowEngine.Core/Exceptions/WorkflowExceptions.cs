using System;

namespace WorkflowEngine.Core.Exceptions
{
    public class WorkflowException : Exception
    {
        public WorkflowException(string message) : base(message)
        {
        }
    }

    public class WorkflowDefinitionNotFoundException : WorkflowException
    {
        public WorkflowDefinitionNotFoundException(Guid id) 
            : base($"Workflow definition with ID '{id}' was not found")
        {
        }
    }

    public class WorkflowInstanceNotFoundException : WorkflowException
    {
        public WorkflowInstanceNotFoundException(Guid id) 
            : base($"Workflow instance with ID '{id}' was not found")
        {
        }
    }

    public class ActionNotFoundException : WorkflowException
    {
        public ActionNotFoundException(Guid id) 
            : base($"Action with ID '{id}' was not found")
        {
        }
    }

    public class InvalidWorkflowDefinitionException : WorkflowException
    {
        public InvalidWorkflowDefinitionException(string message) 
            : base(message)
        {
        }
    }

    public class InvalidActionExecutionException : WorkflowException
    {
        public InvalidActionExecutionException(string message) 
            : base(message)
        {
        }
    }
}
