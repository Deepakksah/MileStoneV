using System.Globalization;
using VideoOS.Management.Client.Proxy;

namespace VideoOS.Management.Client;

public class NumberedObjectKey : ObjectKeyBase
{
	public int Position { get; set; }

	public override bool IsNull => Position == -1;

	public NumberedObjectKey(int position)
	{
		Position = position;
	}

	public override ObjectKeyBase Clone()
	{
		return new NumberedObjectKey(Position);
	}

	public override string GetExceptionName()
	{
		return Position.ToString(CultureInfo.InvariantCulture);
	}

	public override GenericObjectKey ToGenericObjectKey()
	{
		GenericObjectKey genericObjectKey = base.ToGenericObjectKey();
		genericObjectKey.Position = Position;
		return genericObjectKey;
	}
}
