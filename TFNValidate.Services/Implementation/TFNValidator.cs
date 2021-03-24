using System.Linq;

namespace TFNValidate.Services.Implementation
{
    public class TfnValidator : ITfnValidator
    {
        public bool Validate(string taxFileNumber)
        {
            var digits = GetDigitsFrom(taxFileNumber);
            if (digits.Length == 9) return IsNineDigitTfnValid(digits);
            if (digits.Length == 8) return IsEightDigitTfnValid(digits);
            return false;
        }

        private char[] GetDigitsFrom(string taxFileNumber)
        {
            return taxFileNumber.ToCharArray();
        }

        private bool IsNineDigitTfnValid(char[] digits)
        {
            int[] weights = {10, 7, 8, 4, 6, 3, 5, 2, 1};
            var weightedSum = GetWeightedSum(digits, weights);
            return weightedSum % 11 == 0;
        }

        private int GetWeightedSum(char[] digits, int[] weights)
        {
            return digits.Select((digit, index) => int.Parse(digit.ToString()) * weights[index]).Sum();
        }

        private bool IsEightDigitTfnValid(char[] digits)
        {
            int[] weights = {10, 7, 8, 4, 6, 3, 5, 1};
            var weightedSum = GetWeightedSum(digits, weights);
            return weightedSum % 11 == 0;
        }
    }
}