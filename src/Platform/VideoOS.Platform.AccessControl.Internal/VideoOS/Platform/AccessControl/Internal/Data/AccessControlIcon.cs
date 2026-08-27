using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlIcon", Namespace = "urn:milestone-systems")]
public class AccessControlIcon : IExtensibleDataObject
{
	private Icon _icon;

	[DataMember(Order = 1)]
	public string IconKey { get; set; }

	[DataMember(Order = 2)]
	public byte[] IconData { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlIcon()
	{
	}

	public AccessControlIcon(string iconKey, Icon icon)
	{
		IconKey = iconKey;
		_icon = icon;
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			icon.Save(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			IconData = memoryStream.ToArray();
			memoryStream.Close();
		}
		catch (Exception)
		{
		}
	}

	public Icon GetIcon()
	{
		if (_icon == null)
		{
			try
			{
				using MemoryStream memoryStream = new MemoryStream(IconData);
				_icon = new Icon(memoryStream);
				memoryStream.Close();
			}
			catch (Exception)
			{
			}
		}
		return _icon;
	}
}
