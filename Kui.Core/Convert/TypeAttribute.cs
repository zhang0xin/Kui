using System;
using System.Text.RegularExpressions;

namespace Kui.Core.Convert
{
    public abstract class TypeAttribute : Attribute
    {
        public string Label {get; set;}
    }
}