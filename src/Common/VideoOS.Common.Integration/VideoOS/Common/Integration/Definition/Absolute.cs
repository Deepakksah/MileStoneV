using System.Diagnostics;

namespace VideoOS.Common.Integration.Definition;

public class Absolute
{
	private bool canQueryPosition;

	private bool speeedSupport;

	public bool CanQueryPosition
	{
		[DebuggerStepThrough]
		get
		{
			return canQueryPosition;
		}
		[DebuggerStepThrough]
		set
		{
			canQueryPosition = value;
		}
	}

	public bool SpeedSupport
	{
		[DebuggerStepThrough]
		get
		{
			return speeedSupport;
		}
		[DebuggerStepThrough]
		set
		{
			speeedSupport = value;
		}
	}
}
