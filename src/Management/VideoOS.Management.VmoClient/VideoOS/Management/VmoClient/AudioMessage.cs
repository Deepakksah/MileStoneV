using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("AudioMessage")]
public class AudioMessage : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public string Name
	{
		get
		{
			return GetPropertyValue<string>("Name");
		}
		set
		{
			SetPropertyValue("Name", value);
		}
	}

	public string Description
	{
		get
		{
			return GetPropertyValue<string>("Description");
		}
		set
		{
			SetPropertyValue("Description", value);
		}
	}

	public string OriginalFileName => GetPropertyValue<string>("OriginalFileName");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public override bool IsCustomPropertiesSupported => false;

	public AudioMessage(ManagementServer managementServer, string name, string description, string originalFileName, byte[] content)
		: this(managementServer.Repositories, name, description, originalFileName, content)
	{
	}

	public AudioMessage(IVmoRepositories repositories, string name, string description, string originalFileName, byte[] content)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (description == null)
		{
			throw new ArgumentNullException("description");
		}
		if (originalFileName == null)
		{
			throw new ArgumentNullException("originalFileName");
		}
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		SetPropertyValue("Name", name);
		SetPropertyValue("Description", description);
		SetPropertyValue("OriginalFileName", originalFileName);
		SetPropertyValue("Content", Convert.ToBase64String(content));
	}

	public AudioMessage(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(InternalId);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public byte[] GetContent()
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		return Convert.FromBase64String(base.Repositories.ServerCommunication.Execute("GetAudioMessageContent", vmoPropertyCollection)[0].Properties.GetPropertyValue<string>("Content"));
	}

	public void SetContent(string originalFileName, byte[] content)
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		vmoPropertyCollection.Add("OriginalFileName", originalFileName);
		vmoPropertyCollection.Add("Content", Convert.ToBase64String(content));
		base.Repositories.ServerCommunication.Execute("SetAudioMessageContent", vmoPropertyCollection);
		SetPropertyValue("OriginalFileName", originalFileName);
	}
}
