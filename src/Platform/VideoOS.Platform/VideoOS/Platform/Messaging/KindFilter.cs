using System;

namespace VideoOS.Platform.Messaging;

public class KindFilter : MessageFilter
{
	private Guid _kind;

	public KindFilter(Guid kind)
	{
		_kind = kind;
	}

	public override bool Match(Message message, FQID destination, FQID sender)
	{
		if (message.RelatedFQID == null)
		{
			return false;
		}
		return message.RelatedFQID.Kind == _kind;
	}
}
