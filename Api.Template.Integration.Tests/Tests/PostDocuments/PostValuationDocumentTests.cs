using System.Net;
using System.Text;
using FluentAssertions;

namespace Api.Template.Integration.Tests.Tests.PostDocuments
{
    [Collection(nameof(FunctionTestFixtureCollection))]
    public class PostDocumentTests(BaseFunctionTestFixture baseFixture)
    {
        [Theory]
        [InlineData(793306)]
        public async Task PostDocument_With_Invalid_Data_Should_Return_BadRequest(int itemId)
        {
            var url = $"api/items/{itemId}/documents";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new MultipartFormDataContent
            {
                { new StringContent("test-user-name"), "username" },
                { new StringContent("test@outlook.com"), "email"},
                { new StringContent("test-password"), "password"}
            };
            request.Content = content;

            // Act
            var response = await baseFixture.Client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(793306)]
        public async Task PostDocument_With_Valid_Data_Should_Return_Created_Response(int itemId)
        {
            var url = $"api/items/{itemId}/documents";
            var fileContent = "blah blah blah blah";
            var fileBytes = Encoding.UTF8.GetBytes(fileContent);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(fileBytes, 0, fileBytes.Length), "profile_pic", "test.txt" }
            };
            request.Content = content;

            // Act
            var response = await baseFixture.Client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
