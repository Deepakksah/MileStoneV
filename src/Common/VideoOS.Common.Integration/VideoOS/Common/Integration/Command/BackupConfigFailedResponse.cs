using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public sealed class BackupConfigFailedResponse : CommonResponseBase
{
	[DataMember]
	public BackupConfigError ErrorCode { get; set; }

	[DataMember]
	public string EnglishErrorDescription { get; set; }

	[DataMember]
	public string[] ErrorParameters { get; private set; }

	[DataMember]
	public ExceptionDetail ServerException { get; set; }

	[DataMember]
	public bool UnauthorizedAccessException { get; set; }

	public BackupConfigFailedResponse(BackupConfigError errorCode, string englishErrorDescription, IList<string> errorParameters, Exception serverException)
	{
		ErrorCode = errorCode;
		EnglishErrorDescription = englishErrorDescription;
		if (errorParameters == null)
		{
			ErrorParameters = new string[0];
		}
		else
		{
			ErrorParameters = errorParameters.ToArray();
		}
		UnauthorizedAccessException = false;
		if (serverException is FaultException)
		{
			FaultException ex = serverException as FaultException;
			if (ex.Code != null && string.Equals("51000", ex.Code.Name, StringComparison.OrdinalIgnoreCase))
			{
				UnauthorizedAccessException = true;
			}
		}
		ServerException = new ExceptionDetail(serverException);
	}

	public override string ToString()
	{
		return GetType().Name;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
