using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MatrixCommandServerTask : ServerTask
{
	private ObservableCollection<string> _MatrixCameraList;

	public Dictionary<string, string> MatrixCommandTypeValues => base.Wrapper.GetEnumValues("MatrixCommandType");

	public string MatrixCommandType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MatrixCommandType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MatrixCommandType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ObservableCollection<string> MatrixCameraList
	{
		get
		{
			if (_MatrixCameraList == null)
			{
				_MatrixCameraList = base.Wrapper.GetPropertyArrayString("MatrixCameraList");
				_MatrixCameraList.CollectionChanged += _MatrixCameraList_CollectionChanged;
			}
			return _MatrixCameraList;
		}
	}

	public MatrixCommandServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MatrixCommand"));
	}

	private void _MatrixCameraList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("MatrixCameraList", _MatrixCameraList);
	}
}
