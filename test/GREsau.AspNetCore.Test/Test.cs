using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GREsau.AspNetCore.Test
{
    public class Test
    {
        [Fact]
        public async Task SetsJsonPropertyNamesInValidationProblemDetails()
        {
            string ErrorMessage(string name) => new RangeAttribute(1, 100).FormatErrorMessage(name);

            using var testServer = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestApp.Startup>());
            using var client = testServer.CreateClient();

            var response = await client.PostAsync("/", new StringContent("{}", null, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStreamAsync();
            var problemDetails = await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(responseContent);

            Assert.Equal(3, problemDetails.Errors.Count);

            var fooErrors = Assert.Contains("foo", problemDetails.Errors);
            Assert.Equal(ErrorMessage("foo"), Assert.Single(fooErrors));

            var barErrors = Assert.Contains("foo_bar", problemDetails.Errors);
            Assert.Equal(ErrorMessage("foo_bar"), Assert.Single(barErrors));

            var bazErrors = Assert.Contains("bind_overridden", problemDetails.Errors);
            Assert.Equal(ErrorMessage("display_overridden"), Assert.Single(bazErrors));
        }

        [Fact]
        public async Task DoesNotAffectModelValidationSuccess()
        {
            using var testServer = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<TestApp.Startup>());
            using var client = testServer.CreateClient();

            var json = @"
            {
                ""foo"": 1,
                ""foo_bar"": 2,
                ""baz"": 3
            }
            ";

            var response = await client.PostAsync("/", new StringContent(json, null, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStreamAsync();
            var element = await JsonSerializer.DeserializeAsync<JsonElement>(responseContent);

            Assert.Equal(1, element.GetProperty("foo").GetInt32());
            Assert.Equal(2, element.GetProperty("foo_bar").GetInt32());
            Assert.Equal(3, element.GetProperty("baz").GetInt32());
        }
    }
}
