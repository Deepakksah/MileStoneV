using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;

namespace VideoOS.Server.CommandHandler;

public class HardwareScanTaskCommandHandler : ICommandHandler
{
	private IServerEnvironment serverEnvironment;

	private ILogger Logger => serverEnvironment.Server.Logger;

	public HardwareScanTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		this.serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is HardwareScanTaskCommand cmd))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ProcessCommand(cmd, responseSink, callContextInfo);
	}

	private void ProcessCommand(HardwareScanTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		int percentComplete = 0;
		int num = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
		HardwareScanCommand hardwareScanCommand = new HardwareScanCommand();
		Credentials credentials = new Credentials();
		if (!string.IsNullOrEmpty(cmd.UserName.Trim()))
		{
			credentials.Items.Add(cmd.UserName, cmd.Password);
		}
		AddressDriverPairs addressDriverPairs;
		if (!cmd.AddressDriverPairs.Items.Any())
		{
			addressDriverPairs = new AddressDriverPairs();
			AddressDriverPair addressDriverPair = new AddressDriverPair();
			bool flag = cmd.Address.Scheme == Uri.UriSchemeHttps;
			addressDriverPair.Address = cmd.Address.Host;
			addressDriverPair.DriverNumber = -1;
			addressDriverPair.Port = (ushort)cmd.Address.Port;
			addressDriverPair.UseSecurePort = flag;
			addressDriverPair.SecurePort = (ushort)(flag ? addressDriverPair.Port : 0);
			addressDriverPairs.Items.Add(addressDriverPair);
		}
		else
		{
			addressDriverPairs = cmd.AddressDriverPairs;
		}
		hardwareScanCommand.StartManualScanSession(cmd.RecordingServerId, credentials, cmd.UseDefaultCredentials, cmd.PrioritizedDrivers, cmd.SelectedDrivers, addressDriverPairs);
		DiscoveryCollection discoveryCollection = new DiscoveryCollection();
		IResponseCollector responseCollector = serverEnvironment.LocalCommandProcessor.ProcessCommandAsync(hardwareScanCommand, callContextInfo);
		ResponseBase responseBase2;
		while (true)
		{
			if (cmd.Stopping)
			{
				ResponseBase responseBase = CancelScan(hardwareScanCommand, callContextInfo);
				if (responseBase is TaskFailedResponse && discoveryCollection.Count == 0)
				{
					responseSink.AddResponse(responseBase);
					return;
				}
				if (discoveryCollection.Count > 0)
				{
					responseSink.AddResponse(GetTaskSuccessResponse(discoveryCollection));
					return;
				}
				TaskError taskError = new TaskError();
				taskError.ErrorCode = 60638;
				taskError.ErrorText = VmoResources.VMO60638;
				responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
				return;
			}
			try
			{
				responseBase2 = responseCollector.GetNextResponse(TimeSpan.FromSeconds(105.0));
			}
			catch (TimeoutException)
			{
				responseBase2 = null;
			}
			Logger.LogInformation("Scan hardware task - response received. Response: {Response}", (responseBase2 == null) ? "<NULL>" : responseBase2.GetType().Name);
			if (!(responseBase2 is HardwareScanProgressResponse hardwareScanProgressResponse))
			{
				break;
			}
			ScanResult scanResult = ScanResult.FromXml(hardwareScanProgressResponse.ScanResultXml);
			if (scanResult.DiscoveredHardware != null && scanResult.DiscoveredHardware.Count > 0)
			{
				foreach (Discovery item in scanResult.DiscoveredHardware)
				{
					discoveryCollection.Add(item);
				}
			}
			if (scanResult.Completed)
			{
				if (discoveryCollection.Count > 0)
				{
					responseSink.AddResponse(GetTaskSuccessResponse(discoveryCollection));
					return;
				}
				TaskError taskError2 = new TaskError();
				taskError2.ErrorCode = 60638;
				taskError2.ErrorText = VmoResources.VMO60638;
				responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2));
				return;
			}
			percentComplete = Math.Min(99, scanResult.Progress);
			if (percentComplete > num)
			{
				num = percentComplete;
				responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
			}
		}
		responseSink.AddResponse(GetTaskFailedResponse(responseBase2));
	}

	private ResponseBase CancelScan(HardwareScanCommand hardwareScanCommand, ICallContextInfo callContextInfo)
	{
		HardwareScanCommand hardwareScanCommand2 = new HardwareScanCommand();
		hardwareScanCommand2.CancelScanSession(hardwareScanCommand.RecorderId, hardwareScanCommand.SessionId);
		IResponseCollector responseCollector = serverEnvironment.LocalCommandProcessor.ProcessCommandAsync(hardwareScanCommand2, callContextInfo);
		ResponseBase responseBase;
		try
		{
			responseBase = responseCollector.GetNextResponse(TimeSpan.FromSeconds(105.0));
		}
		catch (TimeoutException)
		{
			responseBase = null;
		}
		if (responseBase is SuccessResponse || responseBase is ErrorResponse)
		{
			return new SuccessResponse();
		}
		return GetTaskFailedResponse(responseBase);
	}

	private static TaskSuccessResponse GetTaskSuccessResponse(DiscoveryCollection detectedHardware)
	{
		HardwareScanTaskResult hardwareScanTaskResult = new HardwareScanTaskResult();
		hardwareScanTaskResult.HardwareDriverId = detectedHardware[0].DriverNumber;
		hardwareScanTaskResult.UserName = detectedHardware[0].UserName;
		hardwareScanTaskResult.Password = detectedHardware[0].Password;
		hardwareScanTaskResult.PasswordEncrypted = false;
		hardwareScanTaskResult.Address = detectedHardware[0].Address;
		hardwareScanTaskResult.Port = detectedHardware[0].Port;
		hardwareScanTaskResult.MacAddress = detectedHardware[0].MacAddress;
		hardwareScanTaskResult.MacExistsGlobal = detectedHardware[0].MacExistsGlobal;
		hardwareScanTaskResult.MacExistsLocal = detectedHardware[0].MacExistsLocal;
		hardwareScanTaskResult.Validated = detectedHardware[0].Validated;
		hardwareScanTaskResult.DiscoveryError = detectedHardware[0].OutputParameters?.Error?.ToString() ?? string.Empty;
		return new TaskSuccessResponse(hardwareScanTaskResult.ToXml());
	}

	private TaskFailedResponse GetTaskFailedResponse(ResponseBase response)
	{
		if (response == null)
		{
			TaskError taskError = new TaskError();
			taskError.ErrorCode = 60634;
			taskError.ErrorText = VmoResources.VMO60634;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is InitializeDiscoveryComponentFailedResponse)
		{
			TaskError taskError2 = new TaskError();
			taskError2.ErrorCode = 60630;
			taskError2.ErrorText = VmoResources.VMO60630;
			return new TaskFailedResponse(isFatal: true, taskError2.ErrorText, taskError2);
		}
		if (response is StartSessionFailedResponse)
		{
			TaskError taskError3 = new TaskError();
			taskError3.ErrorCode = 60630;
			taskError3.ErrorText = VmoResources.VMO60630;
			return new TaskFailedResponse(isFatal: true, taskError3.ErrorText, taskError3);
		}
		if (response is DiscoveryComponentSecureScanUnsupportedResponse)
		{
			TaskError taskError4 = new TaskError();
			taskError4.ErrorCode = 60630;
			taskError4.ErrorText = VmoResources.VMO60630;
			return new TaskFailedResponse(isFatal: true, taskError4.ErrorText, taskError4);
		}
		if (response is RegisterSessionFailedResponse)
		{
			TaskError taskError5 = new TaskError();
			taskError5.ErrorCode = 60631;
			taskError5.ErrorText = VmoResources.VMO60631;
			return new TaskFailedResponse(isFatal: true, taskError5.ErrorText, taskError5);
		}
		if (response is DiscoveryComponentFailedResponse)
		{
			TaskError taskError6 = new TaskError();
			taskError6.ErrorCode = 60631;
			taskError6.ErrorText = VmoResources.VMO60631;
			return new TaskFailedResponse(isFatal: true, taskError6.ErrorText, taskError6);
		}
		if (response is RecorderUnavailableResponse)
		{
			TaskError taskError7 = new TaskError();
			taskError7.ErrorCode = 60635;
			taskError7.ErrorText = VmoResources.VMO60633;
			return new TaskFailedResponse(isFatal: true, taskError7.ErrorText, taskError7);
		}
		if (response is RecorderNotSupportedResponse)
		{
			TaskError taskError8 = new TaskError();
			taskError8.ErrorCode = 60636;
			taskError8.ErrorText = VmoResources.VMO60636;
			return new TaskFailedResponse(isFatal: true, taskError8.ErrorText, taskError8);
		}
		if (response is TimeoutResponse)
		{
			TaskError taskError9 = new TaskError();
			taskError9.ErrorCode = 60634;
			taskError9.ErrorText = VmoResources.VMO60634;
			return new TaskFailedResponse(isFatal: true, taskError9.ErrorText, taskError9);
		}
		if (response is FailResponse)
		{
			TaskError taskError10 = new TaskError();
			taskError10.ErrorCode = 60254;
			taskError10.ErrorText = VmoResources.VMO60254;
			return new TaskFailedResponse(isFatal: true, taskError10.ErrorText, taskError10);
		}
		if (response is ExceptionResponse exceptionResponse)
		{
			if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
			{
				Logger.LogWarning("Failed to scan for hardware during 'Hardware scan'. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
			}
			else
			{
				Logger.LogWarning("Failed to scan for hardware during 'Hardware scan'. [Error: {Exception}]", exceptionResponse.Exception);
			}
			TaskError taskError11 = new TaskError();
			taskError11.ErrorCode = 60637;
			taskError11.ErrorText = VmoResources.VMO60637;
			return new TaskFailedResponse(isFatal: true, taskError11.ErrorText, taskError11);
		}
		Logger.LogWarning("Failed to scan for hardware during 'Hardware scan'. Response: {Response}", (response == null) ? "<NULL>" : response.ToString());
		TaskError taskError12 = new TaskError();
		taskError12.ErrorCode = 60637;
		taskError12.ErrorText = VmoResources.VMO60637;
		return new TaskFailedResponse(isFatal: true, taskError12.ErrorText, taskError12);
	}
}
