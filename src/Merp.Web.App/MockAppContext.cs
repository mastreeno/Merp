namespace Merp.Web.App
{
    public class MockAppContext : Merp.Web.IAppContext
    {
        private static readonly Guid _mockUserId = Guid.NewGuid();

        public Guid UserId => _mockUserId;
    }
}