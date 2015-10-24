using System.Collections.Generic;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Windows.ApplicationModel;
using Windows.Management.Deployment;

namespace MetroManager.ViewModels {
	class MainViewModel : BindableBase {
		public IEnumerable<Package> Packages => new PackageManager().FindPackagesForUser(string.Empty);

		public DelegateCommand LaunchCommand { get; }


		public MainViewModel() {
			LaunchCommand = new DelegateCommand(
				() => {
					try {
						MetroLauncher.LaunchApp(SelectedPackage.Id.FullName);
					}
					catch {
						MessageBox.Show("Error launching app", "Metro Launcher");
					}
				},
				() => SelectedPackage != null && !SelectedPackage.IsFramework)
				.ObservesProperty(() => SelectedPackage);

		}

		private Package _selectedPackage;

		public Package SelectedPackage {
			get { return _selectedPackage; }
			set { SetProperty(ref _selectedPackage, value);	}
		}

	}
}
