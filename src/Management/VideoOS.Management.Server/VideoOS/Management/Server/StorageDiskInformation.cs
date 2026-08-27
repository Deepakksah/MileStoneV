using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class StorageDiskInformation
{
	public long AvailableSpace { get; set; }

	public long UsedSpace { get; set; }

	public long LockedUsedSpace { get; set; }

	public DateTimeOffset LastUpdated { get; set; }

	public TableContainsAnyData AnyTablesWithData { get; set; }

	public bool IsAvailable { get; set; }

	public bool IsMounted { get; set; }

	public static StorageDiskInformation GetRecordingStorageInformation(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		StorageDiskInformationInternal storageDiskInformationInternal = null;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			storageDiskInformationInternal = recordingStorageComponent.QueryRecordingStorageDiskUsage(recordingStorageId);
			if (storageDiskInformationInternal != null)
			{
				requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", storageDiskInformationInternal.RecordingServerId);
			}
		}
		if (storageDiskInformationInternal == null || string.IsNullOrEmpty(storageDiskInformationInternal.DiskUsageXml))
		{
			return null;
		}
		try
		{
			return GetStorageDiskUsage(storageDiskInformationInternal.DiskUsageXml, recordingStorageId);
		}
		catch (ArgumentException)
		{
			throw new RecordingStorageInformationException();
		}
	}

	public static StorageDiskInformation GetArchiveStorageInformation(IGeminiRequestContext requestContext, string archiveStorageId)
	{
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		StorageDiskInformationInternal storageDiskInformationInternal = null;
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			storageDiskInformationInternal = archiveStorageComponent.QueryArchiveStorageDiskUsage(archiveStorageId);
			if (storageDiskInformationInternal != null)
			{
				requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", storageDiskInformationInternal.RecordingServerId);
			}
		}
		if (storageDiskInformationInternal == null || string.IsNullOrEmpty(storageDiskInformationInternal.DiskUsageXml))
		{
			return null;
		}
		try
		{
			return GetStorageDiskUsage(storageDiskInformationInternal.DiskUsageXml, archiveStorageId);
		}
		catch (ArgumentException)
		{
			throw new ArchiveStorageInformationException();
		}
	}

	private static StorageDiskInformation GetStorageDiskUsage(string diskUsageXml, string storageId)
	{
		StorageDiskInformation storageDiskInformation = new StorageDiskInformation();
		try
		{
			using StringReader textReader = new StringReader(diskUsageXml);
			XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("StorageUsage");
			if (xPathNavigator2 == null)
			{
				throw new ArgumentException("Invalid xml - StorageUsage node missing", "diskUsageXml");
			}
			if (!DateTimeOffset.TryParseExact(xPathNavigator2.GetAttribute("LocalTime", string.Empty), "o", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result))
			{
				throw new ArgumentException("Invalid xml - LocalTime attribute can't be parsed", "diskUsageXml");
			}
			storageDiskInformation.LastUpdated = result;
			string xpath = string.Format(CultureInfo.InvariantCulture, "StorageUsage/Bank[translate(@Id, 'abcdef', 'ABCDEF') = '{0}']", storageId.ToUpperInvariant());
			XPathNavigator xPathNavigator3 = xPathNavigator.SelectSingleNode(xpath);
			if (xPathNavigator3 == null)
			{
				return null;
			}
			if (!long.TryParse(xPathNavigator3.GetAttribute("UsedSize", string.Empty), out var result2))
			{
				throw new ArgumentException("Invalid xml - UsedSize attribute can't be parsed", "diskUsageXml");
			}
			storageDiskInformation.UsedSpace = result2;
			if (long.TryParse(xPathNavigator3.GetAttribute("LockedUsedSize", string.Empty), out var result3))
			{
				storageDiskInformation.LockedUsedSpace = result3;
			}
			else
			{
				storageDiskInformation.LockedUsedSpace = 0L;
			}
			if (bool.TryParse(xPathNavigator3.GetAttribute("AnyTablesWithData", string.Empty), out var result4))
			{
				storageDiskInformation.AnyTablesWithData = (result4 ? TableContainsAnyData.ContainsData : TableContainsAnyData.DoesNotContainData);
			}
			else
			{
				storageDiskInformation.AnyTablesWithData = TableContainsAnyData.Unknown;
			}
			storageDiskInformation.IsAvailable = true;
			storageDiskInformation.IsMounted = true;
			if (int.TryParse(xPathNavigator3.GetAttribute("ErrorNumber", string.Empty), out var result5))
			{
				switch (result5)
				{
				case 10000:
					storageDiskInformation.IsAvailable = false;
					break;
				case 10004:
					storageDiskInformation.IsAvailable = false;
					storageDiskInformation.IsMounted = false;
					break;
				}
			}
		}
		catch (XmlException)
		{
			throw new ArgumentException("Invalid xml - xml can't be parsed", "diskUsageXml");
		}
		return storageDiskInformation;
	}
}
