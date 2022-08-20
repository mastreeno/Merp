namespace Merp.Accountancy.Web.App.Services
{
    public class UrlBuilder
    {
        private readonly string urlPrefix = "/accountancy";

        public string BuildAccountancyHomePageUrl() => urlPrefix;

        public string BuildSearchJobOrdersUrl() => $"{urlPrefix}/joborder/search";

        public string BuildCreateJobOrderUrl() => $"{urlPrefix}/joborder/create";

        public string BuildJobOrderDetailUrl(Guid jobOrderId) => $"{urlPrefix}/joborder/detail/{jobOrderId}";
    }
}
