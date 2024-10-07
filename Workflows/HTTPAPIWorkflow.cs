using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http;
using Elsa.Builders;
using Elsa.Workflows;
using ElsaServer.Models;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace ElsaServer.Workflows
{
    public class HTTPAPIWorkflow : IWorkflow
    {
        public void Build(Elsa.Builders.IWorkflowBuilder builder)
        {
            builder
                .HttpEndpoint("/test-api")
                .Then(context =>
                {
                    var requestBody = context.Input as string;
                    if (string.IsNullOrEmpty(requestBody))
                        throw new InvalidOperationException("Request body is required.");

                    var user = JsonSerializer.Deserialize<UserModel>(requestBody);

                    if (string.IsNullOrEmpty(user?.Name) || string.IsNullOrEmpty(user?.Email))
                        throw new InvalidOperationException("name and email are required.");

                    context.WorkflowExecutionContext.SetVariable("name", user.Name);
                    context.WorkflowExecutionContext.SetVariable("email", user.Email);
                })
                .Then<SendHttpRequest>(http => http
                    .WithUrl(new Uri("https://localhost:5001/api/User/CreateUserAsync"))
                    .WithMethod(HttpMethod.Post.Method)
                    .WithContentType("application/json")
                    .WithContent(async context =>
                    {
                        var name = context.WorkflowExecutionContext.GetVariable<string>("name");
                        var email = context.WorkflowExecutionContext.GetVariable<string>("email");

                        var user = new UserModel
                        {
                            Name = name,
                            Email = email
                        };

                        return await Task.FromResult(JsonSerializer.Serialize(user));
                    })
                )
                .Then<WriteHttpResponse>(httpResponse => httpResponse
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithContent("API request sent successfully!")
                );
        }
    }
}