using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.Platform.Search;

public abstract class SearchResultUserControl : UserControl, INotifyPropertyChanged
{
	private bool _isLoading;

	public bool IsLoading
	{
		get
		{
			return _isLoading;
		}
		protected set
		{
			if (!object.Equals(_isLoading, value))
			{
				_isLoading = value;
				OnPropertyChanged("IsLoading");
				if (!SuppressDefaultLoadingVisualization)
				{
					this.IsLoadingChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}

	protected bool SuppressDefaultLoadingVisualization { get; set; }

	public Thickness DefaultLoadingVisualizationMargin { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public event EventHandler IsLoadingChanged;

	public abstract void Init(SearchResultData searchResultData);

	public virtual void Close()
	{
	}

	protected virtual void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
