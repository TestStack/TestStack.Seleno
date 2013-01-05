using System;
using System.ComponentModel;

namespace TestStack.Seleno.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Generic method to convert from one type to another and returns the strongly typed value. 
        /// Returns default value for type if cannot convert.
        /// </summary>
        public static TReturn TryConvertTo<TFrom, TReturn>(this TFrom from, TReturn defaultValue = default(TReturn))
        {
            try
            {
                return (TReturn)from.TryConvertTo(typeof(TReturn), defaultValue);
            }
            catch
            {
                
            }

            return defaultValue;
        }

        /// <summary>
        /// Converts from one type to another and returns the value. Returns default value for type if cannot convert.
        /// </summary>
        public static object TryConvertTo<TFrom>(this TFrom from, Type returnType, object defaultValue = null)
        {
            try
            {
                var underlyingType = Nullable.GetUnderlyingType(returnType) ?? returnType;

                if (from == null)
                    return defaultValue;

                if ((from as string) != null)
                {
                    string value = from as string;

                    if (underlyingType.IsEnum)
                        return Enum.Parse(underlyingType, value, true);

                    if (string.IsNullOrEmpty(value))
                        return defaultValue;
                }

                if ((from as IConvertible) != null)
                    return Convert.ChangeType(from, underlyingType);

                if (returnType.IsAssignableFrom(from.GetType()))
                    return from;

                TypeConverter converter = TypeDescriptor.GetConverter(from.GetType());

                if (converter.CanConvertTo(returnType))
                    return converter.ConvertTo(from, returnType);

                if ((from as string) != null)
                    return from.ToString().TryConvertTo<string>(returnType, defaultValue);
            }
            catch
            {
            }

            return defaultValue;
        }

    }
}
