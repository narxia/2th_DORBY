using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NextUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeShowCommand { get; }
        public ICommand IOShowCommand { get; }

        public MainViewModel()
        {
            HomeShowCommand = new RelayCommand(_ => CurrentView = new Views.HomeView());
            IOShowCommand = new RelayCommand(_ => CurrentView = new Views.Machine.IOView());
            CurrentView = new Views.HomeView();
        }
    }


}