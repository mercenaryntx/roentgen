using System;

namespace Neurotoxin.ScOut.Data.Attributes
{
    public enum DisplayTextOption { IncludeMemberName, SkipMemberName }

    public class DisplayTextAttribute : Attribute
    {
        public string[] DisplayText { get; private set; }
        public DisplayTextOption Option { get; private set; }

        public DisplayTextAttribute(params string[] displayText)
        {
            DisplayText = displayText;
            Option = DisplayTextOption.IncludeMemberName;
        }

        public DisplayTextAttribute(DisplayTextOption option, params string[] displayText)
        {
            DisplayText = displayText;
            Option = option;
        }
    }
}
