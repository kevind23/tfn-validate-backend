using System.Collections.Generic;
using System.Linq;

namespace TFNValidate.Services.Implementation
{
    public class LinkedNumberChecker : ILinkedNumberChecker
    {
        public bool AreLinkedNumbersOverThreshold(int[] numbers, int maxLinkedCount)
        {
            var seenDigitGroups = new HashSet<string>();
            var linkedCount = 1;
            foreach (var number in numbers)
            {
                var digitGroups = GetDigitGroupsFor(number);
                var isLinked = digitGroups.Any(digitGroup => seenDigitGroups.Contains(digitGroup));
                if (isLinked)
                {
                    if (++linkedCount > maxLinkedCount)
                    {
                        return true;
                    }
                }
                seenDigitGroups.UnionWith(digitGroups);
            }

            return false;
        }

        private HashSet<string> GetDigitGroupsFor(int number)
        {
            var stringValue = number.ToString();
            var digitGroups = new HashSet<string>();
            for (var i = 0; i < stringValue.Length - 4; i++) digitGroups.Add(stringValue.Substring(i, 4));
            return digitGroups;
        }
    }
}