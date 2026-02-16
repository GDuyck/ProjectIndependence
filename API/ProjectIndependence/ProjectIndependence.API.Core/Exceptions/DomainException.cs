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

    public class IncreaseStockException : DomainException
    {
        public IncreaseStockException(int increaseAmount)
            : base($"Cannot increase stock by {increaseAmount}. Increase amount must be positive.")
        {
        }
    }

    public class DecreaseStockNegativeException : DomainException
    {
        public DecreaseStockNegativeException(int decreaseAmount)
            : base($"Cannot decrease stock by {decreaseAmount}. Decrease amount must be positive.")
        {
        }
    }

    public class DecreaseStockexception : DomainException
    {
        public DecreaseStockexception(int currentStock, int decreaseAmount)
            : base($"Cannot decrease stock by {decreaseAmount}. Current stock is {currentStock}.")
        {
        }
    }

    public class ReserveStockNegativeException : DomainException
    {
        public ReserveStockNegativeException(int reserveAmount)
            : base($"Cannot reserve {reserveAmount} items. Reserve amount must be positive.")
        {
        }
    }

    public class ReserveStockException : DomainException
    {
        public ReserveStockException(int currentStock, int reserveAmount)
            : base($"Cannot reserve {reserveAmount} items. Current stock is {currentStock}.")
        {
        }
    }

    public class ReleaseStockNegativeException : DomainException
    {
        public ReleaseStockNegativeException(int releaseAmount)
            : base($"Cannot release {releaseAmount} items. Release amount must be positive.")
        {
        }
    }

    public class ReleaseStockException : DomainException
    {
        public ReleaseStockException(int reservedStock, int releaseAmount)
            : base($"Cannot release {releaseAmount} items. Currently reserved stock is {reservedStock}.")
        {
        }
    }
}