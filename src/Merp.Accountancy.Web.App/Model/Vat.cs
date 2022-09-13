namespace Merp.Accountancy.Web.App.Model
{
    public record Vat
    {
        public Guid Id { get; set; }

        public decimal Rate { get; set; }

        public string Description { get; set; } = string.Empty;

        public override string ToString() => Description;
    }
}
