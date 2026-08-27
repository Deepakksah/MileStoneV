namespace VideoOS.Platform.Messaging;

public abstract class MessageFilter
{
	public virtual bool Match(Message message, FQID destination, FQID sender)
	{
		return true;
	}
}
