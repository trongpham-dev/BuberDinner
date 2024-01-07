using BuberDinner.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Domain.Menu.ValueObjects
{
    public sealed class MenuSectionId : ValueObject
    {

        public Guid Value { get; }

        private MenuSectionId(Guid value)
        {
            this.Value = value;
        }

        public static MenuSectionId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
