using Elsa;
using System;
using System.Net;
using System.Net.Mime;
using System.Text;
using Elsa.Expressions.Models;
using Elsa.Http;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows;
using Elsa.Workflows.Models;
using Elsa.Workflows.Memory;
using System.Net.Http.Headers;

namespace ElsaServer.Workflows
{
    public class HelloWorldWorkflow : WorkflowBase
    {
        protected override void Build(IWorkflowBuilder builder)
        {
            var httpEndpoint = new HttpEndpoint
            {
                Path = new Input<string>("/hello"), // Path to access the workflow
                SupportedMethods = new Input<ICollection<string>>(new List<string> { "GET" }), // Allowed HTTP methods
                CanStartWorkflow = true // Allow starting the workflow via HTTP GET
            };

            // Define the HTTP response
            var httpResponse = new WriteHttpResponse
            {
                StatusCode = new (HttpStatusCode.OK), // HTTP status code
                Content = new ("Hello World!"), // Response content
                ContentType = new Input<string>(MediaTypeNames.Text.Plain) // Response content type
                // ResponseHeaders = new HttpResponseHeaders { ["x-generator"] = new[] { "Elsa Workflows" } } // Optional: Add custom headers
            };

            // Build the workflow
            builder.Root = new Sequence
            {
                Activities = new List<IActivity>
                {
                    httpEndpoint,
                    httpResponse
                }
            };
        }
    }
}
