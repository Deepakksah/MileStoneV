using System.Diagnostics;

namespace VideoOS.Common.Integration.Definition;

public class Internal
{
	private bool isNumbered;

	private int minNumber;

	private int maxNumber;

	private string formatExpression = string.Empty;

	private bool canLoadFromDevice;

	private bool canSetPreset;

	private bool speedSupport;

	public bool IsNumbered
	{
		[DebuggerStepThrough]
		get
		{
			return isNumbered;
		}
		[DebuggerStepThrough]
		set
		{
			isNumbered = value;
		}
	}

	public int MinimumNumber
	{
		[DebuggerStepThrough]
		get
		{
			return minNumber;
		}
		[DebuggerStepThrough]
		set
		{
			minNumber = value;
		}
	}

	public int MaximumNumber
	{
		[DebuggerStepThrough]
		get
		{
			return maxNumber;
		}
		[DebuggerStepThrough]
		set
		{
			maxNumber = value;
		}
	}

	public string FormatExpression
	{
		[DebuggerStepThrough]
		get
		{
			return formatExpression;
		}
		[DebuggerStepThrough]
		set
		{
			formatExpression = value;
		}
	}

	public bool CanLoadFromDevice
	{
		[DebuggerStepThrough]
		get
		{
			return canLoadFromDevice;
		}
		[DebuggerStepThrough]
		set
		{
			canLoadFromDevice = value;
		}
	}

	public bool CanSetPreset
	{
		[DebuggerStepThrough]
		get
		{
			return canSetPreset;
		}
		[DebuggerStepThrough]
		set
		{
			canSetPreset = value;
		}
	}

	public bool SpeedSupport
	{
		[DebuggerStepThrough]
		get
		{
			return speedSupport;
		}
		[DebuggerStepThrough]
		set
		{
			speedSupport = value;
		}
	}
}
