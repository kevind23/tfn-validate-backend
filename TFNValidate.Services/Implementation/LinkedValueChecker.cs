using System.Collections.Generic;
using System.Linq;

namespace TFNValidate.Services.Implementation
{
    public class LinkedValueChecker : ILinkedValueChecker
    {
        public bool AreLinkedValuesOverThreshold(string[] values, int maxLinkedCount)
        {
            var seenSubstrings = new HashSet<string>();
            var linkedCount = 1;
            foreach (var value in values)
            {
                var linkingSubstrings = GetLinkingSubstringsFor(value);
                var isLinked = linkingSubstrings.Any(substring => seenSubstrings.Contains(substring));
                if (isLinked)
                {
                    if (++linkedCount > maxLinkedCount)
                    {
                        return true;
                    }
                }
                seenSubstrings.UnionWith(linkingSubstrings);
            }

            return false;
        }

        private HashSet<string> GetLinkingSubstringsFor(string value)
        {
            var linkingSubstrings = new HashSet<string>();
            for (var i = 0; i < value.Length - 4; i++) linkingSubstrings.Add(value.Substring(i, 4));
            return linkingSubstrings;
        }
    }
}