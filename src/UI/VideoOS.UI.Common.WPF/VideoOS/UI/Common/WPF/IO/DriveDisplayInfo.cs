using System;
using System.Globalization;
using System.IO;
using System.Text;
using VideoOS.UI.Common.WPF.IO.FolderBrowser.resources;

namespace VideoOS.UI.Common.WPF.IO;

public class DriveDisplayInfo : DisplayInfo
{
	public DriveInfo DriveInfo { get; private set; }

	public DriveDisplayInfo(DriveInfo driveInfo)
	{
		DriveInfo = driveInfo;
		base.RootDirectory = driveInfo.RootDirectory.Name;
		base.FullPath = driveInfo.Name;
		try
		{
			base.DisplayText = ExtractDisplayName(driveInfo);
		}
		catch
		{
			base.DisplayText = string.Format(CultureInfo.InvariantCulture, "({0})", driveInfo.Name);
		}
	}

	private static string ExtractDisplayName(DriveInfo driveInfo)
	{
		if (!driveInfo.IsReady)
		{
			return "";
		}
		switch (driveInfo.DriveType)
		{
		case DriveType.CDRom:
		{
			string volumeLabel = driveInfo.VolumeLabel;
			if (string.IsNullOrWhiteSpace(volumeLabel))
			{
				return string.Format(CultureInfo.InvariantCulture, "({0})", driveInfo.Name);
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", volumeLabel, driveInfo.Name);
		}
		case DriveType.Fixed:
		{
			string volumeLabel = driveInfo.VolumeLabel;
			if (string.IsNullOrWhiteSpace(volumeLabel))
			{
				volumeLabel = Strings.DefaultFixedDiskName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", volumeLabel, driveInfo.Name);
		}
		case DriveType.Removable:
		{
			string volumeLabel = driveInfo.VolumeLabel;
			if (string.IsNullOrWhiteSpace(volumeLabel))
			{
				volumeLabel = Strings.DefaultRemovableDiskName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", volumeLabel, driveInfo.Name);
		}
		case DriveType.Network:
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			int length = stringBuilder.Capacity;
			if (NativeMethods.WNetGetConnection(driveInfo.Name.Replace("\\", ""), stringBuilder, ref length) == 0)
			{
				string text = stringBuilder.ToString();
				int num = text.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
				string arg = text.Substring(0, num);
				string arg2 = text.Substring(num + 1, text.Length - num - 1);
				return string.Format(CultureInfo.InvariantCulture, "{0} ({1}) ({2})", arg2, arg, driveInfo.Name);
			}
			return string.Format(CultureInfo.InvariantCulture, "({0})", driveInfo.Name);
		}
		default:
			return "";
		}
	}
}
