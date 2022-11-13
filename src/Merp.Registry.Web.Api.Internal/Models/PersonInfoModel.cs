namespace Merp.Registry.Web.Api.Internal.Models
{
    public class PersonInfoModel
    {
        public int Id { get; set; }

        public Guid OriginalId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
