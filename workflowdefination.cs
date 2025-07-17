namespace WorkflowEngine.Models;

public class WorkflowDefinition
{
    public string Id { get; set; }
    public List<State> States { get; set; }
    public List<Action> Actions { get; set; }
}
