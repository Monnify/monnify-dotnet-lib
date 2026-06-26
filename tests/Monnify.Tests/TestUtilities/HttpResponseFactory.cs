using System.Net;
using System.Text;

namespace Monnify.Tests.TestUtilities;

internal static class HttpResponseFactory
{
    public static HttpResponseMessage Json(HttpStatusCode statusCode, string json) =>
        new(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };

    public static HttpResponseMessage LoginSuccess(string accessToken, int expiresIn = 3600) => Json(
        HttpStatusCode.OK,
        $$"""
        {
          "requestSuccessful": true,
          "responseMessage": "success",
          "responseCode": "0",
          "responseBody": {
            "accessToken": "{{accessToken}}",
            "expiresIn": {{expiresIn}}
          }
        }
        """);

    public static HttpResponseMessage Unauthorized() => new(HttpStatusCode.Unauthorized)
    {
        Content = new StringContent(
            """{ "requestSuccessful": false, "responseMessage": "Invalid token", "responseCode": "99" }""",
            Encoding.UTF8,
            "application/json"),
    };
}
