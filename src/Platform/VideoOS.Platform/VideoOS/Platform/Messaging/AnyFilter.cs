namespace VideoOS.Platform.Messaging;

public class AnyFilter : MessageFilter
{
	public override bool Match(Message message, FQID destination, FQID sender)
	{
		return true;
	}
}
