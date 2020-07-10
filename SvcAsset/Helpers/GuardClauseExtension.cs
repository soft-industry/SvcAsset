using Ardalis.GuardClauses;
using System;

namespace SvcAsset.Core.Helpers
{
    public static class GuardClauseExtension
    {
        public static void GuidNullOrEmpty(this IGuardClause guardClause, Guid input, string parameterName)
        {
            if (input == Guid.Empty)
            {
                throw new ArgumentException("Guid should not be empty", parameterName);
            }
        }
    }
}
