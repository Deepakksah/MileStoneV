using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VideoOS.Management.Server.SecurityModel;

namespace VideoOS.Management.Server.RestApi.DataAccess;

public class PermissionObjectSecurity
{
	private Dictionary<ulong, PermissionTimeprofile> _timeProfiles = new Dictionary<ulong, PermissionTimeprofile>();

	[JsonProperty(PropertyName = "path")]
	public PathDataModel Path { get; set; }

	[JsonProperty(PropertyName = "permissionMask")]
	public ulong PermissionMask { get; set; }

	[JsonProperty(PropertyName = "timeProfiles", NullValueHandling = NullValueHandling.Ignore)]
	public List<PermissionTimeprofile> TimeProfiles
	{
		get
		{
			if (_timeProfiles.Count <= 0)
			{
				return null;
			}
			return _timeProfiles.Values.ToList();
		}
		set
		{
			_timeProfiles = value.ToDictionary((PermissionTimeprofile x) => x.RelatedToMask, (PermissionTimeprofile x) => x);
		}
	}

	[JsonProperty(PropertyName = "priorities", NullValueHandling = NullValueHandling.Ignore)]
	public List<SecurityPriority> Priorities { get; set; }

	public PermissionObjectSecurity()
	{
	}

	public PermissionObjectSecurity(string resourceName, string objectId)
	{
		Path = new PathDataModel
		{
			Type = resourceName,
			Id = objectId
		};
	}

	public void StoreEntry(ulong mask)
	{
		PermissionMask |= mask;
	}

	public void StoreTimeProfile(ulong mask, PermissionTimeprofile compactTimeprofile)
	{
		if ((mask & 1) == 0L)
		{
			_timeProfiles[mask] = compactTimeprofile;
			PermissionMask |= mask;
		}
	}

	public void StoreTimeProfile(ulong mask, bool isDefault, string idString, int backwards)
	{
		string id = (isDefault ? SecurityPermissionDataTranslater.DefaultTimeProfileId : (string.IsNullOrEmpty(idString) ? SecurityPermissionDataTranslater.AlwaysTimeProfileId : idString));
		StoreTimeProfile(mask, new PermissionTimeprofile(mask)
		{
			TimeBackwards = backwards,
			TimeProfilePath = new PathDataModel
			{
				Type = "timeProfiles",
				Id = id
			}
		});
	}

	public void StorePriority(ulong mask, int priority)
	{
		if (Priorities == null)
		{
			Priorities = new List<SecurityPriority>();
		}
		SecurityPriority securityPriority = Priorities.FirstOrDefault((SecurityPriority p) => p.RelatedToMask == mask);
		if (securityPriority != null)
		{
			securityPriority.Priority = priority;
			return;
		}
		Priorities.Add(new SecurityPriority
		{
			RelatedToMask = mask,
			Priority = priority
		});
	}
}
