using WorkflowEngine.Models;
using WorkflowEngine.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/workflow", (WorkflowDefinition def) =>
{
    if (InMemoryStore.Definitions.ContainsKey(def.Id))
        return Results.BadRequest("Workflow with this ID already exists.");

    if (def.States.Count(s => s.IsInitial) != 1)
        return Results.BadRequest("Exactly one initial state is required.");

    InMemoryStore.Definitions[def.Id] = def;
    return Results.Ok("Workflow definition created successfully.");
});

app.MapGet("/workflow/{id}", (string id) =>
{
    if (!InMemoryStore.Definitions.TryGetValue(id, out var def))
        return Results.NotFound("Workflow not found.");

    return Results.Ok(def);
});

app.MapPost("/workflow/{id}/start", (string id) =>
{
    if (!InMemoryStore.Definitions.TryGetValue(id, out var def))
        return Results.NotFound("Workflow definition not found.");

    var initial = def.States.FirstOrDefault(s => s.IsInitial && s.Enabled);
    if (initial == null)
        return Results.BadRequest("Initial state is missing or disabled.");

    var instance = new WorkflowInstance
    {
        Id = Guid.NewGuid().ToString(),
        DefinitionId = id,
        CurrentStateId = initial.Id
    };

    InMemoryStore.Instances[instance.Id] = instance;
    return Results.Ok(instance);
});

app.MapPost("/instance/{id}/execute", (string id, string actionId) =>
{
    if (!InMemoryStore.Instances.TryGetValue(id, out var instance))
        return Results.NotFound("Instance not found.");

    var def = InMemoryStore.Definitions[instance.DefinitionId];

    var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
    if (action == null || !action.Enabled)
        return Results.BadRequest("Action not found or disabled.");

    if (!action.FromStates.Contains(instance.CurrentStateId))
        return Results.BadRequest("Action is not allowed from current state.");

    var targetState = def.States.FirstOrDefault(s => s.Id == action.ToState && s.Enabled);
    if (targetState == null)
        return Results.BadRequest("Target state is missing or disabled.");

    if (def.States.First(s => s.Id == instance.CurrentStateId).IsFinal)
        return Results.BadRequest("Cannot execute actions from a final state.");

    instance.CurrentStateId = targetState.Id;
    instance.History.Add((actionId, DateTime.UtcNow));

    return Results.Ok(instance);
});

app.MapGet("/instance/{id}", (string id) =>
{
    if (!InMemoryStore.Instances.TryGetValue(id, out var instance))
        return Results.NotFound("Instance not found.");

    return Results.Ok(instance);
});

app.Run();
