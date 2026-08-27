using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class KeyValueDto
{
	private string key;

	private string value;

	public string Key
	{
		[DebuggerStepThrough]
		get
		{
			return key;
		}
		[DebuggerStepThrough]
		set
		{
			key = value;
		}
	}

	public string Value
	{
		[DebuggerStepThrough]
		get
		{
			return value;
		}
		[DebuggerStepThrough]
		set
		{
			this.value = value;
		}
	}

	public KeyValueDto(string key, string value)
	{
		this.key = key;
		this.value = value;
	}
}
