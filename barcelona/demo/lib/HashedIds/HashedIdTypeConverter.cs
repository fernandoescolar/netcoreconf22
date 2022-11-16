using System.ComponentModel;
using System.Globalization;

namespace HashedIds;

public class HashedIdTypeConverter: TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if(sourceType == typeof(int))
        {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        return new HashedId((int)value);
    }
}