using System.Collections.Generic;
using System.Linq;

namespace TFNValidate.Services
{
    public class LinkedNumberChecker
    {
        public static bool AreLinkedNumbersOverThreshold(int firstNumber, int[] otherNumbers, int maxLinkedCount)
        {
            var linkedCount = 0;
            var linkedDigitGroups = GetDigitGroupsFor(firstNumber);
            var foundNewLinkedValue = false;
            var unlinkedNumbers = new List<int>(otherNumbers);
            do
            {
                foundNewLinkedValue = false;
                var numbersToCheck = unlinkedNumbers.ToArray();
                foreach (var numberToCheck in numbersToCheck)
                {
                    var digitGroups = GetDigitGroupsFor(numberToCheck);
                    var isLinked = digitGroups.Any(digitGroup => linkedDigitGroups.Contains(digitGroup));
                    if (isLinked)
                    {
                        foundNewLinkedValue = true;
                        linkedCount++;
                        if (linkedCount == maxLinkedCount)
                        {
                            return true;
                        }
                        unlinkedNumbers.Remove(numberToCheck);
                        linkedDigitGroups.UnionWith(digitGroups);
                    }
                }
            } while (foundNewLinkedValue && unlinkedNumbers.Count > 0);
            return false;
        }

        private static HashSet<string> GetDigitGroupsFor(int number)
        {
            var stringValue = number.ToString();
            var digitGroups = new HashSet<string>();
            for (var i = 0; i < stringValue.Length - 4; i++)
            {
                digitGroups.Add(stringValue.Substring(i, 4));
            }
            return digitGroups;
        }
    }
}
