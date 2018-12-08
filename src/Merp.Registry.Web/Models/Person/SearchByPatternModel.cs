using System;

namespace Merp.Registry.Web.Models.Person
{
    public class SearchByPatternModel
    {
        public int Id { get; set; }

        public Guid OriginalId { get; set; }

        public string Name { get; set; }
    }
}
