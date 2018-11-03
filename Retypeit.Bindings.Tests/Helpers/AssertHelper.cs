using System.Collections.Generic;
using System.Linq;
using Retypeit.Scripts.Bindings.Lexer;
using Xunit;
using Xunit.Sdk;

namespace Retypeit.Scripts.Bindings.Tests.Helpers
{
    internal static class AssertHelper
    {
        public static void Equal(HashSet<Token> source, params Token[] compareTo)
        {
            var sourceList = source.ToList();
            if (sourceList.Count != compareTo.Length)
                throw new EqualException("Token count " + source.Count, "Token count " + compareTo.Length);

            for (int i = 0; i < sourceList.Count; i++)
            {
                var sourceToken = sourceList[i];
                var compareToToken = compareTo[i];

                Assert.Equal(sourceToken.Type, compareToToken.Type);

                if (compareToToken.HasValue)
                    Assert.Equal(sourceToken.Value, compareToToken.Value);
            }
        }
    }
}