using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using VideoOS.Platform.UI.Automation.Serialization;

namespace VideoOS.Platform.UI.Automation.AutomationPeers;

public abstract class VideoOSAutomationPeerBase : FrameworkElementAutomationPeer, IValueProvider
{
	public static readonly AutomationObjectSerializationOptions SerializationOptions = new AutomationObjectSerializationOptions
	{
		IncludeTypeName = true,
		IncludeAssemblyQualifiedTypeName = true,
		IncludeHashCode = true,
		IncludeToStringValue = true
	};

	public static Func<object, string> JsonSerializer { get; set; }

	public string Value => GetValue(base.Owner, GetAutomationPathPatterns());

	public bool IsReadOnly => true;

	public static string GetValue(UIElement owner, List<AutomationObjectSerializationPath> patterns = null)
	{
		return AutomationObjectSerializer.Serialize(owner, patterns, SerializationOptions, (object obj) => JsonSerializer?.Invoke(obj) ?? "JSON serialization not initialized");
	}

	public VideoOSAutomationPeerBase(FrameworkElement owner)
		: base(owner)
	{
	}

	public override object GetPattern(PatternInterface patternInterface)
	{
		if (patternInterface == PatternInterface.Value)
		{
			return this;
		}
		return base.GetPattern(patternInterface);
	}

	public void SetValue(string value)
	{
		throw new NotImplementedException();
	}

	protected sealed override string GetNameCore()
	{
		string nameCore = base.GetNameCore();
		if (!string.IsNullOrWhiteSpace(nameCore))
		{
			return nameCore;
		}
		return GetClassNameCore();
	}

	protected sealed override string GetClassNameCore()
	{
		return base.Owner.GetType().Name;
	}

	protected virtual List<AutomationObjectSerializationPath> GetAutomationPathPatterns()
	{
		return new List<AutomationObjectSerializationPath>();
	}

	protected static List<string> GenerateActionsSubPaths(string actionsPropertyName)
	{
		List<string> list = new List<string>();
		list.Add(FormattableString.Invariant($"{actionsPropertyName}[].*"));
		list.Add(FormattableString.Invariant($"{actionsPropertyName}[].Tag.*"));
		list.Add(FormattableString.Invariant(FormattableStringFactory.Create("{0}[].{{{1}[]}}.*", actionsPropertyName, "Actions")));
		list.Add(FormattableString.Invariant(FormattableStringFactory.Create("{0}[].{{{1}[]}}.Tag.*", actionsPropertyName, "Actions")));
		return list;
	}

	protected static List<AutomationObjectSerializationPath> GenerateActionsPaths(string prePath, string actionsPropertyName)
	{
		List<AutomationObjectSerializationPath> list = new List<AutomationObjectSerializationPath>();
		list.Add(new AutomationObjectSerializationPath(FormattableString.Invariant($"{prePath}.{actionsPropertyName}[].*")));
		list.Add(new AutomationObjectSerializationPath(FormattableString.Invariant($"{prePath}.{actionsPropertyName}[].Tag.*")));
		list.Add(new AutomationObjectSerializationPath(FormattableString.Invariant(FormattableStringFactory.Create("{0}.{1}[].{{{2}[]}}.*", prePath, actionsPropertyName, "Actions"))));
		list.Add(new AutomationObjectSerializationPath(FormattableString.Invariant(FormattableStringFactory.Create("{0}.{1}[].{{{2}[]}}.Tag.*", prePath, actionsPropertyName, "Actions"))));
		return list;
	}
}
