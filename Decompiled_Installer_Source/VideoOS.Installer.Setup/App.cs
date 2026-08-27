using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using VideoOS.Installer.CommandLine;

namespace VideoOS.Installer.Setup;

public class App : Application
{
	private readonly InstallerOptions _options;

	private readonly string[] _args;

	private MainWindow _mainWindow;

	private SetupViewModel _viewModel;

	private int _exitCode;

	internal App()
	{
	}

	public App(InstallerOptions options, string[] args, SetupViewModel setupViewModel)
	{
		_options = options ?? throw new ArgumentNullException("options");
		_viewModel = setupViewModel ?? throw new ArgumentNullException("setupViewModel");
		_args = args;
	}

	public async void ApplicationStartup(object sender, StartupEventArgs e)
	{
		string name = Assembly.GetExecutingAssembly().GetName().Name;
		_mainWindow = new MainWindow(name)
		{
			DataContext = _viewModel
		};
		_mainWindow.Show();
		_exitCode = await _mainWindow.StartProcess(_viewModel, _options, _args);
		_mainWindow.Close();
	}

	protected override void OnExit(ExitEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		e.ApplicationExitCode = _exitCode;
		base.OnExit(e);
	}

	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		base.Startup += ApplicationStartup;
	}

	[STAThread]
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public static void Main()
	{
		App app = new App();
		app.InitializeComponent();
		app.Run();
	}
}
