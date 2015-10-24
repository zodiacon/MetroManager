using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Windows.ApplicationModel;

namespace MetroManager.Converters {
	class VersionToStringConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var version = (PackageVersion)value;
			return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
