namespace CacheInvalidation.Api.Entities
{
    public class Money
    {
        private readonly decimal _value;

        public decimal Value => _value;

        public Money(decimal value)
        {
            _value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public override string ToString() => Value.ToString("F2");
    }
}
