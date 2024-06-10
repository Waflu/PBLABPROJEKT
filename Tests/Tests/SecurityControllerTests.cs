using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System.Text;
using WebApi.Models;
using WebApi.Service;
using Xunit;

public class SecurityControllerTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly SecurityController _controller;
    private readonly IOptions<JwtConfig> _config;

    public SecurityControllerTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _config = Options.Create(new JwtConfig
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            Key = "TestSecretKey12345678901234567890" 
        });
        _controller = new SecurityController(_config, _repoMock.Object);
    }

    [Fact]
    public void GenerateToken_AuthorizedUser_ReturnsOkResultWithToken()
    {
        var user = new WebApiUser { UserName = "testuser", Password = "password" };
        _repoMock.Setup(repo => repo.AuthorizeUser(user.UserName, user.Password)).Returns(true);
        var result = _controller.GenerateToken(user);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.IsType<string>(okResult.Value);
    }

    [Fact]
    public void GenerateToken_UnauthorizedUser_ReturnsUnauthorizedResult()
    {
        var user = new WebApiUser { UserName = "testuser", Password = "wrongpassword" };
        _repoMock.Setup(repo => repo.AuthorizeUser(user.UserName, user.Password)).Returns(false);

        var result = _controller.GenerateToken(user);

        Assert.IsType<UnauthorizedResult>(result);
    }
}
