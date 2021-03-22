using System.Linq;

namespace TFNValidate.Services.Implementation
{
    public class TFNValidator : ITFNValidator
    {
        public bool Validate(int taxFileNumber)
        {
            var digits = GetDigitsFrom(taxFileNumber);
            if (digits.Length == 9) return IsNineDigitTfnValid(digits);
            if (digits.Length == 8) return IsEightDigitTfnValid(digits);
            return false;
        }

        private int[] GetDigitsFrom(int taxFileNumber)
        {
            return taxFileNumber.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();
        }

        private bool IsNineDigitTfnValid(int[] digits)
        {
            int[] weights = {10, 7, 8, 4, 6, 3, 5, 2, 1};
            var weightedSum = GetWeightedSum(digits, weights);
            return weightedSum % 11 == 0;
        }

        private int GetWeightedSum(int[] digits, int[] weights)
        {
            return digits.Select((digit, index) => digit * weights[index]).Sum();
        }

        private bool IsEightDigitTfnValid(int[] digits)
        {
            int[] weights = {10, 7, 8, 4, 6, 3, 5, 1};
            var weightedSum = GetWeightedSum(digits, weights);
            return weightedSum % 11 == 0;
        }
    }
}