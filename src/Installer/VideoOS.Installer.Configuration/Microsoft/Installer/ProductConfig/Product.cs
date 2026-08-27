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
[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
[XmlRoot(Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper", IsNullable = false)]
public class Product
{
	private PackageFilesType packageFilesField;

	private object[] installChecksField;

	private CommandsType commandsField;

	private StringsTypeString[] stringsField;

	private SchedulesTypeSchedule[] schedulesField;

	private object[] relatedProductsField;

	private string productCodeField;

	public PackageFilesType PackageFiles
	{
		get
		{
			return packageFilesField;
		}
		set
		{
			packageFilesField = value;
		}
	}

	[XmlArrayItem("AssemblyCheck", typeof(InstallChecksTypeAssemblyCheck), IsNullable = false)]
	[XmlArrayItem("ExternalCheck", typeof(InstallChecksTypeExternalCheck), IsNullable = false)]
	[XmlArrayItem("FileCheck", typeof(InstallChecksTypeFileCheck), IsNullable = false)]
	[XmlArrayItem("MsiProductCheck", typeof(InstallChecksTypeMsiProductCheck), IsNullable = false)]
	[XmlArrayItem("RegistryCheck", typeof(InstallChecksTypeRegistryCheck), IsNullable = false)]
	[XmlArrayItem("RegistryFileCheck", typeof(InstallChecksTypeRegistryFileCheck), IsNullable = false)]
	public object[] InstallChecks
	{
		get
		{
			return installChecksField;
		}
		set
		{
			installChecksField = value;
		}
	}

	public CommandsType Commands
	{
		get
		{
			return commandsField;
		}
		set
		{
			commandsField = value;
		}
	}

	[XmlArrayItem("String")]
	public StringsTypeString[] Strings
	{
		get
		{
			return stringsField;
		}
		set
		{
			stringsField = value;
		}
	}

	[XmlArrayItem("Schedule")]
	public SchedulesTypeSchedule[] Schedules
	{
		get
		{
			return schedulesField;
		}
		set
		{
			schedulesField = value;
		}
	}

	[XmlArrayItem("DependsOnProduct", typeof(RelatedProductsTypeDependsOnProduct))]
	[XmlArrayItem("EitherProducts", typeof(EitherProductsType), IsNullable = false)]
	[XmlArrayItem("IncludesProduct", typeof(RelatedProductsTypeIncludesProduct))]
	public object[] RelatedProducts
	{
		get
		{
			return relatedProductsField;
		}
		set
		{
			relatedProductsField = value;
		}
	}

	[XmlAttribute]
	public string ProductCode
	{
		get
		{
			return productCodeField;
		}
		set
		{
			productCodeField = value;
		}
	}
}
