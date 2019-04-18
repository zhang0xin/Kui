
using System;
using System.Text.RegularExpressions;

namespace Kui.Core.Convert
{
    public abstract class FieldAttribute : TypeAttribute
    {
        protected abstract bool Validate(string value);
        protected bool ValidateByRegex(string value, string pattern)
        {
            return Regex.IsMatch(value, pattern);
        }
    }
}