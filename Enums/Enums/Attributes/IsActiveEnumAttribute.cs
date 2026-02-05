using System;

namespace Enums.Enums.Attributes
{
    public class IsActiveAttribute : Attribute
    {
        public bool Value { get; private set; }

        public IsActiveAttribute(bool value)
        {
            Value = value;
        }
    }
}
