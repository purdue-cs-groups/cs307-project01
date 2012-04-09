using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MetrocamPan.ScrollLoaders
{
    static class ImplicitTypeConverter
    {
        public static object ConvertToType(object value, Type type)
        {
            ArgumentValidator.AssertNotNull(type, "type");

            if (value == null || type.IsAssignableFrom(value.GetType()))
            {
                return value;
            }

            TypeConverter converter = GetTypeConverter(type);

            if (converter != null && converter.CanConvertFrom(value.GetType()))
            {
                value = converter.ConvertFrom(value);
                return value;
            }

            return null;
        }

        public static TypeConverter GetTypeConverter(Type type)
        {
            var attribute = (TypeConverterAttribute)Attribute.GetCustomAttribute(
                                type, typeof(TypeConverterAttribute), false);

            if (attribute != null)
            {
                try
                {
                    Type converterType = Type.GetType(attribute.ConverterTypeName, false);
                    if (converterType != null)
                    {
                        return Activator.CreateInstance(converterType) as TypeConverter;
                    }
                }
                catch (Exception)
                {
                    /* Suppress. */
                }
            }
            return new FromStringConverter(type);
        }
    }

    public class FromStringConverter : TypeConverter
    {
        readonly Type type;

        public FromStringConverter(Type type)
        {
            this.type = type;
        }

        public override bool CanConvertFrom(
            ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string)
                    || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                if (type == typeof(bool))
                {
                    return bool.Parse(stringValue);
                }

                if (type.IsEnum)
                {
                    return Enum.Parse(type, stringValue, false);
                }

                if (type == typeof(Guid))
                {
                    return new Guid(stringValue);
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.Append("<ContentControl xmlns='http://schemas.microsoft.com/client/2007' xmlns:c='"
                                     + ("clr-namespace:" + type.Namespace + ";assembly=" + type.Assembly.FullName.Split(new[] { ',' })[0]) + "'>\n");
                stringBuilder.Append("<c:" + type.Name + ">\n");
                stringBuilder.Append(stringValue);
                stringBuilder.Append("</c:" + type.Name + ">\n");
                stringBuilder.Append("</ContentControl>");
                var instance = XamlReader.Load(stringBuilder.ToString()) as ContentControl;
                if (instance != null)
                {
                    return instance.Content;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
