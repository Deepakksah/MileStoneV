using System;
using System.Drawing;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Internal.Util;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetCredentialHolderImageParams", Namespace = "urn:milestone-systems")]
public class SetCredentialHolderImageParams : IExtensibleDataObject
{
	private Image _picture;

	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public string AccessControlCredentialHolderId { get; set; }

	[DataMember]
	public byte[] PictureData { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public SetCredentialHolderImageParams(Guid accessControlSystemId, string accessControlCredentialHolderId)
		: this(accessControlSystemId, accessControlCredentialHolderId, null)
	{
	}

	public SetCredentialHolderImageParams(Guid accessControlSystemId, string accessControlCredentialHolderId, Image picture)
	{
		AccessControlSystemId = accessControlSystemId;
		AccessControlCredentialHolderId = accessControlCredentialHolderId;
		_picture = picture;
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
}
