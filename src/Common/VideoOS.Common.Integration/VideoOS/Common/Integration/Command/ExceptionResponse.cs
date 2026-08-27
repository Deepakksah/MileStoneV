using System;
using System.Globalization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public sealed class ExceptionResponse : CommonResponseBase
{
	public Exception Exception { get; private set; }

	public ExceptionResponse(Exception exception)
	{
		Exception = exception;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", GetType().Name, Exception);
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
