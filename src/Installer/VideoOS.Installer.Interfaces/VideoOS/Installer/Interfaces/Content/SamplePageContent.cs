using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces.Content;

public class SamplePageContent
{
	public string SampleName { get; }

	public string SampleTextBox { get; set; }

	public bool SampleCheckBox { get; set; }

	public string SelectedComboBoxItem { get; set; }

	public IEnumerable<string> ComboBoxSelectableItems { get; set; }

	public int Rank { get; }

	public virtual bool Validate()
	{
		return true;
	}

	public SamplePageContent(string sampleName, string sampleTextBox, bool sampleCheckBox, string selectedComboBoxItem, int rank)
	{
		SampleName = sampleName;
		SampleTextBox = sampleTextBox;
		SampleCheckBox = sampleCheckBox;
		SelectedComboBoxItem = selectedComboBoxItem;
		Rank = rank;
	}
}
