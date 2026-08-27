using System;

namespace VideoOS.Mobile.Portable.Interfaces;

public interface IControlExt : IControl
{
	bool WaitToStop(TimeSpan period);
}
