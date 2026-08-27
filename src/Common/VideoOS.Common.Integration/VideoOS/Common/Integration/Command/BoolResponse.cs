using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class BoolResponse : CommonResponseBase
{
	[DataMember]
	public bool Value { get; private set; }

	public BoolResponse(bool value)
	{
		Value = value;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", GetType().Name, Value);
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
