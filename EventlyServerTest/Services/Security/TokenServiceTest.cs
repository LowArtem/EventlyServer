using EventlyServer.Services.Security;

namespace EventlyServerTest.Services.Security;

public class TokenServiceTest
{
    [Fact]
    public void GetLoginFromToken_Test()
    {
        var token =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVzdEB0ZXN0LmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVTRVIiLCJuYmYiOjE2NjU5Mzc5MDEsImV4cCI6MTY2NTkzNzk2MSwiaXNzIjoiaW5Ib2xpZGF5U2VydmVyIiwiYXVkIjoiaW5Ib2xpZGF5Q2xpZW50In0.Ud6uRIlH93hKsnYY3WoaduXGfqQxYohPX6K53MGZLjI";

        var email = TokenService.GetLoginFromToken(token);
        var email_exp = "test@test.com";
        
        Assert.Equal(email_exp, email);
    }
}