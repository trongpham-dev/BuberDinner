using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.Host.ValueObjects
{
    public sealed class HostId : ValueObject
    {
        public string Value { get; }

        private HostId(string value)
        {
            this.Value = value;
        }

        public static HostId CreateUnique()
        {
            //return new(Guid.NewGuid());
            return new("dsadasd");
        }

        public static HostId Create(string value)
        {
            return new HostId(value);
        }
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
