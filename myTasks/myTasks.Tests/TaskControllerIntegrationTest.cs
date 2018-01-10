using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using myTasks.Models;
using Xunit;

namespace myTasks.Tests
{
    public class TaskControllerIntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public TaskControllerIntegrationTest()
        {
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task CreateReturnsBadRequestForEmptyName()
        {
            var taskItem = new TaskItemDto { Name = string.Empty, IsComplete = false };

            var response = await _client.PostAsJsonAsync("api/tasks", taskItem);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateReturnsProperErrorMessageForEmptyName()
        {
            var taskItem = new TaskItemDto { Name = string.Empty, IsComplete = false };

            var response = await _client.PostAsJsonAsync("api/tasks", taskItem);

            var returnMessage = response.Content.ReadAsStringAsync().Result;
            var expectedMessage = new TaskItemDtoValidator().Validate(taskItem).Errors[0].ErrorMessage;
            Assert.Contains(expectedMessage, returnMessage);
        }

        [Fact]
        public async Task CreateReturnsBadRequestForNoName()
        {
            var taskItem = new TaskItemDto { IsComplete = false };

            var response = await _client.PostAsJsonAsync("api/tasks", taskItem);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
