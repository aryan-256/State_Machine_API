# Configurable Workflow Engine (State-Machine API)

A minimal backend service that implements a configurable state machine workflow engine using .NET 8 and C#. This API allows clients to define workflows as configurable state machines, start workflow instances, execute transitions between states with validation, and inspect the current state of workflow instances.

## Quick Start

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code with C# extensions

### Building and Running the Application
```bash
# Clone the repository
git clone https://github.com/aryan-256/State_Machine_API.git
cd State_Machine_API

# Build the application
dotnet build

# Run the application
dotnet run --project src/WorkflowEngine.Api
```

The API will be available at `https://localhost:7001` and `http://localhost:5000`.

## API Documentation

### Workflow Configuration
- `POST /api/workflow-definitions` - Create a new workflow definition
- `GET /api/workflow-definitions/{id}` - Retrieve a workflow definition by ID
- `GET /api/workflow-definitions` - List all workflow definitions

### Runtime Operations
- `POST /api/workflow-instances` - Start a new workflow instance from a definition
- `POST /api/workflow-instances/{instanceId}/actions/{actionId}` - Execute an action on an instance
- `GET /api/workflow-instances/{id}` - Get current state and history of an instance
- `GET /api/workflow-instances` - List all workflow instances

## Core Concepts

### State
Represents a point in the workflow where an instance can be at a given time:
- **ID/Name**: Unique identifier and descriptive name
- **IsInitial**: Flag indicating if this is the starting state (exactly one required per definition)
- **IsFinal**: Flag indicating if this is an end state (no further actions possible)
- **IsEnabled**: Flag indicating if the state is active

### Action (Transition)
Represents a possible transition between states:
- **ID/Name**: Unique identifier and descriptive name
- **FromStateIds**: Collection of state IDs from which this action can be executed
- **ToStateId**: The target state ID after action execution
- **IsEnabled**: Flag indicating if the action is available

### Workflow Definition
A collection of states and actions that form a complete process flow:
- Must contain exactly one initial state
- States and actions must have unique IDs
- Actions must reference valid states

### Workflow Instance
A running instance of a workflow definition:
- References its definition
- Tracks current state
- Maintains history of executed actions with timestamps

## Implementation Details

The solution follows a minimal API approach with clean domain modeling and separation of concerns:

- **Domain Layer**: Core business logic and entities like State, Action, WorkflowDefinition, and WorkflowInstance
- **Service Layer**: Business logic for creating/managing workflows and executing actions
- **API Layer**: HTTP endpoints and request/response models
- **Infrastructure Layer**: In-memory persistence implementation

## Validation Rules

The system enforces several validation rules:
1. Workflow definitions must have exactly one initial state
2. All state and action IDs must be unique within a definition
3. Actions can only reference states that exist in the definition
4. Actions can only be executed if they are enabled
5. The current state must be in the action's FromStateIds collection
6. Actions cannot be executed on instances in a final state

## Assumptions & Design Decisions

1. Workflow definitions and instances are stored in-memory (no external database)
2. IDs are represented as GUIDs for simplicity
3. A workflow definition cannot be modified after instances have been created from it
4. History of state transitions is stored with timestamps
5. Error responses include descriptive messages to help diagnose issues

## Example Workflow

The included example in `WorkflowEngine.Api.http` demonstrates a document approval process with the following states:
- Draft (Initial)
- Under Review
- Approved (Final)
- Rejected (Final)

And actions:
- Submit for Review (Draft → Under Review)
- Approve (Under Review → Approved)
- Reject (Under Review → Rejected)
- Request Changes (Under Review → Draft)

## Potential Enhancements
- Persist workflow definitions and instances to a database
- Add versioning for workflow definitions
- Support for conditional transitions based on data
- Implement event notifications for state changes
- Add authentication and authorization