using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MonitorPreset")]
public class MonitorPreset : VmoObjectGuidBased, IUpdatable
{
	public string DefinitionXml
	{
		get
		{
			return GetPropertyValue<string>("DefinitionXml");
		}
		set
		{
			SetPropertyValue("DefinitionXml", value);
		}
	}

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public Monitor Monitor => GetObjectById<Monitor>(MonitorId);

	public string MonitorId => GetPropertyValue<string>("MonitorId");

	public VideoWallPreset VideoWallPreset => GetObjectById<VideoWallPreset>(VideoWallPresetId);

	public string VideoWallPresetId => GetPropertyValue<string>("VideoWallPresetId");

	public string VideoWallId => GetPropertyValue<string>("VideoWallId");

	public MonitorPreset(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}
}
