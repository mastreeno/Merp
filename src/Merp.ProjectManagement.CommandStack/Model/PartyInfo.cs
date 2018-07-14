using System;
using System.Collections.Generic;
using System.Text;

namespace Merp.ProjectManagement.CommandStack.Model
{
    public class PartyInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public PartyInfo(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("A non empty party Id must be specificed", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Party name must be specified", nameof(name));

            Id = id;
            Name = name;
        }
    }
}
