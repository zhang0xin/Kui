using System.Text.RegularExpressions;

namespace Kui.Core.Convert
{
    public class TextAttribute : FieldAttribute
    {
        public int MaxLength {get; set;} = 0; 
        public int MinLength {get; set;} = int.MaxValue;

        protected override bool Validate(string value)
        {
            var pattern = $@"^\w{{{MinLength}, {MaxLength}}}$";
            return ValidateByRegex(value, pattern);
        }
    }
}