using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class SingleSummaryControlViewModel : INotifyPropertyChanged
{
	private string _header;

	private string _emptySummaryDescription = "";

	public ObservableCollection<string> Summary { get; } = new ObservableCollection<string>();

	public string Header
	{
		get
		{
			return _header;
		}
		set
		{
			if (_header != value)
			{
				_header = value;
				OnPropertyChanged("Header");
			}
		}
	}

	public string EmptySummaryDescription
	{
		get
		{
			return _emptySummaryDescription;
		}
		set
		{
			_emptySummaryDescription = value;
			OnPropertyChanged("EmptySummaryDescription");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public SingleSummaryControlViewModel(IEnumerable<string> items, string header)
	{
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		foreach (string item in items)
		{
			Summary.Add(item);
		}
		Header = header;
	}

	protected void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
