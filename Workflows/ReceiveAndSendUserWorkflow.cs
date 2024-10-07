using Elsa.Builders;
using Elsa.Activities.Http;
using Elsa.Activities.Primitives;
using Elsa.Activities.Console;
using Elsa.Services.Models;
using System.Text.Json;
using System.Net.Http;
using System.Text;
using ElsaServer.Models;
using Elsa.Activities.ControlFlow;
using Elsa.Workflows;

namespace ElsaServer.Workflows
{
    public class ReceiveAndSendUserWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                // Step 1: Define an HTTP Trigger to receive input from Postman
                .HttpEndpoint(endpoint => endpoint
                    .WithPath("/receive-user")
                    .WithMethod(HttpMethod.Post.Method)
                    .WithReadContent())  // This will read the POST request body content

                // Step 2: Deserialize the incoming JSON object into the 'User' object
                .Then(context =>
                {
                    var requestBody = context.GetInput<string>();
                    var receivedUser = JsonSerializer.Deserialize<UserModel>(requestBody);

                    // You now have access to 'receivedUser' from the incoming HTTP POST body.
                    context.SetVariable("ReceivedUser", receivedUser);
                })

                // Step 3: Make an API call to send the received object to another endpoint
                .Then(context =>
                {
                    // Get the user from the workflow variable
                    var userToSend = context.GetVariable<UserModel>("ReceivedUser");

                    // Serialize the user object to JSON for sending
                    var userJson = JsonSerializer.Serialize(userToSend);

                    // Make an HTTP POST request to the external API to send this user object
                    var httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5001/api/User/CreateUserAsync")
                    {
                        Content = new StringContent(userJson, Encoding.UTF8, "application/json")
                    };

                    // Await response and read the content
                    var response = httpClient.SendAsync(request).Result;
                    var responseBody = response.Content.ReadAsStringAsync().Result;

                    // Deserialize the API response into a User object
                    var userResponse = JsonSerializer.Deserialize<UserModel>(responseBody);

                    // Set the UserId as a workflow variable
                    context.SetVariable("name", userResponse.Name);
                })

                // Step 4: Log the UserId to the console for debugging
                .Then<WriteLine>(setup => setup.WithText(context => $"UserId retrieved from API and set as: {context.GetVariable<int>("UserId")}"))

                // Finish the workflow
                .Then<Finish>();
        }
    }
}
