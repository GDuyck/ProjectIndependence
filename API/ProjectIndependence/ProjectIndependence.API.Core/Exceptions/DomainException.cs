namespace ProjectIndependence.API.Core.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {
        }
    }

    public class InvalidTaxRateException : DomainException
    {
        public InvalidTaxRateException(int attemptedValue)
            : base($"Invalid tax rate: {attemptedValue}. Allowed values are 0, 6, 12 and 21")
        {
        }
    }

    public class ProductCodeAlreadyExistsException : DomainException
    {
        public ProductCodeAlreadyExistsException(string productCode)
            : base($"A product with code {productCode} already exists.")
        {
        }
    }
}