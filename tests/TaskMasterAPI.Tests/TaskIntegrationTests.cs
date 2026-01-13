using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace TaskMasterAPI.Tests;

public class TasksIntegrationTests
{
    [Fact]
    public async Task Post_then_get_returns_the_created_task()
    {
        using var factory = new TaskMasterApiFactory();
        using var client = factory.CreateClient();

        // POST create
        var postResponse = await client.PostAsJsonAsync("/api/tasks", new { title = "Integration Created" });
        Assert.True(postResponse.IsSuccessStatusCode);

        // GET all should include it
        var getResponse = await client.GetAsync("/api/tasks");
        getResponse.EnsureSuccessStatusCode();

        var body = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("Integration Created", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Post_invalid_input_returns_400_with_validation_details_json()
    {
        using var factory = new TaskMasterApiFactory();
        using var client = factory.CreateClient();

        var json = "{\"title\":\"\"}";
        var response = await client.PostAsync(
            "/api/tasks",
            new StringContent(json, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();

        // Works with both ValidationProblemDetails and similar shapes
        Assert.Contains("errors", body, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("title", body, StringComparison.OrdinalIgnoreCase);
    }
}

