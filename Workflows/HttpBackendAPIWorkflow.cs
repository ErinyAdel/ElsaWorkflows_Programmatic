using Elsa.Activities.ControlFlow;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using ElsaServer.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

public class HttpBackendAPIWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        var user = builder.WithVariable<UserModel>().WithMemoryStorage();

        builder.Root = new Sequence
        {
            Activities =
            {
                // 1. HTTP Endpoint: This listens for requests at the specified path.
                new HttpEndpoint
                {
                    Path = new("/run-api"), // This is the URL endpoint
                    CanStartWorkflow = true
                },

                // 2.Send an HTTP POST Request to the external API
                new SendHttpRequest
                {
                    Url = new Input<Uri?>(context => new Uri("https://localhost:5001/api/User/CreateUserAsync")),
                    Method = new Input<string>("POST"),

                    // Serialize the user data to JSON format and send it as request content.
                    Content = new Input<object>(context =>
                    {
                        var name = "Eriny";
                        var email = "e.adel@singleclic.com";

                        //var name = context.GetVariable<string>("name");
                        //var email = context.GetVariable<string>("email");

                        // Check for required values
                        //if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
                        //        throw new InvalidOperationException("Name and Email are required.");

                        var user = new UserModel
                        {
                            Name = name,
                            Email = email
                        };

                        return JsonSerializer.Serialize(user);
                        //return JsonSerializer.Serialize(user, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    }),

                    // Specify the content type as JSON.
                    ContentType = new Input<string?>("application/json")
                },

                // 3.Respond with a success message or handle error responses.
                new WriteHttpResponse
                {
                    Content = new Input<object>(context => "API request sent successfully!"),
                    StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK)
                }
            }
        };
    }
}

/*
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Http.Models;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using ElsaServer.Activities;
using ElsaServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;


public class HttpBackendAPIWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        //var name = builder.WithVariable<string>("name");
        var email = builder.WithVariable<string>();

        builder.WithVariable("name", "myName");


        // Define the workflow root
        builder.Root = new Sequence
        {
            Activities =
            {
                // 1. HTTP Endpoint to listen for requests
                new HttpEndpoint
                {
                    Path = new("/run-api"), // This is the URL endpoint
                    CanStartWorkflow = true
                },

                // 2. Capture Name from the query parameters
                //new SetVariable<string>("name", context =>
                //{
                //    var httpRequest = context.GetInput<HttpRequestModel>("HttpRequest");
                //    var queryParams = httpRequest.QueryString;
                //    var name = queryParams["name"];
                //    if (!string.IsNullOrEmpty(name))
                //    {
                //        return "nnnnnn";
                //    }
                //    throw new InvalidOperationException("Name is required.");
                //}),

                new SetVariable
                {
                    Variable = name,
                    Value = new(context => context.GetInput("name"))
                },

                // 3. Capture Email from the query parameters
                new SetVariable
                {
                    Variable = email,
                    Value = new(context => context.GetInput("email"))
                },

                // 4. Send an HTTP POST request to the external API
                new SendHttpRequest
                {
                    Url = new Input<Uri>(context => new Uri("https://localhost:5001/api/User/CreateUserAsync")),
                    Method = new Input<string>("POST"),

                    Content = new Input<object>(context =>
                    {
                        // Prepare the user model
                        var user = new UserModel
                        {
                            Name = context.GetVariable<string>("name"),
                            Email = context.GetVariable<string>("email")
                        };

                        if(user.Name == null || user.Email == null)
                        {
                            user = new UserModel
                            {
                                Name = "name",
                                Email = "email"
                            };
                        }

                        var aaaaaaa = context.GetVariable<string>("name");

                        return JsonSerializer.Serialize(user);
                    }),

                    ContentType = new Input<string>("application/json")
                },

                // 5. Write response
                new WriteHttpResponse
                {
                    Content = new Input<object>(context => "API request sent successfully!"),
                    StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK)
                }
            }
        };
    }
}

*/