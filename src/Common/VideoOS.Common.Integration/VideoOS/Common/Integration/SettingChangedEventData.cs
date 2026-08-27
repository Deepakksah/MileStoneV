using System.Diagnostics;

namespace VideoOS.Common.Integration;

[DebuggerDisplay("{section} {setting.Name}")]
public class SettingChangedEventData
{
	private SettingChangedAction action;

	private string section;

	private SettingEntry setting;

	private string valueBefore;

	private string valueAfter;

	public SettingChangedAction Action
	{
		[DebuggerStepThrough]
		get
		{
			return action;
		}
	}

	public string Section
	{
		[DebuggerStepThrough]
		get
		{
			return section;
		}
	}

	public SettingEntry Setting
	{
		[DebuggerStepThrough]
		get
		{
			return setting;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return setting.Name;
		}
	}

	public string ValueBefore
	{
		[DebuggerStepThrough]
		get
		{
			return valueBefore;
		}
	}

	public string ValueAfter
	{
		[DebuggerStepThrough]
		get
		{
			return valueAfter;
		}
	}

	public SettingChangedEventData(SettingChangedAction action, string section, SettingEntry setting, string valueBefore, string valueAfter)
	{
		this.action = action;
		this.section = section;
		this.setting = setting;
		this.valueBefore = valueBefore;
		this.valueAfter = valueAfter;
	}
}
