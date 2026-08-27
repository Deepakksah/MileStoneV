using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using VideoOS.Platform.Login;

namespace VideoOS.ConfigurationApi.ClientService;

[DebuggerDisplay("{Key} = {Value}")]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Property", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class Property : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DisplayNameField;

	private bool IsSettableField;

	private bool ItemReferenceField;

	private string KeyField;

	private string SelectedValueTypeInfoNameField;

	private bool ServerValidationField;

	private int SortKeyField;

	private string ToolTipTranslationIdField;

	private string TranslationIdField;

	private int UIImportanceField;

	private string ValueField;

	private string[] ValueArrayField;

	private string ValueTypeField;

	private ValueTypeInfo[] ValueTypeInfosField;

	private bool IsSecureValue;

	private SecureString SecureValueField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public bool IsSettable
	{
		get
		{
			return IsSettableField;
		}
		set
		{
			IsSettableField = value;
		}
	}

	[DataMember]
	public bool ItemReference
	{
		get
		{
			return ItemReferenceField;
		}
		set
		{
			ItemReferenceField = value;
		}
	}

	[DataMember]
	public string Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			KeyField = value;
			EnsureSecure();
		}
	}

	[DataMember]
	public string SelectedValueTypeInfoName
	{
		get
		{
			return SelectedValueTypeInfoNameField;
		}
		set
		{
			SelectedValueTypeInfoNameField = value;
		}
	}

	[DataMember]
	public bool ServerValidation
	{
		get
		{
			return ServerValidationField;
		}
		set
		{
			ServerValidationField = value;
		}
	}

	[DataMember]
	public int SortKey
	{
		get
		{
			return SortKeyField;
		}
		set
		{
			SortKeyField = value;
		}
	}

	[DataMember]
	public string ToolTipTranslationId
	{
		get
		{
			return ToolTipTranslationIdField;
		}
		set
		{
			ToolTipTranslationIdField = value;
		}
	}

	[DataMember]
	public string TranslationId
	{
		get
		{
			return TranslationIdField;
		}
		set
		{
			TranslationIdField = value;
		}
	}

	[DataMember]
	public int UIImportance
	{
		get
		{
			return UIImportanceField;
		}
		set
		{
			UIImportanceField = value;
			EnsureSecure();
		}
	}

	[DataMember]
	public string Value
	{
		get
		{
			if (IsSecureValue)
			{
				if (SecureValue == null)
				{
					return string.Empty;
				}
				return new NetworkCredential(string.Empty, SecureValue).Password;
			}
			return ValueField;
		}
		set
		{
			if (IsSecureValue)
			{
				SecureValueField = value.ToSecureString();
			}
			else
			{
				ValueField = value;
			}
		}
	}

	public SecureString SecureValue
	{
		get
		{
			return SecureValueField;
		}
		set
		{
			IsSecureValue = true;
			SecureValueField = value;
		}
	}

	[DataMember]
	public string[] ValueArray
	{
		get
		{
			return ValueArrayField;
		}
		set
		{
			ValueArrayField = value;
		}
	}

	[DataMember]
	public string ValueType
	{
		get
		{
			return ValueTypeField;
		}
		set
		{
			ValueTypeField = value;
		}
	}

	[DataMember]
	public ValueTypeInfo[] ValueTypeInfos
	{
		get
		{
			return ValueTypeInfosField;
		}
		set
		{
			ValueTypeInfosField = value;
		}
	}

	private void EnsureSecure()
	{
		if (UIImportanceField == 4 || string.Compare(Key.ToLower(), "password") == 0)
		{
			IsSecureValue = true;
			if (!string.IsNullOrEmpty(Value))
			{
				SecureValueField = ValueField.ToSecureString();
				ValueField = string.Empty;
			}
		}
	}
}
