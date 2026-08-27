using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class NotificationKey<T>
{
	private readonly T _message;

	private readonly List<T> _argsList;

	public T Message => _message;

	public List<T> ArgumentsList => _argsList;

	public NotificationKey(T message, List<T> argsList)
	{
		_message = message;
		_argsList = new List<T>();
		if (argsList != null)
		{
			_argsList.AddRange(argsList);
		}
	}

	public override int GetHashCode()
	{
		return (_message.GetHashCode() * 397) ^ ((_argsList != null) ? _argsList.GetHashCode() : 0);
	}

	public override bool Equals(object other)
	{
		if (other is NotificationKey<T> { _message: var message } notificationKey && message.Equals(_message))
		{
			return _argsList.SequenceEqual(notificationKey._argsList);
		}
		return false;
	}
}
