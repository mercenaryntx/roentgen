using System;

namespace Neurotoxin.ScOut.Data.Attributes
{
    public class IconAttribute : Attribute
    {
        public string ResourceName { get; private set; }

        public IconAttribute(string resourceName)
        {
            ResourceName = resourceName;
        }
    }
}