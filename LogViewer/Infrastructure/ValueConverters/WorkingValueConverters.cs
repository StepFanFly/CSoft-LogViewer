using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;

namespace LogViewer.ValueConverters
{
    public class FilterOperationValueConverter : BaseValueConverter<FilterOperationValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<eOperationType> operTypes) 
            {
                List<string> strItems = new List<string>();

                foreach (var curType in operTypes) 
                {
                    if (curType == eOperationType.eUnion)
                        strItems.Add(new string("U"));

                    if (curType == eOperationType.eIntersect)
                        strItems.Add(new string("ꓵ"));

                    if (curType == eOperationType.eShielding)
                        strItems.Add(new string("\\"));
                } 

                return strItems;
            }
            else if (value is eOperationType operType) 
            {

                if (eOperationType.eIntersect == operType)
                    return "ꓵ";
                else if (eOperationType.eShielding == operType)
                    return "\\";
                else if (eOperationType.eUnion == operType)
                    return "U";

                return string.Empty;

            }

            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
                return null;

            string strVal = value as string;
            switch (strVal)
            {
                case "U": return eOperationType.eUnion;
                case "ꓵ": return eOperationType.eIntersect;
                case "\\": return eOperationType.eShielding;
                default:
                    return eOperationType.eNone;
            }

        }
    }

    public class BooleanToVisibilityValueConverter : BaseValueConverter<BooleanToVisibilityValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bIsVisible)
            {
                return bIsVisible ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    //public class FilterValueConverter : BaseMultiValueConverter<FilterValueConverter>
    //{
    //    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (values[0] is Filter outerFilter  && values[1] is Filter innerFilter)
    //        {
    //            if (innerFilter.Type != outerFilter.Type) 
    //            {
    //                outerFilter = innerFilter;
    //                return innerFilter;
    //            }

    //        }
    //       return values[1];
    //    }

    //    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    //    {
    //        return null;
    //    }
    //}
}
