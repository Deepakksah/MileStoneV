using System;
using System.Diagnostics;
using System.Text;

namespace VideoOS.Common.Integration.Configuration;

public class License
{
	public static class WellKnownId
	{
		public static Guid ServerLicense { get; } = new Guid("94F58776-84BD-44DB-B19B-851F1D53A832");

		public static Guid MipLicense { get; } = new Guid("B1CB8385-EC7E-492B-B046-1B84E378CB71");

		public static Guid SiteInformation { get; } = new Guid("6F1E043A-B0FA-421D-957F-2474327112AA");

		public static Guid LicenseOwner { get; } = new Guid("464F449A-A14F-4131-B86E-24F16B82DEFB");
	}

	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.LicenseRow _datarow;

	public ConfigurationDataSet.LicenseRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public Guid IDLicense => _datarow.IDLicense;

	public bool HasData => _datarow.Data.Length != 0;

	public byte[] DataAsBytes
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Data;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Data = value;
		}
	}

	public string DataAsString
	{
		[DebuggerStepThrough]
		get
		{
			return Encoding.UTF8.GetString(_datarow.Data);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Data = Encoding.UTF8.GetBytes(value);
		}
	}

	public DateTime LastModified
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.LastModified;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.LastModified = value;
		}
	}

	public License(ConfigurationManager configurationManager, ConfigurationDataSet.LicenseRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}

	public void Refresh()
	{
		using ConfigurationDataSet configurationDataSet = _configurationManager.dataLoader.GetLicenseByLicenseId(IDLicense);
		_datarow.Table.BeginLoadData();
		_configurationManager.DisableEventHandling = true;
		foreach (ConfigurationDataSet.LicenseRow row in configurationDataSet.License.Rows)
		{
			if (_datarow.IDLicense == row.IDLicense)
			{
				_datarow.ItemArray = row.ItemArray;
				_datarow.AcceptChanges();
				break;
			}
		}
		_configurationManager.DisableEventHandling = false;
		_datarow.Table.EndLoadData();
	}
}
