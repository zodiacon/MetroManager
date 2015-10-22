using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace MetroManager.ViewModels {
	class MainViewModel : BindableBase {
		public IEnumerable<Package> Packages => new PackageManager().FindPackagesForUser(string.Empty);

		public DelegateCommand LaunchCommand { get; }


		public MainViewModel() {
			LaunchCommand = new DelegateCommand(
				() => {
					var item = ShellObject.FromParsingName(SelectedPackage.Id.FamilyName);
					var idprop = item.Properties.GetProperty(SystemProperties.System.AppUserModel.ID);
					var id = (string)idprop.ValueAsObject;
					MetroLauncher.LaunchApp(id);
			},
				() => SelectedPackage != null && !SelectedPackage.IsFramework);

		}

		private Package _selectedPackage;

		public Package SelectedPackage {
			get { return _selectedPackage; }
			set {
				if(SetProperty(ref _selectedPackage, value)) {
					LaunchCommand.RaiseCanExecuteChanged();
				}
			}
		}

	}
}
