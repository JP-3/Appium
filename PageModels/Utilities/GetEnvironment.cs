using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.Utilities
{
    public class GetEnvironment
    {
        public GetEnvironment(string setVariable, string defaultVariable)
        {
            Environment = System.Environment.GetEnvironmentVariable(setVariable) ?? defaultVariable;

            if (IsBadString(defaultVariable) ||
                IsBadString(Environment))
            {
                throw new ArgumentException($"{Environment}, {defaultVariable} - variables are null : check Appium.SetConfig.GetEnvironment");
            }
        }

        internal string Environment { get; set; }

        public static GetEnvironment Set(string setVariable, string defalutEnvironment) =>
                  new GetEnvironment(setVariable, defalutEnvironment);

        private bool IsBadString(string check) =>
            string.IsNullOrEmpty(check) ||
                (check.Length > 0 && char.IsHighSurrogate(check[check.Length - 1]));
    }
}
