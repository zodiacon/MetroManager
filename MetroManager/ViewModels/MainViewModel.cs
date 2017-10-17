using System.Collections.Generic;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using System.Windows.Input;
using System.Windows.Data;
using System.Linq;

namespace MetroManager.ViewModels {
    class MainViewModel : BindableBase {
        IEnumerable<Package> _packages;
        public IEnumerable<Package> Packages => _packages ?? (_packages = new PackageManager().FindPackagesForUser(string.Empty).ToArray());

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
            set { SetProperty(ref _selectedPackage, value); }
        }


        public ICommand RefreshCommand => new DelegateCommand(() => OnPropertyChanged(nameof(Packages)));

        private string _searchText;

        public string SearchText {
            get => _searchText;
            set {
                if (SetProperty(ref _searchText, value)) {
                    var view = CollectionViewSource.GetDefaultView(Packages);
                    if (string.IsNullOrEmpty(value))
                        view.Filter = null;
                    else {
                        var text = value.ToLower();
                        view.Filter = obj => {
                            var package = (Package)obj;
                            return package.Id.Name.ToLower().Contains(text);
                        };
                    }
                }
            }
        }

    }
}
