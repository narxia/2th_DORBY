using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NextUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand ShowHomeCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public MainViewModel()
        {
            ShowHomeCommand = new RelayCommand(_ => CurrentView = new Views.HomeView());
            ShowSettingsCommand = new RelayCommand(_ => CurrentView = new Views.SettingsView());
            CurrentView = new Views.HomeView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // 간단한 RelayCommand 구현
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        public RelayCommand(Action<object> execute) => _execute = execute;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute(parameter);
    }
}