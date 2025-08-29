using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Core.ValueObjects
{
    public class TaxRate
    {
        public int Value { get; private set; }
        private static readonly int[] AllowedValues = { 0, 6, 12, 21 };

        public TaxRate(int value)
        {
            if (!AllowedValues.Contains(value))
                throw new ArgumentException();

            Value = value;
        }

        public static implicit operator int(TaxRate taxRate) => taxRate.Value;
        public static implicit operator TaxRate(int value) => new TaxRate(value);
    }
    
    public record ProductSnapshot(Guid id, string name, decimal price, TaxRate taxRate);
}