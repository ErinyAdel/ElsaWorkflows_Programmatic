using Elsa.Workflows;
using System.Net;
using Elsa.Workflows.Models;
using Elsa.Http;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Activities;
using Elsa.Activities.Http;

public class SumWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Root = new WriteLine("Hello from Elsa Server Not Studio!");
    }
}
