using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public sealed class ExceptionResponseDataContract : CommonResponseBase
{
	[DataMember]
	public ExceptionDetail Exception { get; set; }

	[DataMember]
	public string ExceptionAssemblyName { get; set; }

	public ExceptionResponseDataContract()
	{
	}

	public ExceptionResponseDataContract(ExceptionResponse exceptionResponse)
	{
		if (exceptionResponse == null)
		{
			throw new ArgumentNullException("exceptionResponse");
		}
		Exception = new ExceptionDetail(exceptionResponse.Exception);
		ExceptionAssemblyName = exceptionResponse.Exception.GetType().Assembly.GetName().Name;
		base.Token = exceptionResponse.Token;
	}

	public bool IsTypeOrSubclassOf(Type exceptionType)
	{
		try
		{
			Type type = Type.GetType(Exception.Type + ", " + ExceptionAssemblyName);
			return exceptionType == type || type.IsSubclassOf(exceptionType);
		}
		catch
		{
			return false;
		}
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", GetType().Name, Exception.ToString());
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
