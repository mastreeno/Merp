namespace Merp.Web.Auth.Configuration
{
    public class ClientConfiguration
    {
        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string Endpoint { get; set; }

        public string RedirectUri { get; set; }

        public string PostLogoutUri { get; set; }

        public string Secret { get; set; }
    }
}
