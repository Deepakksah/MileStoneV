using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using VideoOS.Installer.CommandLine;
using VideoOS.Installer.Setup.Properties;

namespace VideoOS.Installer.Setup;

internal static class Program
{
	private static readonly Version _minimumRequiredDotNetFrameworkVersion = new Version(4, 7, 2, 0);

	private static InstallerOptions _options;

	private static SetupViewModel _setupViewModel;

	[STAThread]
	public static void Main(string[] args)
	{
		bool flag = false;
		try
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += ExceptionHandler;
			currentDomain.AssemblyResolve += (object sender, ResolveEventArgs eventArgs) => (new AssemblyName(eventArgs.Name).Name == typeof(Program).Namespace) ? Assembly.GetExecutingAssembly() : null;
			_options = CommandLineInterpreter.GetOptions(args);
			flag = _options != null && !_options.IsHelp && !_options.IsQuiet;
			if (!flag)
			{
				NativeMethods.AttachConsole();
				if (AnotherInstanceIsRunning())
				{
					Console.WriteLine(Resources.ErrorAnotherInstanceRunning);
					Environment.ExitCode = 1;
					return;
				}
			}
			else if (AnotherInstanceIsRunning())
			{
				if (!FocusExistingInstance())
				{
					MessageBox.Show(Resources.ErrorAnotherInstanceRunning, Resources.ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Hand);
				}
				Environment.ExitCode = 1;
				return;
			}
			if (_options == null)
			{
				Console.WriteLine("Invalid arguments");
				new InstallerOptions().GetUsage();
				Environment.ExitCode = 100;
				return;
			}
			if (_options.IsHelp)
			{
				_options.GetUsage();
				return;
			}
			new SingleAssemblyResourceManager(typeof(Resources).FullName, typeof(Resources).Assembly).AttachToDesignerClass(typeof(Resources));
			SetLanguage(_options.LanguageId);
			if (!CheckDotNetVersion())
			{
				Environment.ExitCode = 1;
				return;
			}
			_setupViewModel = new SetupViewModel();
			if (flag)
			{
				App app = new App(_options, args, _setupViewModel);
				app.InitializeComponent();
				Environment.ExitCode = app.Run();
				return;
			}
			Execute execute = new Execute(_options, _setupViewModel.ProgressBar, _setupViewModel.StatusProgress);
			if (_options.ShowConsole)
			{
				_setupViewModel.PropertyChanged += WriteStatusToConsole;
			}
			string text = execute.ValidateAndExtractExternalZipFile();
			if (text != null)
			{
				Console.WriteLine(FormattableString.Invariant($"{Resources.ErrorOccured}: {text}"));
				Environment.ExitCode = 1;
			}
			else
			{
				Process result = execute.StartInstaller(args, withUi: false).Result;
				Environment.ExitCode = execute.Installation(result);
			}
		}
		finally
		{
			if (!flag)
			{
				IntPtr consoleWindow = NativeMethods.GetConsoleWindow();
				if (consoleWindow != IntPtr.Zero)
				{
					NativeMethods.PostMessage(consoleWindow, 256u, (IntPtr)13L, IntPtr.Zero);
					NativeMethods.PostMessage(consoleWindow, 257u, (IntPtr)13L, IntPtr.Zero);
					NativeMethods.FreeConsole();
				}
			}
		}
	}

	private static bool CheckDotNetVersion()
	{
		if (GetDotNetFrameworkVersion() < _minimumRequiredDotNetFrameworkVersion)
		{
			string friendlyName = AppDomain.CurrentDomain.FriendlyName;
			if (_options.IsQuiet)
			{
				Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Resources.ErrorPrereq, friendlyName));
			}
			else
			{
				MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Resources.ErrorPrereq, friendlyName), Resources.ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			return false;
		}
		return true;
	}

	private static void SetLanguage(string languageId)
	{
		if (languageId != null)
		{
			try
			{
				CultureInfo currentUICulture = new CultureInfo(languageId);
				Thread.CurrentThread.CurrentUICulture = currentUICulture;
			}
			catch (CultureNotFoundException)
			{
			}
		}
	}

	private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs e)
	{
		Exception ex = (Exception)e.ExceptionObject;
		string text = string.Format(CultureInfo.CurrentCulture, Resources.ErrorUnexpected, ex.ToString());
		if (_options == null || _options.IsQuiet)
		{
			Console.WriteLine(text);
		}
		else
		{
			MessageBox.Show(text, Resources.ErrorOccured, MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		AssemblyName assemblyName = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
		EventLog.WriteEntry(FormattableString.Invariant($"{assemblyName.Name}, {assemblyName.Version}"), ex.ToString(), EventLogEntryType.Error);
		Environment.Exit(1);
	}

	private static void WriteStatusToConsole(object sender, PropertyChangedEventArgs e)
	{
		string propertyName = e.PropertyName;
		if (!(propertyName == "ProgressBarProgress"))
		{
			if (propertyName == "StatusProgressState")
			{
				Console.WriteLine();
				Console.WriteLine(_setupViewModel.StatusProgressState);
			}
		}
		else
		{
			Console.Write(FormattableString.Invariant($"\r{_setupViewModel.ProgressBarProgress} %"));
		}
	}

	private static Version GetDotNetFrameworkVersion()
	{
		if (Version.TryParse(((AssemblyFileVersionAttribute)typeof(object).GetTypeInfo().Assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute))).Version, out var result))
		{
			return result;
		}
		return new Version();
	}

	private static bool AnotherInstanceIsRunning()
	{
		return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
	}

	private static bool FocusExistingInstance()
	{
		Process currentProcess = Process.GetCurrentProcess();
		if (BringMainWindowToFront(Process.GetProcessesByName(currentProcess.ProcessName).FirstOrDefault((Process x) => x.Id != currentProcess.Id)))
		{
			return true;
		}
		return false;
	}

	private static bool BringMainWindowToFront(Process process)
	{
		if (process != null)
		{
			NativeMethods.Windowplacement lpwndpl = default(NativeMethods.Windowplacement);
			NativeMethods.GetWindowPlacement(process.MainWindowHandle, ref lpwndpl);
			if (lpwndpl.showCmd == 2)
			{
				NativeMethods.ShowWindow(process.MainWindowHandle, NativeMethods.ShowWindowEnum.Restore);
			}
			return NativeMethods.SetForegroundWindow(process.MainWindowHandle);
		}
		return false;
	}
}
