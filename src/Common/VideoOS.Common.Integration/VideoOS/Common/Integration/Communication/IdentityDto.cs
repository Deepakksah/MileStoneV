using System;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Communication.Internal;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public sealed class IdentityDto
{
	[DataMember]
	public string Sid;

	[DataMember]
	public IdentityTypeDto Type;

	[DataMember]
	public string AccountName;

	[DataMember]
	public string DisplayName;

	[DataMember]
	public string Domain;

	[DataMember]
	public string DistinguishedName;

	[DataMember]
	public bool Deleted;

	[DataMember]
	public string Description;

	[DataMember]
	public string[] MemberOf;

	[DataMember]
	public string[] Members;

	public static IdentityDto Create(Identity identity)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		return new IdentityDto
		{
			Sid = identity.Sid,
			Type = DtoConvert.ToIdentityTypeDto(identity.Type),
			AccountName = identity.AccountName,
			DisplayName = identity.DisplayName,
			Domain = identity.Domain,
			DistinguishedName = identity.DistinguishedName,
			Deleted = identity.Deleted,
			Description = identity.Description,
			MemberOf = (string[])identity.MemberOf.Clone(),
			Members = (string[])identity.Members.Clone()
		};
	}

	public static Identity CreateIdentity(IdentityDto identityDto)
	{
		if (identityDto == null)
		{
			throw new ArgumentNullException("identityDto");
		}
		return new Identity
		{
			Sid = identityDto.Sid,
			Type = DtoConvert.ToIdentityType(identityDto.Type),
			AccountName = identityDto.AccountName,
			DisplayName = identityDto.DisplayName,
			Domain = identityDto.Domain,
			DistinguishedName = identityDto.DistinguishedName,
			Deleted = identityDto.Deleted,
			Description = identityDto.Description,
			MemberOf = (string[])identityDto.MemberOf.Clone(),
			Members = (string[])identityDto.Members.Clone()
		};
	}
}
