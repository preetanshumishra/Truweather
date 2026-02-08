namespace TruweatherAPI.Tests.Fixtures;

public static class MockSignInManager
{
    public static Mock<SignInManager<User>> Create(Mock<UserManager<User>> userManager)
    {
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        return new Mock<SignInManager<User>>(
            userManager.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            null!, null!, null!, null!);
    }
}
