namespace TruweatherAPI.Tests.Fixtures;

public static class MockUserManager
{
    public static Mock<UserManager<User>> Create()
    {
        var store = new Mock<IUserStore<User>>();
        var mgr = new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        return mgr;
    }
}
