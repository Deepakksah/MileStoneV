using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.Properties;

namespace VideoOS.Server.CommandHandler;

public class HardwareScanExpressTaskCommandHandler : ICommandHandler
{
	private readonly IServerEnvironment serverEnvironment;

	private ILogger Logger => serverEnvironment.Server.Logger;

	public HardwareScanExpressTaskCommandHandler(IServerEnvironment serverEnvironment)
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
		if (!(command is HardwareScanExpressTaskCommand cmd))
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

	private void ProcessCommand(HardwareScanExpressTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		DateTime utcNow = DateTime.UtcNow;
		int percentComplete = 0;
		int num = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
		HardwareScanCommand hardwareScanCommand = new HardwareScanCommand();
		Credentials credentials = cmd.Credentials;
		hardwareScanCommand.StartExpressScanSession(cmd.RecordingServerId, credentials, cmd.UseDefaultCredentials, cmd.UseSecurePort, cmd.PrioritizedDrivers);
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
				TaskError taskError = new TaskError();
				taskError.ErrorCode = 60638;
				taskError.ErrorText = VmoResources.VMO60638;
				responseSink.AddResponse(new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError));
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
			TimeSpan timeout = TimeSpan.FromSeconds(5.0);
			responseBase = responseCollector.GetNextResponse(timeout);
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
		HardwareScanExpressTaskResult hardwareScanExpressTaskResult = new HardwareScanExpressTaskResult();
		List<HardwareScanResult> list = new List<HardwareScanResult>();
		foreach (Discovery item in detectedHardware)
		{
			HardwareScanResult hardwareScanResult = new HardwareScanResult();
			hardwareScanResult.HardwareDriverId = item.DriverNumber;
			hardwareScanResult.UserName = item.UserName;
			hardwareScanResult.Password = item.Password;
			hardwareScanResult.PasswordEncrypted = false;
			if (item.SecurePort != 0)
			{
				hardwareScanResult.Address = "https://" + item.Address;
				hardwareScanResult.Port = item.SecurePort;
			}
			else
			{
				hardwareScanResult.Address = item.Address;
				hardwareScanResult.Port = item.Port;
			}
			hardwareScanResult.MacAddress = item.MacAddress;
			hardwareScanResult.MacExistsGlobal = item.MacExistsGlobal;
			hardwareScanResult.MacExistsLocal = item.MacExistsLocal;
			hardwareScanResult.Validated = item.Validated;
			hardwareScanResult.DiscoveryError = item.OutputParameters?.Error?.ToString() ?? string.Empty;
			list.Add(hardwareScanResult);
		}
		hardwareScanExpressTaskResult.HardwareScanResults = list.ToArray();
		return new TaskSuccessResponse(hardwareScanExpressTaskResult.ToXml());
	}

	private TaskFailedResponse GetTaskFailedResponse(ResponseBase response)
	{
		TaskError taskError = new TaskError();
		if (response == null)
		{
			taskError.ErrorCode = 60634;
			taskError.ErrorText = VmoResources.VMO60634;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is InitializeDiscoveryComponentFailedResponse)
		{
			taskError.ErrorCode = 60630;
			taskError.ErrorText = VmoResources.VMO60630;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is StartSessionFailedResponse)
		{
			taskError.ErrorCode = 60630;
			taskError.ErrorText = VmoResources.VMO60630;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is DiscoveryComponentSecureScanUnsupportedResponse)
		{
			taskError.ErrorCode = 60630;
			taskError.ErrorText = VmoResources.VMO60630;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is RegisterSessionFailedResponse)
		{
			taskError.ErrorCode = 60631;
			taskError.ErrorText = VmoResources.VMO60631;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is DiscoveryComponentFailedResponse)
		{
			taskError.ErrorCode = 60631;
			taskError.ErrorText = VmoResources.VMO60631;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is RecorderUnavailableResponse)
		{
			taskError.ErrorCode = 60635;
			taskError.ErrorText = VmoResources.VMO60633;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is RecorderNotSupportedResponse)
		{
			taskError.ErrorCode = 60636;
			taskError.ErrorText = VmoResources.VMO60636;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is TimeoutResponse)
		{
			taskError.ErrorCode = 60634;
			taskError.ErrorText = VmoResources.VMO60634;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is FailResponse)
		{
			taskError.ErrorCode = 60254;
			taskError.ErrorText = VmoResources.VMO60254;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		if (response is ExceptionResponse exceptionResponse)
		{
			if (exceptionResponse.Exception is TargetInvocationException && exceptionResponse.Exception.InnerException != null)
			{
				Logger.LogWarning("Failed to scan for hardware during 'Hardware scan express'. [Error: {Exception}]", exceptionResponse.Exception.InnerException);
			}
			else
			{
				Logger.LogWarning("Failed to scan for hardware during 'Hardware scan express'. [Error: {Exception}]", exceptionResponse.Exception);
			}
			taskError.ErrorCode = 60637;
			taskError.ErrorText = VmoResources.VMO60637;
			return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
		}
		Logger.LogWarning("Failed to scan for hardware during 'Hardware scan express'. Response: {Response}", (response == null) ? "<NULL>" : response.ToString());
		taskError.ErrorCode = 60637;
		taskError.ErrorText = VmoResources.VMO60637;
		return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
	}
}
