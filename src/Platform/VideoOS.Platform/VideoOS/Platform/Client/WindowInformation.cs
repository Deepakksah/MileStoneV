using System;

namespace VideoOS.Platform.Client;

public abstract class WindowInformation
{
	public abstract Mode Mode { get; }

	public abstract ViewAndLayoutItem ViewAndLayoutItem { get; }

	public virtual Item Window => null;

	public virtual Guid WindowId => Guid.Empty;

	public FQID PlaybackControllerFQID => ClientControl.Instance.GetPlaybackController(this);

	public bool IsSentFromMyWindow(FQID sender)
	{
		if (WindowId == Guid.Empty && sender == null)
		{
			return true;
		}
		if (WindowId == Kind.Window && sender == null)
		{
			return true;
		}
		if (WindowId == Guid.Empty || sender == null)
		{
			return false;
		}
		return WindowId == sender.ObjectId;
	}
}
