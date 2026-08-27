using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class HelpForm : Form
{
	protected string helpTopic;

	private IContainer components;

	public HelpForm()
	{
		base.HelpRequested += HelpForm_HelpRequested;
		helpTopic = base.Name;
	}

	protected void ShowHelp()
	{
		string internalTopicId = helpTopic;
		string text = GetHelpTopic();
		string defaultFallbackTopic = GetDefaultFallbackTopic();
		if (string.IsNullOrEmpty(text))
		{
			if (string.IsNullOrEmpty(helpTopic))
			{
				internalTopicId = base.Name;
			}
		}
		else
		{
			internalTopicId = text;
		}
		CorporateHelp.Instance.ShowHelpTopic(internalTopicId, defaultFallbackTopic, base.Handle);
	}

	protected virtual string GetHelpTopic()
	{
		if (string.IsNullOrEmpty(helpTopic))
		{
			return base.Name;
		}
		return string.Empty;
	}

	protected virtual string GetDefaultFallbackTopic()
	{
		return null;
	}

	private void HelpForm_HelpRequested(object sender, HelpEventArgs hlpevent)
	{
		ShowHelp();
		hlpevent.Handled = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.HelpForm));
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "HelpForm";
		base.ResumeLayout(false);
	}
}
