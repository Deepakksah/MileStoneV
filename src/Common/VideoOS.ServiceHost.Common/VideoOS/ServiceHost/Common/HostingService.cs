using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using VideoOS.Utilities.Validation;

namespace VideoOS.ServiceHost.Common;

public abstract class HostingService : ServerHostCreator, IHostingService, IDisposable
{
	private readonly ICommunicationServiceHostLog _communicationServiceHostLog;

	public abstract IEnumerable<System.ServiceModel.ServiceHost> Hosts { get; }

	protected abstract string LogSuccessMessage { get; }

	protected HostingService(ICommunicationServiceHostLog communicationServiceHostLog, ICertificateRepository certificateRepository, IWebServiceSettings webServiceSettings)
		: base(certificateRepository, webServiceSettings)
	{
		communicationServiceHostLog.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\HostingService.cs", 27);
		_communicationServiceHostLog = communicationServiceHostLog;
	}

	public void Open()
	{
		try
		{
			foreach (System.ServiceModel.ServiceHost host in Hosts)
			{
				try
				{
					host.Open();
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}
			string logSuccessMessage = LogSuccessMessage;
			if (!string.IsNullOrEmpty(logSuccessMessage))
			{
				_communicationServiceHostLog.Info(this, logSuccessMessage);
			}
		}
		catch (AddressAccessDeniedException ex)
		{
			_communicationServiceHostLog.Error(this, GetLogErrorMessage(ex) + ex.Message, ex);
		}
	}

	public void Close()
	{
		foreach (System.ServiceModel.ServiceHost item in Hosts.Where((System.ServiceModel.ServiceHost h) => h.State == CommunicationState.Opened))
		{
			item.Close();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		foreach (System.ServiceModel.ServiceHost host in Hosts)
		{
			((IDisposable)host).Dispose();
		}
	}

	protected abstract string GetLogErrorMessage(Exception exception);
}
