using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.Installer.ProductConfig;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
public class PackageFilesType
{
	private FileType[] packageFileField;

	private CopyAllFilesType copyAllPackageFilesField;

	[XmlElement("PackageFile")]
	public FileType[] PackageFile
	{
		get
		{
			return packageFileField;
		}
		set
		{
			packageFileField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(CopyAllFilesType.@true)]
	public CopyAllFilesType CopyAllPackageFiles
	{
		get
		{
			return copyAllPackageFilesField;
		}
		set
		{
			copyAllPackageFilesField = value;
		}
	}

	public PackageFilesType()
	{
		copyAllPackageFilesField = CopyAllFilesType.@true;
	}
}
