using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("DevicePrivacyProtection")]
public class PrivacyProtection : VmoObjectGuidBased, IUpdatable
{
	private PrivacyMask _mask;

	public bool Enabled
	{
		get
		{
			return GetPropertyValue<bool>("Enabled");
		}
		set
		{
			SetPropertyValue("Enabled", value);
		}
	}

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public string DeviceId => GetPropertyValue<string>("DeviceId");

	public CameraDevice Device => GetObjectById<CameraDevice>(DeviceId);

	public PrivacyMask Mask
	{
		get
		{
			if (_mask == null)
			{
				string propertyValue = GetPropertyValue<string>("MaskXml");
				_mask = PrivacyMask.FromXml(propertyValue);
				_mask.Changed += MaskChanged;
			}
			return _mask;
		}
	}

	public override bool IsCustomPropertiesSupported => false;

	public PrivacyProtection(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	private void MaskChanged(object sender, EventArgs e)
	{
		SetPropertyValue("MaskXml", _mask.ToXml());
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(InternalId);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	protected override void PropertiesLoaded()
	{
		base.PropertiesLoaded();
		if (_mask != null)
		{
			string propertyValue = GetPropertyValue<string>("MaskXml");
			_mask.LoadXml(propertyValue);
		}
	}
}
