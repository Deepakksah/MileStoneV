using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using VideoOS.Installer.CommandLine;
using VideoOS.Installer.Setup.Properties;

namespace VideoOS.Installer.Setup;

public partial class MainWindow : Window
{
	private const int GWL_STYLE = -16;

	private const int WS_SYSMENU = 524288;

	public MainWindow(string componentName)
	{
		Uri resourceLocator = new Uri("/" + componentName + ";component/mainwindow.xaml", UriKind.Relative);
		Application.LoadComponent(this, resourceLocator);
		base.Loaded += MainWindow_Loaded;
	}

	public MainWindow()
	{
		InitializeComponent();
		base.Loaded += MainWindow_Loaded;
	}

	private void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		IntPtr handle = new WindowInteropHelper(this).Handle;
		NativeMethods.SetWindowLong(handle, -16, NativeMethods.GetWindowLong(handle, -16) & -524289);
	}

	public async Task<int> StartProcess(SetupViewModel setupViewModel, InstallerOptions options, string[] args)
	{
		Execute execute = new Execute(options, setupViewModel.ProgressBar, setupViewModel.StatusProgress);
		string text = await Task.Run(() => execute.ValidateAndExtractExternalZipFile());
		if (text != null)
		{
			MessageBox.Show(text, VideoOS.Installer.Setup.Properties.Resources.ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Hand);
			return 1;
		}
		Process process = await execute.StartInstaller(args, withUi: true);
		Hide();
		return await Task.Run(() => execute.Installation(process));
	}
}
