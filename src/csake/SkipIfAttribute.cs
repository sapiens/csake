using System;

namespace CSake
{
    public class SkipIfAttribute:Attribute
    {
        public bool Always { get; set; }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }

        public SkipIfAttribute(bool always)
        {
            Always = always;
        }

        public SkipIfAttribute(string fieldName,object fieldValue)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}