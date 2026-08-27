using System;
using System.Drawing;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Internal.Util;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlCredentialHolder", Namespace = "urn:milestone-systems")]
public class AccessControlCredentialHolder : IExtensibleDataObject
{
	private Image _picture;

	[DataMember(Order = 1)]
	public Guid AccessControlSystemId { get; set; }

	[DataMember(Order = 2)]
	public string AccessControlCredentialHolderId { get; set; }

	[DataMember(Order = 3)]
	public string[] CredentialHolderRoles { get; set; }

	[DataMember(Order = 4)]
	public string Name { get; set; }

	[DataMember(Order = 5)]
	public byte[] PictureData { get; set; }

	[DataMember(Order = 6)]
	public Property[] Properties { get; set; }

	[DataMember(Order = 7)]
	public bool PictureOverridden { get; set; }

	[DataMember(Order = 8)]
	public DateTime ReceivedTime { get; set; }

	[DataMember(Order = 9)]
	public long SeqNo { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlCredentialHolder()
	{
	}

	public AccessControlCredentialHolder(Guid accessControlSystemId, string accessControlCredentialHolderId, string name, string[] credentialHolderRoles, Image picture, Property[] properties, bool pictureOverridden)
	{
		AccessControlSystemId = accessControlSystemId;
		AccessControlCredentialHolderId = accessControlCredentialHolderId;
		Name = name;
		CredentialHolderRoles = credentialHolderRoles;
		_picture = picture;
		Properties = properties;
		PictureOverridden = pictureOverridden;
		if (picture != null)
		{
			try
			{
				PictureData = ImageToByteDataConverter.ToByteData(picture);
			}
			catch (Exception)
			{
			}
		}
	}

	public Image GetPicture()
	{
		if (_picture == null && PictureData != null && PictureData.Length != 0)
		{
			try
			{
				_picture = ImageToByteDataConverter.FromByteData(PictureData);
			}
			catch (Exception)
			{
			}
		}
		return _picture;
	}

	public void InvalidatePicture()
	{
		_picture = null;
	}
}
