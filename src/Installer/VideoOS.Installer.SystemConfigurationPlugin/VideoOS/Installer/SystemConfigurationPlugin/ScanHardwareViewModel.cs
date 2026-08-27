using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ScanHardwareViewModel : ProgressViewModel, IDisposable
{
	private string _foundCamerasXml;

	private string _scanErrorNumber;

	private int _port = 80;

	private CancellationTokenSource _cancelTokenSource;

	private ICommand _stopCommand;

	private bool _stopping;

	private bool _operationCanBeStopped;

	private bool disposedValue;

	public string FoundCamerasXml => _foundCamerasXml;

	public ICommand StopCommand
	{
		get
		{
			return _stopCommand;
		}
		set
		{
			_stopCommand = value;
			OnPropertyChanged("StopCommand");
		}
	}

	public bool Stopping
	{
		get
		{
			return _stopping;
		}
		set
		{
			_stopping = value;
			OnPropertyChanged("Stopping");
		}
	}

	public bool OperationCanBeStopped
	{
		get
		{
			return _operationCanBeStopped;
		}
		set
		{
			_operationCanBeStopped = value;
			OnPropertyChanged("OperationCanBeStopped");
		}
	}

	public string ScanErrorNumber => _scanErrorNumber;

	public ScanHardwareViewModel()
	{
		_foundCamerasXml = string.Empty;
		_cancelTokenSource = new CancellationTokenSource();
		StopCommand = new DelegateCommand(delegate
		{
			StopScan();
		});
		InitServerPort();
	}

	private void InitServerPort()
	{
		if (GlobalData.TryGetValue(ServerParameters.ServerPort.ToString(), out var value) && !int.TryParse(value, out _port))
		{
			_port = 80;
		}
	}

	public void InitStartHardwareScan(object sender, DoWorkEventArgs e)
	{
		ObservableCollection<IUserInformation> userInformation = new ObservableCollection<IUserInformation>();
		bool result = true;
		string value = "";
		if (GlobalData.TryGetValue(ServerParameters.UseDefaultCameraCredential.ToString(), out value) && !bool.TryParse(value, out result))
		{
			result = true;
		}
		string value2 = "";
		if (GlobalData.TryGetValue(ServerParameters.ScanDataUserNamePassword.ToString(), out value2))
		{
			userInformation = CameraUserNamePasswordWizard.GetUserInfoFromXml(value2);
		}
		bool result2 = false;
		if (GlobalData.TryGetValue(ServerParameters.UseHttps.ToString(), out var value3) && !bool.TryParse(value3, out result2))
		{
			result2 = false;
		}
		StartHardwareScan(result, userInformation, result2);
	}

	public void StartHardwareScan(bool useDefaultCredentials, ObservableCollection<IUserInformation> userInformation, bool useHttpsScan)
	{
		if (userInformation == null)
		{
			throw new ArgumentNullException("userInformation");
		}
		InitServerPort();
		ScanResults scanResults = new ScanResults();
		using (ConfigurationApiServiceManager serviceManager = new ConfigurationApiServiceManager(_port))
		{
			RecorderOnlinePlugin recorderOnlinePlugin = new RecorderOnlinePlugin(_port);
			try
			{
				recorderOnlinePlugin.ProgressChanged += RecorderPlugin_ProgressChanged;
				bool flag = recorderOnlinePlugin.RecorderOnline();
				LogManager.Write("Recorder online: " + flag);
				recorderOnlinePlugin.ProgressChanged -= RecorderPlugin_ProgressChanged;
				recorderOnlinePlugin = null;
				if (flag)
				{
					LogManager.Write("Scanning Hardware started ");
					base.Header = Resources.HardwareScanInfo;
					HardwareScanPlugin hardwareScanPlugin = new HardwareScanPlugin(_port);
					Stopping = false;
					OperationCanBeStopped = true;
					_cancelTokenSource = new CancellationTokenSource();
					hardwareScanPlugin.ProgressChanged += ScanPlugin_ProgressChanged;
					scanResults = hardwareScanPlugin.DoHardwareScan(useDefaultCredentials, userInformation[0].UserName, userInformation[0].Password.ConvertToUnsecureString(), userInformation[1].UserName, userInformation[1].Password.ConvertToUnsecureString(), userInformation[2].UserName, userInformation[2].Password.ConvertToUnsecureString(), useHttpsScan, serviceManager, _cancelTokenSource.Token);
					LogManager.Write(FormattableString.Invariant($"Scanning Hardware ended, found {HardwareConfigurations.GetFromXml(scanResults.Result).Count} Cameras"));
					hardwareScanPlugin.ProgressChanged -= ScanPlugin_ProgressChanged;
					hardwareScanPlugin = null;
				}
				else
				{
					scanResults = new ScanResults(ScanErrorCode.Generic, "<CameraConfigurations></CameraConfigurations>");
					LogManager.Write("Scanning Hardware failed because recording server was not online ");
				}
				_foundCamerasXml = scanResults.Result;
				_scanErrorNumber = scanResults.ErrorCode.ToString();
			}
			catch (ArgumentNullException ex)
			{
				LogManager.Write("Scanning Hardware failed " + ex.Message);
				throw;
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
		SetProgress(100);
	}

	public void StopScan()
	{
		base.Header = Resources.HardwareScanStopping;
		Stopping = true;
		OperationCanBeStopped = false;
		_cancelTokenSource.Cancel();
	}

	private void RecorderPlugin_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetProgress(e.Progress);
		base.Header = e.Message;
	}

	private void ScanPlugin_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetProgress(e.Progress);
		base.Header = e.Message;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_cancelTokenSource.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
