using System;
using System.Globalization;

namespace LogViewer.ValueConverters
{
    public class FilterValueConverter : BaseValueConverter<FilterValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if(value is Filter filter) 
            //{
            //    //var type = FilterFactory.GetTypeByName((string)parameter);
            //    //if (type != filter.Type)
            //        return FilterFactory.Create(filter.Type);
            //}

            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Filter filter)
            {
                //var type = FilterFactory.GetTypeByName((string)parameter);
                //if (type != filter.Type)
                return FilterFactory.Create(filter.Type);
            }

            return null;
            //return value;
        }

      
    }
}
