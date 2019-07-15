using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Neurotoxin.ScOut.Data.Attributes;

namespace Neurotoxin.ScOut.Data.Extensions
{
    public static class EnumExtensions
    {
        public static IReadOnlyCollection<TAttribute> GetCustomAttributes<TAttribute>(this string enumValueString, Type enumType) where TAttribute : Attribute
        {
            var memberInfo = enumType.GetMember(enumValueString).SingleOrDefault();

            if (memberInfo == null)
                return new List<TAttribute>();

            return memberInfo
                .GetCustomAttributes(typeof (TAttribute), false)
                .Select(attribute => (TAttribute) attribute)
                .ToList();
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to an equivalent enumerated object based on the <see cref="DisplayTextAttribute"/>s applied to the enumeration constants. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value"/>.</typeparam>
        /// <param name="value">The text to convert based on the <see cref="DisplayTextAttribute"/>s applied to the enumeration constants.</param>
        /// <param name="result">When this method returns, contains an object of type TEnum whose value is represented by <paramref name="value"/>. This parameter is passed uninitialized.</param>
        /// <returns>true if <paramref name="value"/> was converted successfully; otherwise, false.</returns>
        public static bool TryGetEnumValue<TEnum>(this string value, out TEnum result) where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("The type parameter is not an Enum type!");

            // [TEnum].[Unknown] or (as an exception) [SourceControlType].[TFS]
            result = default(TEnum);

            if (String.IsNullOrEmpty(value))
                return true;

            var enumNameValuePairs = GetEnumNameValuePairs<TEnum>();
            var hasNone = enumNameValuePairs.ContainsKey("None");
            var hasNotAvailable = enumNameValuePairs.ContainsKey("NotAvailable");

            var hasFlagsAttribute = enumType.GetCustomAttributes<FlagsAttribute>().Any();
            var members = enumType.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            if (hasNone && value.ToLowerInvariant().Contains("none"))
            {
                result = (TEnum) Enum.Parse(enumType, "None");
                return true;
            }

            if (hasNotAvailable && value.ToLowerInvariant().Contains("n/a"))
            {
                result = (TEnum) Enum.Parse(enumType, "NotAvailable");
                return true;
            }

            if (hasFlagsAttribute)
            {
                var enumUnderlyingValue = 0;

                foreach (var member in members)
                {
                    var attributes = member
                        .GetCustomAttributes<DisplayTextAttribute>()
                        .ToList();

                    var displayTexts = attributes
                        .SelectMany(a => a.DisplayText)
                        .ToList();

                    if (!displayTexts.Any() || attributes.Any(a => a.Option == DisplayTextOption.IncludeMemberName))
                        displayTexts.Add(member.Name);

                    if (displayTexts.Any(t => new Regex(t, RegexOptions.IgnoreCase).IsMatch(value)))
                    {
                        enumUnderlyingValue |= enumNameValuePairs[member.Name];
                    }
                }

                result = (TEnum) Enum.Parse(enumType, enumUnderlyingValue.ToString(CultureInfo.InvariantCulture));
                return true;
            }
            else
            {
                var member = members.SingleOrDefault(m => m.Name
                    .GetCustomAttributes<DisplayTextAttribute>(enumType)
                    .SingleOrDefault(a => a.DisplayText.Contains(value)) != null);

                if (member == null)
                    return false;

                result = (TEnum) Enum.Parse(enumType, member.Name);
                return true;
            }
        }

        private static IReadOnlyDictionary<string, int> GetEnumNameValuePairs<TEnum>() where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("The type parameter is not an Enum type!");

            var result = new Dictionary<string, int>();

            foreach (var value in Enum.GetValues(enumType))
            {
                var key = Enum.GetName(enumType, value);

                if (String.IsNullOrEmpty(key))
                    continue;

                result[key] = (int)value;
            }

            return result;
        }
    }
}
