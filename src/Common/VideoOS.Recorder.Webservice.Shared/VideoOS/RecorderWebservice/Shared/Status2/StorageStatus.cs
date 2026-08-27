using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class StorageStatus
{
	private Guid storageIdField;

	private string nameField;

	private string pathField;

	private bool availableField;

	private ulong usedSpaceInBytesField;

	private ulong freeSpaceInBytesField;

	private bool mountedField;

	public Guid StorageId
	{
		get
		{
			return storageIdField;
		}
		set
		{
			storageIdField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public string Path
	{
		get
		{
			return pathField;
		}
		set
		{
			pathField = value;
		}
	}

	public bool Available
	{
		get
		{
			return availableField;
		}
		set
		{
			availableField = value;
		}
	}

	public ulong UsedSpaceInBytes
	{
		get
		{
			return usedSpaceInBytesField;
		}
		set
		{
			usedSpaceInBytesField = value;
		}
	}

	public ulong FreeSpaceInBytes
	{
		get
		{
			return freeSpaceInBytesField;
		}
		set
		{
			freeSpaceInBytesField = value;
		}
	}

	public bool Mounted
	{
		get
		{
			return mountedField;
		}
		set
		{
			mountedField = value;
		}
	}
}
