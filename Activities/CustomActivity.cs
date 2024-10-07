using ElsaServer.Interfaces;
using Elsa;
using Elsa.Services.Models;
using System.Threading.Tasks;
using Elsa.Services;
using ElsaServer.Models;
using Elsa.ActivityResults;

namespace ElsaServer.Activities
{
    public class CustomActivity : Activity
    {
        //private readonly Func<ActivityExecutionContext, string, HttpBackendAPIWorkflow> _deserializer;
        //private readonly string _variableName;

        //public CustomActivity()
        //{
        //}

        //public CustomActivity(string variableName, Func<ActivityExecutionContext, string, HttpBackendAPIWorkflow> deserializer)
        //{
        //    _variableName = variableName;
        //    _deserializer = deserializer;
        //}

        //// Ensure the method is public and correctly overrides the base class method
        //public override async ValueTask<IActivityExecutionResult> ExecuteAsync(ActivityExecutionContext context)
        //{
        //    var input = context.Input as string; // Ensure input is a string
        //    var result = _deserializer(context, input);
        //    context.SetVariable(_variableName, result); // Set the result in the context
        //    return await ValueTask.FromResult(ActivityExecutionResult.Next());
        //}
    }
}
