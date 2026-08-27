using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VideoOS.Platform.UI.Automation.Serialization;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.UI.Automation.AutomationPeers;

public class VideoOSTreeViewAutomationPeer : VideoOSAutomationPeerBase
{
	public VideoOSTreeViewAutomationPeer(VideoOSTreeView owner)
		: base(owner)
	{
	}

	protected override List<AutomationObjectSerializationPath> GetAutomationPathPatterns()
	{
		VideoOSTreeView videoOSTreeView = (VideoOSTreeView)base.Owner;
		List<AutomationObjectSerializationPath> list = new List<AutomationObjectSerializationPath>();
		list.Add(new AutomationObjectSerializationPath("*"));
		list.Add(new AutomationObjectSerializationPath(FormattableString.Invariant(FormattableStringFactory.Create("{0}.*", "SelectedItem"))));
		list.AddRange(VideoOSAutomationPeerBase.GenerateActionsPaths(FormattableString.Invariant(FormattableStringFactory.Create("({0}){1}", typeof(VideoOSTreeViewItem).AssemblyQualifiedName, "SelectedItem")), "Actions"));
		list.AddRange(VideoOSAutomationPeerBase.GenerateActionsPaths(FormattableString.Invariant(FormattableStringFactory.Create("(!{0}){1}", typeof(VideoOSTreeViewItem).AssemblyQualifiedName, "SelectedItem")), videoOSTreeView.ItemActionsPropertyName));
		list.Add(new AutomationObjectSerializationPath(FormattableString.Invariant(FormattableStringFactory.Create("{0}[].*", "ItemsSource"))));
		list.AddRange(VideoOSAutomationPeerBase.GenerateActionsPaths(FormattableString.Invariant(FormattableStringFactory.Create("{0}[({1})]", "ItemsSource", typeof(VideoOSTreeViewItem).AssemblyQualifiedName)), "Actions"));
		list.AddRange(VideoOSAutomationPeerBase.GenerateActionsPaths(FormattableString.Invariant(FormattableStringFactory.Create("{0}[(!{1})]", "ItemsSource", typeof(VideoOSTreeViewItem).AssemblyQualifiedName)), videoOSTreeView.ItemActionsPropertyName));
		return list;
	}
}
