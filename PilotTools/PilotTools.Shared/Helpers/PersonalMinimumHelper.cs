using System;
using System.Collections.Generic;
using System.Text;

namespace PilotTools.Helpers
{
    public static class PersonalMinimumsHelper
    {
        public static PersonalMinimumVerificationResult IsOver(this int testValue, int limit)
        {
            var diff = testValue - limit;
            PersonalMinimumVerificationResult result = PersonalMinimumVerificationResult.Unknown;

            if(diff < 0)
            {
                result = PersonalMinimumVerificationResult.Fail;
            }
            else if (diff >= 0 && diff < limit / 10)
            {
                result = PersonalMinimumVerificationResult.Warning;
            }
            else
            {
                result = PersonalMinimumVerificationResult.Pass;
            }

            return result;
        }
    }
}
