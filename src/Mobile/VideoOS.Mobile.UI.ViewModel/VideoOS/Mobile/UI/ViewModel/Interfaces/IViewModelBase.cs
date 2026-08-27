using System.ComponentModel;
using System.Windows.Input;

namespace VideoOS.Mobile.UI.ViewModel.Interfaces;

public interface IViewModelBase : INotifyPropertyChanged
{
	bool IsBusy { get; set; }

	void OnPropertyChangedAll();

	void RaiseCanExecute(ICommand command);

	void RaiseCanExecuteAll();
}
