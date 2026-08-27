using System.Threading.Tasks;
using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public interface IAsyncCommand : ICommand
{
	Task ExecuteAsync(object parameter);
}
