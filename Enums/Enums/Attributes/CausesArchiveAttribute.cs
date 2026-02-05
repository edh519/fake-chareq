using System;

namespace Enums.Enums.Attributes
{
    public class CausesArchiveAttribute : Attribute
    {
        public bool Value { get; private set; }

        public CausesArchiveAttribute(bool value)
        {
            Value = value;
        }
    }
}
