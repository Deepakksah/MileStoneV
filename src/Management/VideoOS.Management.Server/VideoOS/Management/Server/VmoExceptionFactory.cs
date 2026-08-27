using System;
using System.CodeDom.Compiler;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[GeneratedCode("CreateVmoExceptionFactory", "0.0.0.0")]
public static class VmoExceptionFactory
{
	public static VmoServerServiceException VMO60000_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60000)
		{
			ErrorCode = 60000
		};
	}

	public static VmoServerServiceException VMO60000_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60000, innerException)
		{
			ErrorCode = 60000
		};
	}

	public static VmoServerServiceException VMO60001_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60001, name))
		{
			ErrorCode = 60001,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60001_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60001, name), innerException)
		{
			ErrorCode = 60001,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60002_VmoServerServiceException(string idRole)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60002, idRole))
		{
			ErrorCode = 60002,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDRole", idRole)
			} }
		};
	}

	public static VmoServerServiceException VMO60002_VmoServerServiceException(string idRole, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60002, idRole), innerException)
		{
			ErrorCode = 60002,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDRole", idRole)
			} }
		};
	}

	public static VmoServerServiceException VMO60003_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60003, name))
		{
			ErrorCode = 60003,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60003_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60003, name), innerException)
		{
			ErrorCode = 60003,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60004_VmoServerServiceException(string idRole)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60004, idRole))
		{
			ErrorCode = 60004,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDRole", idRole)
			} }
		};
	}

	public static VmoServerServiceException VMO60004_VmoServerServiceException(string idRole, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60004, idRole), innerException)
		{
			ErrorCode = 60004,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDRole", idRole)
			} }
		};
	}

	public static VmoServerServiceException VMO60005_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60005)
		{
			ErrorCode = 60005
		};
	}

	public static VmoServerServiceException VMO60005_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60005, innerException)
		{
			ErrorCode = 60005
		};
	}

	public static VmoServerServiceException VMO60006_VmoServerServiceException(string idRole)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60006, idRole))
		{
			ErrorCode = 60006,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDRole", idRole)
			} }
		};
	}

	public static VmoServerServiceException VMO60006_VmoServerServiceException(string idRole, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60006, idRole), innerException)
		{
			ErrorCode = 60006,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDRole", idRole)
			} }
		};
	}

	public static VmoServerServiceException VMO60007_VmoServerServiceException(string roleName, string memberName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60007, roleName, memberName))
		{
			ErrorCode = 60007,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "RoleName", roleName)
				},
				{
					1,
					new MessageParameter(1, "MemberName", memberName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60007_VmoServerServiceException(string roleName, string memberName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60007, roleName, memberName), innerException)
		{
			ErrorCode = 60007,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "RoleName", roleName)
				},
				{
					1,
					new MessageParameter(1, "MemberName", memberName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60008_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60008)
		{
			ErrorCode = 60008
		};
	}

	public static VmoServerServiceException VMO60008_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60008, innerException)
		{
			ErrorCode = 60008
		};
	}

	public static VmoServerServiceException VMO60009_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60009, name))
		{
			ErrorCode = 60009,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60009_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60009, name), innerException)
		{
			ErrorCode = 60009,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60010_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60010)
		{
			ErrorCode = 60010
		};
	}

	public static VmoServerServiceException VMO60010_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60010, innerException)
		{
			ErrorCode = 60010
		};
	}

	public static VmoServerServiceException VMO60011_VmoServerServiceException(string memberSid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60011, memberSid))
		{
			ErrorCode = 60011,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "MemberSid", memberSid)
			} }
		};
	}

	public static VmoServerServiceException VMO60011_VmoServerServiceException(string memberSid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60011, memberSid), innerException)
		{
			ErrorCode = 60011,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "MemberSid", memberSid)
			} }
		};
	}

	public static VmoServerServiceException VMO60012_VmoServerServiceException(string idObject)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60012, idObject))
		{
			ErrorCode = 60012,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDObject", idObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60012_VmoServerServiceException(string idObject, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60012, idObject), innerException)
		{
			ErrorCode = 60012,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDObject", idObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60013_VmoServerServiceException(string idAction)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60013, idAction))
		{
			ErrorCode = 60013,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDAction", idAction)
			} }
		};
	}

	public static VmoServerServiceException VMO60013_VmoServerServiceException(string idAction, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60013, idAction), innerException)
		{
			ErrorCode = 60013,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDAction", idAction)
			} }
		};
	}

	public static VmoServerServiceException VMO60014_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60014, name))
		{
			ErrorCode = 60014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60014_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60014, name), innerException)
		{
			ErrorCode = 60014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60015_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60015, name))
		{
			ErrorCode = 60015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60015_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60015, name), innerException)
		{
			ErrorCode = 60015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60016_VmoServerServiceException(string idObject)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60016, idObject))
		{
			ErrorCode = 60016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDObject", idObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60016_VmoServerServiceException(string idObject, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60016, idObject), innerException)
		{
			ErrorCode = 60016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDObject", idObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60017_VmoServerServiceException(string idClass)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60017, idClass))
		{
			ErrorCode = 60017,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDClass", idClass)
			} }
		};
	}

	public static VmoServerServiceException VMO60017_VmoServerServiceException(string idClass, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60017, idClass), innerException)
		{
			ErrorCode = 60017,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDClass", idClass)
			} }
		};
	}

	public static VmoServerServiceException VMO60018_VmoServerServiceException(string idParentObject)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60018, idParentObject))
		{
			ErrorCode = 60018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDParentObject", idParentObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60018_VmoServerServiceException(string idParentObject, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60018, idParentObject), innerException)
		{
			ErrorCode = 60018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDParentObject", idParentObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60019_VmoServerServiceException(string idParentObject, string idObject)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60019, idParentObject, idObject))
		{
			ErrorCode = 60019,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDParentObject", idParentObject)
				},
				{
					1,
					new MessageParameter(1, "IDObject", idObject)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60019_VmoServerServiceException(string idParentObject, string idObject, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60019, idParentObject, idObject), innerException)
		{
			ErrorCode = 60019,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDParentObject", idParentObject)
				},
				{
					1,
					new MessageParameter(1, "IDObject", idObject)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60020_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60020, name))
		{
			ErrorCode = 60020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60020_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60020, name), innerException)
		{
			ErrorCode = 60020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60021_VmoServerServiceException(string idObject)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60021, idObject))
		{
			ErrorCode = 60021,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDObject", idObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60021_VmoServerServiceException(string idObject, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60021, idObject), innerException)
		{
			ErrorCode = 60021,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDObject", idObject)
			} }
		};
	}

	public static VmoServerServiceException VMO60022_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60022)
		{
			ErrorCode = 60022
		};
	}

	public static VmoServerServiceException VMO60022_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60022, innerException)
		{
			ErrorCode = 60022
		};
	}

	public static VmoServerServiceException VMO60023_VmoServerServiceException(string idClass)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60023, idClass))
		{
			ErrorCode = 60023,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDClass", idClass)
			} }
		};
	}

	public static VmoServerServiceException VMO60023_VmoServerServiceException(string idClass, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60023, idClass), innerException)
		{
			ErrorCode = 60023,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDClass", idClass)
			} }
		};
	}

	public static VmoServerServiceException VMO60024_VmoServerServiceException(string idObject, string idAction, string idUser)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60024, idObject, idAction, idUser))
		{
			ErrorCode = 60024,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDObject", idObject)
				},
				{
					1,
					new MessageParameter(1, "IDAction", idAction)
				},
				{
					2,
					new MessageParameter(2, "IDUser", idUser)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60024_VmoServerServiceException(string idObject, string idAction, string idUser, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60024, idObject, idAction, idUser), innerException)
		{
			ErrorCode = 60024,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDObject", idObject)
				},
				{
					1,
					new MessageParameter(1, "IDAction", idAction)
				},
				{
					2,
					new MessageParameter(2, "IDUser", idUser)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60025_VmoServerServiceException(string name, string recordingServerName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60025, name, recordingServerName))
		{
			ErrorCode = 60025,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60025_VmoServerServiceException(string name, string recordingServerName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60025, name, recordingServerName), innerException)
		{
			ErrorCode = 60025,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60026_VmoServerServiceException(string name, string recordingServerName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60026, name, recordingServerName))
		{
			ErrorCode = 60026,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60026_VmoServerServiceException(string name, string recordingServerName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60026, name, recordingServerName), innerException)
		{
			ErrorCode = 60026,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60027_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60027)
		{
			ErrorCode = 60027
		};
	}

	public static VmoServerServiceException VMO60027_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60027, innerException)
		{
			ErrorCode = 60027
		};
	}

	public static VmoServerServiceException VMO60028_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60028)
		{
			ErrorCode = 60028
		};
	}

	public static VmoServerServiceException VMO60028_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60028, innerException)
		{
			ErrorCode = 60028
		};
	}

	public static VmoServerServiceException VMO60029_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60029)
		{
			ErrorCode = 60029
		};
	}

	public static VmoServerServiceException VMO60029_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60029, innerException)
		{
			ErrorCode = 60029
		};
	}

	public static VmoServerServiceException VMO60030_VmoServerServiceException(string idParentGroup)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60030, idParentGroup))
		{
			ErrorCode = 60030,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDParentGroup", idParentGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60030_VmoServerServiceException(string idParentGroup, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60030, idParentGroup), innerException)
		{
			ErrorCode = 60030,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDParentGroup", idParentGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60031_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60031, name))
		{
			ErrorCode = 60031,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60031_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60031, name), innerException)
		{
			ErrorCode = 60031,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60032_VmoServerServiceException(string idFailoverGroup)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60032, idFailoverGroup))
		{
			ErrorCode = 60032,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverGroup", idFailoverGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60032_VmoServerServiceException(string idFailoverGroup, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60032, idFailoverGroup), innerException)
		{
			ErrorCode = 60032,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverGroup", idFailoverGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60033_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60033, name))
		{
			ErrorCode = 60033,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60033_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60033, name), innerException)
		{
			ErrorCode = 60033,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60034_VmoServerServiceException(string idFailoverGroup)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60034, idFailoverGroup))
		{
			ErrorCode = 60034,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverGroup", idFailoverGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60034_VmoServerServiceException(string idFailoverGroup, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60034, idFailoverGroup), innerException)
		{
			ErrorCode = 60034,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverGroup", idFailoverGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60035_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60035)
		{
			ErrorCode = 60035
		};
	}

	public static VmoServerServiceException VMO60035_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60035, innerException)
		{
			ErrorCode = 60035
		};
	}

	public static VmoServerServiceException VMO60036_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60036)
		{
			ErrorCode = 60036
		};
	}

	public static VmoServerServiceException VMO60036_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60036, innerException)
		{
			ErrorCode = 60036
		};
	}

	public static VmoServerServiceException VMO60037_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60037, name, groupName))
		{
			ErrorCode = 60037,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60037_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60037, name, groupName), innerException)
		{
			ErrorCode = 60037,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60038_VmoServerServiceException(string idFailoverGroup)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60038, idFailoverGroup))
		{
			ErrorCode = 60038,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverGroup", idFailoverGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60038_VmoServerServiceException(string idFailoverGroup, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60038, idFailoverGroup), innerException)
		{
			ErrorCode = 60038,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverGroup", idFailoverGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60039_VmoServerServiceException(string idFailoverServer)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60039, idFailoverServer))
		{
			ErrorCode = 60039,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverServer", idFailoverServer)
			} }
		};
	}

	public static VmoServerServiceException VMO60039_VmoServerServiceException(string idFailoverServer, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60039, idFailoverServer), innerException)
		{
			ErrorCode = 60039,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverServer", idFailoverServer)
			} }
		};
	}

	public static VmoServerServiceException VMO60040_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60040, name, groupName))
		{
			ErrorCode = 60040,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60040_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60040, name, groupName), innerException)
		{
			ErrorCode = 60040,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60041_VmoServerServiceException(string idTargetGroup)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60041, idTargetGroup))
		{
			ErrorCode = 60041,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDTargetGroup", idTargetGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60041_VmoServerServiceException(string idTargetGroup, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60041, idTargetGroup), innerException)
		{
			ErrorCode = 60041,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDTargetGroup", idTargetGroup)
			} }
		};
	}

	public static VmoServerServiceException VMO60042_VmoServerServiceException(string targetGroupName, string failoverServerName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60042, targetGroupName, failoverServerName))
		{
			ErrorCode = 60042,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "targetGroupName", targetGroupName)
				},
				{
					1,
					new MessageParameter(1, "failoverServerName", failoverServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60042_VmoServerServiceException(string targetGroupName, string failoverServerName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60042, targetGroupName, failoverServerName), innerException)
		{
			ErrorCode = 60042,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "targetGroupName", targetGroupName)
				},
				{
					1,
					new MessageParameter(1, "failoverServerName", failoverServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60043_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60043)
		{
			ErrorCode = 60043
		};
	}

	public static VmoServerServiceException VMO60043_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60043, innerException)
		{
			ErrorCode = 60043
		};
	}

	public static VmoServerServiceException VMO60044_VmoServerServiceException(string idEventType)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60044, idEventType))
		{
			ErrorCode = 60044,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDEventType", idEventType)
			} }
		};
	}

	public static VmoServerServiceException VMO60044_VmoServerServiceException(string idEventType, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60044, idEventType), innerException)
		{
			ErrorCode = 60044,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDEventType", idEventType)
			} }
		};
	}

	public static VmoServerServiceException VMO60045_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60045)
		{
			ErrorCode = 60045
		};
	}

	public static VmoServerServiceException VMO60045_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60045, innerException)
		{
			ErrorCode = 60045
		};
	}

	public static VmoServerServiceException VMO60046_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60046)
		{
			ErrorCode = 60046
		};
	}

	public static VmoServerServiceException VMO60046_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60046, innerException)
		{
			ErrorCode = 60046
		};
	}

	public static VmoServerServiceException VMO60047_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60047)
		{
			ErrorCode = 60047
		};
	}

	public static VmoServerServiceException VMO60047_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60047, innerException)
		{
			ErrorCode = 60047
		};
	}

	public static VmoServerServiceException VMO60048_VmoServerServiceException(string idFailoverServer)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60048, idFailoverServer))
		{
			ErrorCode = 60048,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverServer", idFailoverServer)
			} }
		};
	}

	public static VmoServerServiceException VMO60048_VmoServerServiceException(string idFailoverServer, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60048, idFailoverServer), innerException)
		{
			ErrorCode = 60048,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDFailoverServer", idFailoverServer)
			} }
		};
	}

	public static VmoServerServiceException VMO60049_VmoServerServiceException(string name, string recordingServerName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60049, name, recordingServerName))
		{
			ErrorCode = 60049,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60049_VmoServerServiceException(string name, string recordingServerName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60049, name, recordingServerName), innerException)
		{
			ErrorCode = 60049,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60050_VmoServerServiceException(string name, string recordingServerName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60050, name, recordingServerName))
		{
			ErrorCode = 60050,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60050_VmoServerServiceException(string name, string recordingServerName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60050, name, recordingServerName), innerException)
		{
			ErrorCode = 60050,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "recordingServerName", recordingServerName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60051_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60051)
		{
			ErrorCode = 60051
		};
	}

	public static VmoServerServiceException VMO60051_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60051, innerException)
		{
			ErrorCode = 60051
		};
	}

	public static VmoServerServiceException VMO60052_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60052)
		{
			ErrorCode = 60052
		};
	}

	public static VmoServerServiceException VMO60052_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60052, innerException)
		{
			ErrorCode = 60052
		};
	}

	public static VmoServerServiceException VMO60053_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60053)
		{
			ErrorCode = 60053
		};
	}

	public static VmoServerServiceException VMO60053_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60053, innerException)
		{
			ErrorCode = 60053
		};
	}

	public static VmoServerServiceException VMO60054_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60054)
		{
			ErrorCode = 60054
		};
	}

	public static VmoServerServiceException VMO60054_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60054, innerException)
		{
			ErrorCode = 60054
		};
	}

	public static VmoServerServiceException VMO60055_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60055, name, groupName))
		{
			ErrorCode = 60055,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60055_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60055, name, groupName), innerException)
		{
			ErrorCode = 60055,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60056_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60056)
		{
			ErrorCode = 60056
		};
	}

	public static VmoServerServiceException VMO60056_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60056, innerException)
		{
			ErrorCode = 60056
		};
	}

	public static VmoServerServiceException VMO60057_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60057)
		{
			ErrorCode = 60057
		};
	}

	public static VmoServerServiceException VMO60057_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60057, innerException)
		{
			ErrorCode = 60057
		};
	}

	public static VmoServerServiceException VMO60058_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60058)
		{
			ErrorCode = 60058
		};
	}

	public static VmoServerServiceException VMO60058_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60058, innerException)
		{
			ErrorCode = 60058
		};
	}

	public static VmoServerServiceException VMO60059_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60059)
		{
			ErrorCode = 60059
		};
	}

	public static VmoServerServiceException VMO60059_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60059, innerException)
		{
			ErrorCode = 60059
		};
	}

	public static VmoServerServiceException VMO60060_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60060)
		{
			ErrorCode = 60060
		};
	}

	public static VmoServerServiceException VMO60060_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60060, innerException)
		{
			ErrorCode = 60060
		};
	}

	public static VmoServerServiceException VMO60061_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60061, name))
		{
			ErrorCode = 60061,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60061_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60061, name), innerException)
		{
			ErrorCode = 60061,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60062_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60062)
		{
			ErrorCode = 60062
		};
	}

	public static VmoServerServiceException VMO60062_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60062, innerException)
		{
			ErrorCode = 60062
		};
	}

	public static VmoServerServiceException VMO60063_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60063)
		{
			ErrorCode = 60063
		};
	}

	public static VmoServerServiceException VMO60063_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60063, innerException)
		{
			ErrorCode = 60063
		};
	}

	public static VmoServerServiceException VMO60064_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60064)
		{
			ErrorCode = 60064
		};
	}

	public static VmoServerServiceException VMO60064_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60064, innerException)
		{
			ErrorCode = 60064
		};
	}

	public static VmoServerServiceException VMO60065_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60065)
		{
			ErrorCode = 60065
		};
	}

	public static VmoServerServiceException VMO60065_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60065, innerException)
		{
			ErrorCode = 60065
		};
	}

	public static VmoServerServiceException VMO60066_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60066)
		{
			ErrorCode = 60066
		};
	}

	public static VmoServerServiceException VMO60066_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60066, innerException)
		{
			ErrorCode = 60066
		};
	}

	public static VmoServerServiceException VMO60067_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60067)
		{
			ErrorCode = 60067
		};
	}

	public static VmoServerServiceException VMO60067_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60067, innerException)
		{
			ErrorCode = 60067
		};
	}

	public static VmoServerServiceException VMO60068_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60068)
		{
			ErrorCode = 60068
		};
	}

	public static VmoServerServiceException VMO60068_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60068, innerException)
		{
			ErrorCode = 60068
		};
	}

	public static VmoServerServiceException VMO60069_VmoServerServiceException(string idRecorderPlugin, string idDevice)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60069, idRecorderPlugin, idDevice))
		{
			ErrorCode = 60069,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDRecorderPlugin", idRecorderPlugin)
				},
				{
					1,
					new MessageParameter(1, "IDDevice", idDevice)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60069_VmoServerServiceException(string idRecorderPlugin, string idDevice, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60069, idRecorderPlugin, idDevice), innerException)
		{
			ErrorCode = 60069,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDRecorderPlugin", idRecorderPlugin)
				},
				{
					1,
					new MessageParameter(1, "IDDevice", idDevice)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60070_VmoServerServiceException(string idRecorderPlugin, string idDeviceTemplate)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60070, idRecorderPlugin, idDeviceTemplate))
		{
			ErrorCode = 60070,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDRecorderPlugin", idRecorderPlugin)
				},
				{
					1,
					new MessageParameter(1, "IDDeviceTemplate", idDeviceTemplate)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60070_VmoServerServiceException(string idRecorderPlugin, string idDeviceTemplate, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60070, idRecorderPlugin, idDeviceTemplate), innerException)
		{
			ErrorCode = 60070,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDRecorderPlugin", idRecorderPlugin)
				},
				{
					1,
					new MessageParameter(1, "IDDeviceTemplate", idDeviceTemplate)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60071_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60071)
		{
			ErrorCode = 60071
		};
	}

	public static VmoServerServiceException VMO60071_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60071, innerException)
		{
			ErrorCode = 60071
		};
	}

	public static VmoServerServiceException VMO60072_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60072)
		{
			ErrorCode = 60072
		};
	}

	public static VmoServerServiceException VMO60072_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60072, innerException)
		{
			ErrorCode = 60072
		};
	}

	public static VmoServerServiceException VMO60073_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60073)
		{
			ErrorCode = 60073
		};
	}

	public static VmoServerServiceException VMO60073_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60073, innerException)
		{
			ErrorCode = 60073
		};
	}

	public static VmoServerServiceException VMO60074_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60074)
		{
			ErrorCode = 60074
		};
	}

	public static VmoServerServiceException VMO60074_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60074, innerException)
		{
			ErrorCode = 60074
		};
	}

	public static VmoServerServiceException VMO60075_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60075)
		{
			ErrorCode = 60075
		};
	}

	public static VmoServerServiceException VMO60075_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60075, innerException)
		{
			ErrorCode = 60075
		};
	}

	public static VmoServerServiceException VMO60076_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60076)
		{
			ErrorCode = 60076
		};
	}

	public static VmoServerServiceException VMO60076_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60076, innerException)
		{
			ErrorCode = 60076
		};
	}

	public static VmoServerServiceException VMO60077_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60077)
		{
			ErrorCode = 60077
		};
	}

	public static VmoServerServiceException VMO60077_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60077, innerException)
		{
			ErrorCode = 60077
		};
	}

	public static VmoServerServiceException VMO60078_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60078)
		{
			ErrorCode = 60078
		};
	}

	public static VmoServerServiceException VMO60078_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60078, innerException)
		{
			ErrorCode = 60078
		};
	}

	public static VmoServerServiceException VMO60079_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60079)
		{
			ErrorCode = 60079
		};
	}

	public static VmoServerServiceException VMO60079_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60079, innerException)
		{
			ErrorCode = 60079
		};
	}

	public static VmoServerServiceException VMO60080_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60080)
		{
			ErrorCode = 60080
		};
	}

	public static VmoServerServiceException VMO60080_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60080, innerException)
		{
			ErrorCode = 60080
		};
	}

	public static VmoServerServiceException VMO60081_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60081)
		{
			ErrorCode = 60081
		};
	}

	public static VmoServerServiceException VMO60081_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60081, innerException)
		{
			ErrorCode = 60081
		};
	}

	public static VmoServerServiceException VMO60082_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60082)
		{
			ErrorCode = 60082
		};
	}

	public static VmoServerServiceException VMO60082_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60082, innerException)
		{
			ErrorCode = 60082
		};
	}

	public static VmoServerServiceException VMO60083_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60083)
		{
			ErrorCode = 60083
		};
	}

	public static VmoServerServiceException VMO60083_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60083, innerException)
		{
			ErrorCode = 60083
		};
	}

	public static VmoServerServiceException VMO60084_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60084)
		{
			ErrorCode = 60084
		};
	}

	public static VmoServerServiceException VMO60084_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60084, innerException)
		{
			ErrorCode = 60084
		};
	}

	public static VmoServerServiceException VMO60085_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60085)
		{
			ErrorCode = 60085
		};
	}

	public static VmoServerServiceException VMO60085_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60085, innerException)
		{
			ErrorCode = 60085
		};
	}

	public static VmoServerServiceException VMO60086_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60086)
		{
			ErrorCode = 60086
		};
	}

	public static VmoServerServiceException VMO60086_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60086, innerException)
		{
			ErrorCode = 60086
		};
	}

	public static VmoServerServiceException VMO60087_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60087, name, groupName))
		{
			ErrorCode = 60087,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60087_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60087, name, groupName), innerException)
		{
			ErrorCode = 60087,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "groupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60088_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60088)
		{
			ErrorCode = 60088
		};
	}

	public static VmoServerServiceException VMO60088_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60088, innerException)
		{
			ErrorCode = 60088
		};
	}

	public static VmoServerServiceException VMO60089_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60089)
		{
			ErrorCode = 60089
		};
	}

	public static VmoServerServiceException VMO60089_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60089, innerException)
		{
			ErrorCode = 60089
		};
	}

	public static VmoServerServiceException VMO60090_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60090)
		{
			ErrorCode = 60090
		};
	}

	public static VmoServerServiceException VMO60090_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60090, innerException)
		{
			ErrorCode = 60090
		};
	}

	public static VmoServerServiceException VMO60091_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60091)
		{
			ErrorCode = 60091
		};
	}

	public static VmoServerServiceException VMO60091_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60091, innerException)
		{
			ErrorCode = 60091
		};
	}

	public static VmoServerServiceException VMO60092_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60092)
		{
			ErrorCode = 60092
		};
	}

	public static VmoServerServiceException VMO60092_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60092, innerException)
		{
			ErrorCode = 60092
		};
	}

	public static VmoServerServiceException VMO60093_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60093)
		{
			ErrorCode = 60093
		};
	}

	public static VmoServerServiceException VMO60093_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60093, innerException)
		{
			ErrorCode = 60093
		};
	}

	public static VmoServerServiceException VMO60094_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60094)
		{
			ErrorCode = 60094
		};
	}

	public static VmoServerServiceException VMO60094_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60094, innerException)
		{
			ErrorCode = 60094
		};
	}

	public static VmoServerServiceException VMO60095_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60095)
		{
			ErrorCode = 60095
		};
	}

	public static VmoServerServiceException VMO60095_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60095, innerException)
		{
			ErrorCode = 60095
		};
	}

	public static VmoServerServiceException VMO60096_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60096)
		{
			ErrorCode = 60096
		};
	}

	public static VmoServerServiceException VMO60096_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60096, innerException)
		{
			ErrorCode = 60096
		};
	}

	public static VmoServerServiceException VMO60097_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60097)
		{
			ErrorCode = 60097
		};
	}

	public static VmoServerServiceException VMO60097_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60097, innerException)
		{
			ErrorCode = 60097
		};
	}

	public static VmoServerServiceException VMO60098_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60098)
		{
			ErrorCode = 60098
		};
	}

	public static VmoServerServiceException VMO60098_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60098, innerException)
		{
			ErrorCode = 60098
		};
	}

	public static VmoServerServiceException VMO60099_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60099)
		{
			ErrorCode = 60099
		};
	}

	public static VmoServerServiceException VMO60099_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60099, innerException)
		{
			ErrorCode = 60099
		};
	}

	public static VmoServerServiceException VMO60100_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60100)
		{
			ErrorCode = 60100
		};
	}

	public static VmoServerServiceException VMO60100_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60100, innerException)
		{
			ErrorCode = 60100
		};
	}

	public static VmoServerServiceException VMO60101_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60101)
		{
			ErrorCode = 60101
		};
	}

	public static VmoServerServiceException VMO60101_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60101, innerException)
		{
			ErrorCode = 60101
		};
	}

	public static VmoServerServiceException VMO60102_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60102, name))
		{
			ErrorCode = 60102,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60102_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60102, name), innerException)
		{
			ErrorCode = 60102,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60103_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60103, name))
		{
			ErrorCode = 60103,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60103_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60103, name), innerException)
		{
			ErrorCode = 60103,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60104_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60104, name))
		{
			ErrorCode = 60104,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60104_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60104, name), innerException)
		{
			ErrorCode = 60104,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60105_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60105)
		{
			ErrorCode = 60105
		};
	}

	public static VmoServerServiceException VMO60105_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60105, innerException)
		{
			ErrorCode = 60105
		};
	}

	public static VmoServerServiceException VMO60106_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60106)
		{
			ErrorCode = 60106
		};
	}

	public static VmoServerServiceException VMO60106_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60106, innerException)
		{
			ErrorCode = 60106
		};
	}

	public static VmoServerServiceException VMO60107_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60107)
		{
			ErrorCode = 60107
		};
	}

	public static VmoServerServiceException VMO60107_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60107, innerException)
		{
			ErrorCode = 60107
		};
	}

	public static VmoServerServiceException VMO60108_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60108, name, groupName))
		{
			ErrorCode = 60108,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60108_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60108, name, groupName), innerException)
		{
			ErrorCode = 60108,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60109_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60109)
		{
			ErrorCode = 60109
		};
	}

	public static VmoServerServiceException VMO60109_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60109, innerException)
		{
			ErrorCode = 60109
		};
	}

	public static VmoServerServiceException VMO60110_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60110)
		{
			ErrorCode = 60110
		};
	}

	public static VmoServerServiceException VMO60110_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60110, innerException)
		{
			ErrorCode = 60110
		};
	}

	public static VmoServerServiceException VMO60111_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60111, name, groupName))
		{
			ErrorCode = 60111,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60111_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60111, name, groupName), innerException)
		{
			ErrorCode = 60111,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60112_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60112)
		{
			ErrorCode = 60112
		};
	}

	public static VmoServerServiceException VMO60112_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60112, innerException)
		{
			ErrorCode = 60112
		};
	}

	public static VmoServerServiceException VMO60113_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60113)
		{
			ErrorCode = 60113
		};
	}

	public static VmoServerServiceException VMO60113_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60113, innerException)
		{
			ErrorCode = 60113
		};
	}

	public static VmoServerServiceException VMO60114_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60114)
		{
			ErrorCode = 60114
		};
	}

	public static VmoServerServiceException VMO60114_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60114, innerException)
		{
			ErrorCode = 60114
		};
	}

	public static VmoServerServiceException VMO60115_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60115)
		{
			ErrorCode = 60115
		};
	}

	public static VmoServerServiceException VMO60115_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60115, innerException)
		{
			ErrorCode = 60115
		};
	}

	public static VmoServerServiceException VMO60116_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60116, name, groupName))
		{
			ErrorCode = 60116,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60116_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60116, name, groupName), innerException)
		{
			ErrorCode = 60116,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60117_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60117)
		{
			ErrorCode = 60117
		};
	}

	public static VmoServerServiceException VMO60117_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60117, innerException)
		{
			ErrorCode = 60117
		};
	}

	public static VmoServerServiceException VMO60118_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60118)
		{
			ErrorCode = 60118
		};
	}

	public static VmoServerServiceException VMO60118_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60118, innerException)
		{
			ErrorCode = 60118
		};
	}

	public static VmoServerServiceException VMO60119_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60119)
		{
			ErrorCode = 60119
		};
	}

	public static VmoServerServiceException VMO60119_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60119, innerException)
		{
			ErrorCode = 60119
		};
	}

	public static VmoServerServiceException VMO60120_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60120)
		{
			ErrorCode = 60120
		};
	}

	public static VmoServerServiceException VMO60120_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60120, innerException)
		{
			ErrorCode = 60120
		};
	}

	public static VmoServerServiceException VMO60121_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60121)
		{
			ErrorCode = 60121
		};
	}

	public static VmoServerServiceException VMO60121_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60121, innerException)
		{
			ErrorCode = 60121
		};
	}

	public static VmoServerServiceException VMO60122_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60122)
		{
			ErrorCode = 60122
		};
	}

	public static VmoServerServiceException VMO60122_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60122, innerException)
		{
			ErrorCode = 60122
		};
	}

	public static VmoServerServiceException VMO60123_VmoServerServiceException(string name, string groupName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60123, name, groupName))
		{
			ErrorCode = 60123,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60123_VmoServerServiceException(string name, string groupName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60123, name, groupName), innerException)
		{
			ErrorCode = 60123,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Name", name)
				},
				{
					1,
					new MessageParameter(1, "GroupName", groupName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60124_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60124)
		{
			ErrorCode = 60124
		};
	}

	public static VmoServerServiceException VMO60124_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60124, innerException)
		{
			ErrorCode = 60124
		};
	}

	public static VmoServerServiceException VMO60125_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60125)
		{
			ErrorCode = 60125
		};
	}

	public static VmoServerServiceException VMO60125_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60125, innerException)
		{
			ErrorCode = 60125
		};
	}

	public static VmoServerServiceException VMO60126_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60126)
		{
			ErrorCode = 60126
		};
	}

	public static VmoServerServiceException VMO60126_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60126, innerException)
		{
			ErrorCode = 60126
		};
	}

	public static VmoServerServiceException VMO60127_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60127)
		{
			ErrorCode = 60127
		};
	}

	public static VmoServerServiceException VMO60127_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60127, innerException)
		{
			ErrorCode = 60127
		};
	}

	public static VmoServerServiceException VMO60128_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60128)
		{
			ErrorCode = 60128
		};
	}

	public static VmoServerServiceException VMO60128_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60128, innerException)
		{
			ErrorCode = 60128
		};
	}

	public static VmoServerServiceException VMO60129_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60129)
		{
			ErrorCode = 60129
		};
	}

	public static VmoServerServiceException VMO60129_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60129, innerException)
		{
			ErrorCode = 60129
		};
	}

	public static VmoServerServiceException VMO60130_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60130)
		{
			ErrorCode = 60130
		};
	}

	public static VmoServerServiceException VMO60130_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60130, innerException)
		{
			ErrorCode = 60130
		};
	}

	public static VmoServerServiceException VMO60131_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60131)
		{
			ErrorCode = 60131
		};
	}

	public static VmoServerServiceException VMO60131_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60131, innerException)
		{
			ErrorCode = 60131
		};
	}

	public static VmoServerServiceException VMO60132_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60132)
		{
			ErrorCode = 60132
		};
	}

	public static VmoServerServiceException VMO60132_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60132, innerException)
		{
			ErrorCode = 60132
		};
	}

	public static VmoServerServiceException VMO60133_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60133)
		{
			ErrorCode = 60133
		};
	}

	public static VmoServerServiceException VMO60133_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60133, innerException)
		{
			ErrorCode = 60133
		};
	}

	public static VmoServerServiceException VMO60134_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60134)
		{
			ErrorCode = 60134
		};
	}

	public static VmoServerServiceException VMO60134_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60134, innerException)
		{
			ErrorCode = 60134
		};
	}

	public static VmoServerServiceException VMO60135_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60135)
		{
			ErrorCode = 60135
		};
	}

	public static VmoServerServiceException VMO60135_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60135, innerException)
		{
			ErrorCode = 60135
		};
	}

	public static VmoServerServiceException VMO60136_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60136)
		{
			ErrorCode = 60136
		};
	}

	public static VmoServerServiceException VMO60136_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60136, innerException)
		{
			ErrorCode = 60136
		};
	}

	public static VmoServerServiceException VMO60137_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60137)
		{
			ErrorCode = 60137
		};
	}

	public static VmoServerServiceException VMO60137_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60137, innerException)
		{
			ErrorCode = 60137
		};
	}

	public static VmoServerServiceException VMO60138_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60138)
		{
			ErrorCode = 60138
		};
	}

	public static VmoServerServiceException VMO60138_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60138, innerException)
		{
			ErrorCode = 60138
		};
	}

	public static VmoServerServiceException VMO60139_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60139)
		{
			ErrorCode = 60139
		};
	}

	public static VmoServerServiceException VMO60139_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60139, innerException)
		{
			ErrorCode = 60139
		};
	}

	public static VmoServerServiceException VMO60140_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60140)
		{
			ErrorCode = 60140
		};
	}

	public static VmoServerServiceException VMO60140_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60140, innerException)
		{
			ErrorCode = 60140
		};
	}

	public static VmoServerServiceException VMO60141_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60141)
		{
			ErrorCode = 60141
		};
	}

	public static VmoServerServiceException VMO60141_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60141, innerException)
		{
			ErrorCode = 60141
		};
	}

	public static VmoServerServiceException VMO60142_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60142)
		{
			ErrorCode = 60142
		};
	}

	public static VmoServerServiceException VMO60142_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60142, innerException)
		{
			ErrorCode = 60142
		};
	}

	public static VmoServerServiceException VMO60143_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60143)
		{
			ErrorCode = 60143
		};
	}

	public static VmoServerServiceException VMO60143_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60143, innerException)
		{
			ErrorCode = 60143
		};
	}

	public static VmoServerServiceException VMO60144_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60144)
		{
			ErrorCode = 60144
		};
	}

	public static VmoServerServiceException VMO60144_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60144, innerException)
		{
			ErrorCode = 60144
		};
	}

	public static VmoServerServiceException VMO60145_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60145)
		{
			ErrorCode = 60145
		};
	}

	public static VmoServerServiceException VMO60145_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60145, innerException)
		{
			ErrorCode = 60145
		};
	}

	public static VmoServerServiceException VMO60146_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60146)
		{
			ErrorCode = 60146
		};
	}

	public static VmoServerServiceException VMO60146_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60146, innerException)
		{
			ErrorCode = 60146
		};
	}

	public static VmoServerServiceException VMO60147_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60147)
		{
			ErrorCode = 60147
		};
	}

	public static VmoServerServiceException VMO60147_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60147, innerException)
		{
			ErrorCode = 60147
		};
	}

	public static VmoServerServiceException VMO60148_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60148)
		{
			ErrorCode = 60148
		};
	}

	public static VmoServerServiceException VMO60148_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60148, innerException)
		{
			ErrorCode = 60148
		};
	}

	public static VmoServerServiceException VMO60149_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60149)
		{
			ErrorCode = 60149
		};
	}

	public static VmoServerServiceException VMO60149_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60149, innerException)
		{
			ErrorCode = 60149
		};
	}

	public static VmoServerServiceException VMO60150_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60150)
		{
			ErrorCode = 60150
		};
	}

	public static VmoServerServiceException VMO60150_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60150, innerException)
		{
			ErrorCode = 60150
		};
	}

	public static VmoServerServiceException VMO60151_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60151)
		{
			ErrorCode = 60151
		};
	}

	public static VmoServerServiceException VMO60151_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60151, innerException)
		{
			ErrorCode = 60151
		};
	}

	public static VmoServerServiceException VMO60152_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60152)
		{
			ErrorCode = 60152
		};
	}

	public static VmoServerServiceException VMO60152_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60152, innerException)
		{
			ErrorCode = 60152
		};
	}

	public static VmoServerServiceException VMO60153_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60153)
		{
			ErrorCode = 60153
		};
	}

	public static VmoServerServiceException VMO60153_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60153, innerException)
		{
			ErrorCode = 60153
		};
	}

	public static ActiveDirectoryAccessException VMO60154_ActiveDirectoryAccessException()
	{
		return new ActiveDirectoryAccessException(VmoResources.VMO60154)
		{
			ErrorCode = 60154
		};
	}

	public static ActiveDirectoryAccessException VMO60154_ActiveDirectoryAccessException(Exception innerException)
	{
		return new ActiveDirectoryAccessException(VmoResources.VMO60154, innerException)
		{
			ErrorCode = 60154
		};
	}

	public static ActiveDirectoryAccessException VMO60155_ActiveDirectoryAccessException(string name)
	{
		return new ActiveDirectoryAccessException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60155, name))
		{
			ErrorCode = 60155,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static ActiveDirectoryAccessException VMO60155_ActiveDirectoryAccessException(string name, Exception innerException)
	{
		return new ActiveDirectoryAccessException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60155, name), innerException)
		{
			ErrorCode = 60155,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60156_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60156, name))
		{
			ErrorCode = 60156,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60156_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60156, name), innerException)
		{
			ErrorCode = 60156,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60157_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60157)
		{
			ErrorCode = 60157
		};
	}

	public static VmoServerServiceException VMO60157_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60157, innerException)
		{
			ErrorCode = 60157
		};
	}

	public static VmoServerServiceException VMO60158_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60158)
		{
			ErrorCode = 60158
		};
	}

	public static VmoServerServiceException VMO60158_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60158, innerException)
		{
			ErrorCode = 60158
		};
	}

	public static VmoServerServiceException VMO60159_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60159)
		{
			ErrorCode = 60159
		};
	}

	public static VmoServerServiceException VMO60159_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60159, innerException)
		{
			ErrorCode = 60159
		};
	}

	public static VmoServerServiceException VMO60160_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60160, name))
		{
			ErrorCode = 60160,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60160_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60160, name), innerException)
		{
			ErrorCode = 60160,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60161_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60161)
		{
			ErrorCode = 60161
		};
	}

	public static VmoServerServiceException VMO60161_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60161, innerException)
		{
			ErrorCode = 60161
		};
	}

	public static VmoServerServiceException VMO60162_VmoServerServiceException(int maxItems)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60162, maxItems))
		{
			ErrorCode = 60162,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "maxItems", maxItems.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60162_VmoServerServiceException(int maxItems, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60162, maxItems), innerException)
		{
			ErrorCode = 60162,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "maxItems", maxItems.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60163_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60163)
		{
			ErrorCode = 60163
		};
	}

	public static VmoServerServiceException VMO60163_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60163, innerException)
		{
			ErrorCode = 60163
		};
	}

	public static VmoServerServiceException VMO60164_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60164)
		{
			ErrorCode = 60164
		};
	}

	public static VmoServerServiceException VMO60164_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60164, innerException)
		{
			ErrorCode = 60164
		};
	}

	public static VmoServerServiceException VMO60165_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60165)
		{
			ErrorCode = 60165
		};
	}

	public static VmoServerServiceException VMO60165_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60165, innerException)
		{
			ErrorCode = 60165
		};
	}

	public static VmoServerServiceException VMO60166_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60166)
		{
			ErrorCode = 60166
		};
	}

	public static VmoServerServiceException VMO60166_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60166, innerException)
		{
			ErrorCode = 60166
		};
	}

	public static VmoServerServiceException VMO60167_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60167)
		{
			ErrorCode = 60167
		};
	}

	public static VmoServerServiceException VMO60167_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60167, innerException)
		{
			ErrorCode = 60167
		};
	}

	public static VmoServerServiceException VMO60168_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60168)
		{
			ErrorCode = 60168
		};
	}

	public static VmoServerServiceException VMO60168_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60168, innerException)
		{
			ErrorCode = 60168
		};
	}

	public static VmoServerServiceException VMO60169_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60169)
		{
			ErrorCode = 60169
		};
	}

	public static VmoServerServiceException VMO60169_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60169, innerException)
		{
			ErrorCode = 60169
		};
	}

	public static VmoServerServiceException VMO60174_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60174)
		{
			ErrorCode = 60174
		};
	}

	public static VmoServerServiceException VMO60174_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60174, innerException)
		{
			ErrorCode = 60174
		};
	}

	public static VmoServerServiceException VMO60175_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60175)
		{
			ErrorCode = 60175
		};
	}

	public static VmoServerServiceException VMO60175_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60175, innerException)
		{
			ErrorCode = 60175
		};
	}

	public static VmoServerServiceException VMO60176_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60176)
		{
			ErrorCode = 60176
		};
	}

	public static VmoServerServiceException VMO60176_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60176, innerException)
		{
			ErrorCode = 60176
		};
	}

	public static VmoServerServiceException VMO60177_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60177)
		{
			ErrorCode = 60177
		};
	}

	public static VmoServerServiceException VMO60177_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60177, innerException)
		{
			ErrorCode = 60177
		};
	}

	public static VmoServerServiceException VMO60178_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60178)
		{
			ErrorCode = 60178
		};
	}

	public static VmoServerServiceException VMO60178_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60178, innerException)
		{
			ErrorCode = 60178
		};
	}

	public static VmoServerServiceException VMO60179_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60179)
		{
			ErrorCode = 60179
		};
	}

	public static VmoServerServiceException VMO60179_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60179, innerException)
		{
			ErrorCode = 60179
		};
	}

	public static VmoServerServiceException VMO60180_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60180)
		{
			ErrorCode = 60180
		};
	}

	public static VmoServerServiceException VMO60180_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60180, innerException)
		{
			ErrorCode = 60180
		};
	}

	public static VmoServerServiceException VMO60181_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60181)
		{
			ErrorCode = 60181
		};
	}

	public static VmoServerServiceException VMO60181_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60181, innerException)
		{
			ErrorCode = 60181
		};
	}

	public static VmoServerServiceException VMO60182_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60182)
		{
			ErrorCode = 60182
		};
	}

	public static VmoServerServiceException VMO60182_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60182, innerException)
		{
			ErrorCode = 60182
		};
	}

	public static VmoServerServiceException VMO60183_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60183)
		{
			ErrorCode = 60183
		};
	}

	public static VmoServerServiceException VMO60183_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60183, innerException)
		{
			ErrorCode = 60183
		};
	}

	public static VmoServerServiceException VMO60184_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60184)
		{
			ErrorCode = 60184
		};
	}

	public static VmoServerServiceException VMO60184_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60184, innerException)
		{
			ErrorCode = 60184
		};
	}

	public static VmoServerServiceException VMO60185_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60185)
		{
			ErrorCode = 60185
		};
	}

	public static VmoServerServiceException VMO60185_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60185, innerException)
		{
			ErrorCode = 60185
		};
	}

	public static VmoServerServiceException VMO60186_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60186)
		{
			ErrorCode = 60186
		};
	}

	public static VmoServerServiceException VMO60186_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60186, innerException)
		{
			ErrorCode = 60186
		};
	}

	public static VmoServerServiceException VMO60187_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60187)
		{
			ErrorCode = 60187
		};
	}

	public static VmoServerServiceException VMO60187_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60187, innerException)
		{
			ErrorCode = 60187
		};
	}

	public static VmoServerServiceException VMO60188_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60188)
		{
			ErrorCode = 60188
		};
	}

	public static VmoServerServiceException VMO60188_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60188, innerException)
		{
			ErrorCode = 60188
		};
	}

	public static VmoServerServiceException VMO60189_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60189)
		{
			ErrorCode = 60189
		};
	}

	public static VmoServerServiceException VMO60189_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60189, innerException)
		{
			ErrorCode = 60189
		};
	}

	public static VmoServerServiceException VMO60190_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60190)
		{
			ErrorCode = 60190
		};
	}

	public static VmoServerServiceException VMO60190_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60190, innerException)
		{
			ErrorCode = 60190
		};
	}

	public static VmoServerServiceException VMO60191_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60191)
		{
			ErrorCode = 60191
		};
	}

	public static VmoServerServiceException VMO60191_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60191, innerException)
		{
			ErrorCode = 60191
		};
	}

	public static VmoServerServiceException VMO60192_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60192)
		{
			ErrorCode = 60192
		};
	}

	public static VmoServerServiceException VMO60192_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60192, innerException)
		{
			ErrorCode = 60192
		};
	}

	public static VmoServerServiceException VMO60193_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60193, name))
		{
			ErrorCode = 60193,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60193_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60193, name), innerException)
		{
			ErrorCode = 60193,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60194_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60194, name))
		{
			ErrorCode = 60194,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60194_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60194, name), innerException)
		{
			ErrorCode = 60194,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60195_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60195, name))
		{
			ErrorCode = 60195,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60195_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60195, name), innerException)
		{
			ErrorCode = 60195,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60196_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60196, name))
		{
			ErrorCode = 60196,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60196_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60196, name), innerException)
		{
			ErrorCode = 60196,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60197_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60197)
		{
			ErrorCode = 60197
		};
	}

	public static VmoServerServiceException VMO60197_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60197, innerException)
		{
			ErrorCode = 60197
		};
	}

	public static VmoServerServiceException VMO60198_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60198)
		{
			ErrorCode = 60198
		};
	}

	public static VmoServerServiceException VMO60198_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60198, innerException)
		{
			ErrorCode = 60198
		};
	}

	public static VmoServerServiceException VMO60199_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60199)
		{
			ErrorCode = 60199
		};
	}

	public static VmoServerServiceException VMO60199_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60199, innerException)
		{
			ErrorCode = 60199
		};
	}

	public static VmoServerServiceException VMO60200_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60200, name))
		{
			ErrorCode = 60200,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60200_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60200, name), innerException)
		{
			ErrorCode = 60200,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60201_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60201)
		{
			ErrorCode = 60201
		};
	}

	public static VmoServerServiceException VMO60201_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60201, innerException)
		{
			ErrorCode = 60201
		};
	}

	public static VmoServerServiceException VMO60202_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60202, name))
		{
			ErrorCode = 60202,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60202_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60202, name), innerException)
		{
			ErrorCode = 60202,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60203_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60203)
		{
			ErrorCode = 60203
		};
	}

	public static VmoServerServiceException VMO60203_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60203, innerException)
		{
			ErrorCode = 60203
		};
	}

	public static VmoServerServiceException VMO60204_VmoServerServiceException(string name, string videoWallName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60204, name, videoWallName))
		{
			ErrorCode = 60204,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60204_VmoServerServiceException(string name, string videoWallName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60204, name, videoWallName), innerException)
		{
			ErrorCode = 60204,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60205_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60205)
		{
			ErrorCode = 60205
		};
	}

	public static VmoServerServiceException VMO60205_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60205, innerException)
		{
			ErrorCode = 60205
		};
	}

	public static VmoServerServiceException VMO60206_VmoServerServiceException(string name, string videoWallName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60206, name, videoWallName))
		{
			ErrorCode = 60206,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60206_VmoServerServiceException(string name, string videoWallName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60206, name, videoWallName), innerException)
		{
			ErrorCode = 60206,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60207_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60207)
		{
			ErrorCode = 60207
		};
	}

	public static VmoServerServiceException VMO60207_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60207, innerException)
		{
			ErrorCode = 60207
		};
	}

	public static VmoServerServiceException VMO60208_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60208)
		{
			ErrorCode = 60208
		};
	}

	public static VmoServerServiceException VMO60208_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60208, innerException)
		{
			ErrorCode = 60208
		};
	}

	public static VmoServerServiceException VMO60209_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60209)
		{
			ErrorCode = 60209
		};
	}

	public static VmoServerServiceException VMO60209_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60209, innerException)
		{
			ErrorCode = 60209
		};
	}

	public static VmoServerServiceException VMO60210_VmoServerServiceException(string name, string videoWallName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60210, name, videoWallName))
		{
			ErrorCode = 60210,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60210_VmoServerServiceException(string name, string videoWallName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60210, name, videoWallName), innerException)
		{
			ErrorCode = 60210,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60211_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60211)
		{
			ErrorCode = 60211
		};
	}

	public static VmoServerServiceException VMO60211_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60211, innerException)
		{
			ErrorCode = 60211
		};
	}

	public static VmoServerServiceException VMO60212_VmoServerServiceException(string name, string videoWallName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60212, name, videoWallName))
		{
			ErrorCode = 60212,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60212_VmoServerServiceException(string name, string videoWallName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60212, name, videoWallName), innerException)
		{
			ErrorCode = 60212,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "name", name)
				},
				{
					1,
					new MessageParameter(1, "videoWallName", videoWallName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60213_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60213)
		{
			ErrorCode = 60213
		};
	}

	public static VmoServerServiceException VMO60213_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60213, innerException)
		{
			ErrorCode = 60213
		};
	}

	public static VmoServerServiceException VMO60214_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60214, name))
		{
			ErrorCode = 60214,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60214_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60214, name), innerException)
		{
			ErrorCode = 60214,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60215_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60215)
		{
			ErrorCode = 60215
		};
	}

	public static VmoServerServiceException VMO60215_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60215, innerException)
		{
			ErrorCode = 60215
		};
	}

	public static VmoServerServiceException VMO60216_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60216, name))
		{
			ErrorCode = 60216,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60216_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60216, name), innerException)
		{
			ErrorCode = 60216,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60217_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60217)
		{
			ErrorCode = 60217
		};
	}

	public static VmoServerServiceException VMO60217_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60217, innerException)
		{
			ErrorCode = 60217
		};
	}

	public static VmoServerServiceException VMO60218_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60218, id))
		{
			ErrorCode = 60218,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO60218_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60218, id), innerException)
		{
			ErrorCode = 60218,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO60219_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60219, id))
		{
			ErrorCode = 60219,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO60219_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60219, id), innerException)
		{
			ErrorCode = 60219,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO60220_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60220)
		{
			ErrorCode = 60220
		};
	}

	public static VmoServerServiceException VMO60220_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60220, innerException)
		{
			ErrorCode = 60220
		};
	}

	public static VmoServerServiceException VMO60221_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60221)
		{
			ErrorCode = 60221
		};
	}

	public static VmoServerServiceException VMO60221_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60221, innerException)
		{
			ErrorCode = 60221
		};
	}

	public static VmoServerServiceException VMO60222_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60222, name))
		{
			ErrorCode = 60222,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60222_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60222, name), innerException)
		{
			ErrorCode = 60222,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60223_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60223)
		{
			ErrorCode = 60223
		};
	}

	public static VmoServerServiceException VMO60223_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60223, innerException)
		{
			ErrorCode = 60223
		};
	}

	public static VmoServerServiceException VMO60224_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60224, name))
		{
			ErrorCode = 60224,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60224_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60224, name), innerException)
		{
			ErrorCode = 60224,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60225_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60225)
		{
			ErrorCode = 60225
		};
	}

	public static VmoServerServiceException VMO60225_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60225, innerException)
		{
			ErrorCode = 60225
		};
	}

	public static VmoServerServiceException VMO60226_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60226)
		{
			ErrorCode = 60226
		};
	}

	public static VmoServerServiceException VMO60226_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60226, innerException)
		{
			ErrorCode = 60226
		};
	}

	public static VmoServerServiceException VMO60227_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60227)
		{
			ErrorCode = 60227
		};
	}

	public static VmoServerServiceException VMO60227_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60227, innerException)
		{
			ErrorCode = 60227
		};
	}

	public static VmoServerServiceException VMO60228_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60228)
		{
			ErrorCode = 60228
		};
	}

	public static VmoServerServiceException VMO60228_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60228, innerException)
		{
			ErrorCode = 60228
		};
	}

	public static DeviceStreamConfigurationException VMO60229_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60229)
		{
			ErrorCode = 60229
		};
	}

	public static DeviceStreamConfigurationException VMO60229_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60229, innerException)
		{
			ErrorCode = 60229
		};
	}

	public static DeviceStreamConfigurationException VMO60230_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60230)
		{
			ErrorCode = 60230
		};
	}

	public static DeviceStreamConfigurationException VMO60230_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60230, innerException)
		{
			ErrorCode = 60230
		};
	}

	public static DeviceStreamConfigurationException VMO60231_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60231)
		{
			ErrorCode = 60231
		};
	}

	public static DeviceStreamConfigurationException VMO60231_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60231, innerException)
		{
			ErrorCode = 60231
		};
	}

	public static DeviceStreamConfigurationException VMO60232_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60232)
		{
			ErrorCode = 60232
		};
	}

	public static DeviceStreamConfigurationException VMO60232_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60232, innerException)
		{
			ErrorCode = 60232
		};
	}

	public static DeviceStreamConfigurationException VMO60233_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60233)
		{
			ErrorCode = 60233
		};
	}

	public static DeviceStreamConfigurationException VMO60233_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60233, innerException)
		{
			ErrorCode = 60233
		};
	}

	public static DeviceStreamConfigurationException VMO60234_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60234)
		{
			ErrorCode = 60234
		};
	}

	public static DeviceStreamConfigurationException VMO60234_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60234, innerException)
		{
			ErrorCode = 60234
		};
	}

	public static DeviceStreamConfigurationException VMO60235_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60235)
		{
			ErrorCode = 60235
		};
	}

	public static DeviceStreamConfigurationException VMO60235_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60235, innerException)
		{
			ErrorCode = 60235
		};
	}

	public static DeviceStreamConfigurationException VMO60236_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60236)
		{
			ErrorCode = 60236
		};
	}

	public static DeviceStreamConfigurationException VMO60236_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60236, innerException)
		{
			ErrorCode = 60236
		};
	}

	public static VmoServerServiceException VMO60237_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60237)
		{
			ErrorCode = 60237
		};
	}

	public static VmoServerServiceException VMO60237_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60237, innerException)
		{
			ErrorCode = 60237
		};
	}

	public static VmoServerServiceException VMO60238_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60238)
		{
			ErrorCode = 60238
		};
	}

	public static VmoServerServiceException VMO60238_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60238, innerException)
		{
			ErrorCode = 60238
		};
	}

	public static VmoServerServiceException VMO60239_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60239)
		{
			ErrorCode = 60239
		};
	}

	public static VmoServerServiceException VMO60239_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60239, innerException)
		{
			ErrorCode = 60239
		};
	}

	public static DeviceStreamConfigurationException VMO60240_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60240)
		{
			ErrorCode = 60240
		};
	}

	public static DeviceStreamConfigurationException VMO60240_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60240, innerException)
		{
			ErrorCode = 60240
		};
	}

	public static DeviceStreamConfigurationException VMO60241_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60241)
		{
			ErrorCode = 60241
		};
	}

	public static DeviceStreamConfigurationException VMO60241_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60241, innerException)
		{
			ErrorCode = 60241
		};
	}

	public static VmoServerServiceException VMO60242_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60242)
		{
			ErrorCode = 60242
		};
	}

	public static VmoServerServiceException VMO60242_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60242, innerException)
		{
			ErrorCode = 60242
		};
	}

	public static VmoServerServiceException VMO60243_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60243)
		{
			ErrorCode = 60243
		};
	}

	public static VmoServerServiceException VMO60243_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60243, innerException)
		{
			ErrorCode = 60243
		};
	}

	public static VmoServerServiceException VMO60244_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60244)
		{
			ErrorCode = 60244
		};
	}

	public static VmoServerServiceException VMO60244_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60244, innerException)
		{
			ErrorCode = 60244
		};
	}

	public static VmoServerServiceException VMO60245_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60245)
		{
			ErrorCode = 60245
		};
	}

	public static VmoServerServiceException VMO60245_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60245, innerException)
		{
			ErrorCode = 60245
		};
	}

	public static VmoServerServiceException VMO60246_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60246)
		{
			ErrorCode = 60246
		};
	}

	public static VmoServerServiceException VMO60246_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60246, innerException)
		{
			ErrorCode = 60246
		};
	}

	public static VmoServerServiceException VMO60247_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60247)
		{
			ErrorCode = 60247
		};
	}

	public static VmoServerServiceException VMO60247_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60247, innerException)
		{
			ErrorCode = 60247
		};
	}

	public static VmoServerServiceException VMO60248_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60248)
		{
			ErrorCode = 60248
		};
	}

	public static VmoServerServiceException VMO60248_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60248, innerException)
		{
			ErrorCode = 60248
		};
	}

	public static VmoServerServiceException VMO60249_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60249)
		{
			ErrorCode = 60249
		};
	}

	public static VmoServerServiceException VMO60249_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60249, innerException)
		{
			ErrorCode = 60249
		};
	}

	public static VmoServerServiceException VMO60250_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60250)
		{
			ErrorCode = 60250
		};
	}

	public static VmoServerServiceException VMO60250_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60250, innerException)
		{
			ErrorCode = 60250
		};
	}

	public static VmoServerServiceException VMO60251_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60251)
		{
			ErrorCode = 60251
		};
	}

	public static VmoServerServiceException VMO60251_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60251, innerException)
		{
			ErrorCode = 60251
		};
	}

	public static VmoServerServiceException VMO60252_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60252)
		{
			ErrorCode = 60252
		};
	}

	public static VmoServerServiceException VMO60252_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60252, innerException)
		{
			ErrorCode = 60252
		};
	}

	public static VmoServerServiceException VMO60253_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60253, name))
		{
			ErrorCode = 60253,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60253_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60253, name), innerException)
		{
			ErrorCode = 60253,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60254_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60254)
		{
			ErrorCode = 60254
		};
	}

	public static VmoServerServiceException VMO60254_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60254, innerException)
		{
			ErrorCode = 60254
		};
	}

	public static VmoServerServiceException VMO60255_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60255)
		{
			ErrorCode = 60255
		};
	}

	public static VmoServerServiceException VMO60255_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60255, innerException)
		{
			ErrorCode = 60255
		};
	}

	public static VmoServerServiceException VMO60256_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60256)
		{
			ErrorCode = 60256
		};
	}

	public static VmoServerServiceException VMO60256_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60256, innerException)
		{
			ErrorCode = 60256
		};
	}

	public static VmoServerServiceException VMO60257_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60257)
		{
			ErrorCode = 60257
		};
	}

	public static VmoServerServiceException VMO60257_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60257, innerException)
		{
			ErrorCode = 60257
		};
	}

	public static VmoServerServiceException VMO60258_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60258)
		{
			ErrorCode = 60258
		};
	}

	public static VmoServerServiceException VMO60258_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60258, innerException)
		{
			ErrorCode = 60258
		};
	}

	public static VmoServerServiceException VMO60259_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60259)
		{
			ErrorCode = 60259
		};
	}

	public static VmoServerServiceException VMO60259_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60259, innerException)
		{
			ErrorCode = 60259
		};
	}

	public static VmoServerServiceException VMO60260_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60260)
		{
			ErrorCode = 60260
		};
	}

	public static VmoServerServiceException VMO60260_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60260, innerException)
		{
			ErrorCode = 60260
		};
	}

	public static VmoServerServiceException VMO60261_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60261)
		{
			ErrorCode = 60261
		};
	}

	public static VmoServerServiceException VMO60261_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60261, innerException)
		{
			ErrorCode = 60261
		};
	}

	public static VmoServerServiceException VMO60262_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60262)
		{
			ErrorCode = 60262
		};
	}

	public static VmoServerServiceException VMO60262_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60262, innerException)
		{
			ErrorCode = 60262
		};
	}

	public static VmoServerServiceException VMO60263_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60263)
		{
			ErrorCode = 60263
		};
	}

	public static VmoServerServiceException VMO60263_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60263, innerException)
		{
			ErrorCode = 60263
		};
	}

	public static VmoServerServiceException VMO60264_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60264)
		{
			ErrorCode = 60264
		};
	}

	public static VmoServerServiceException VMO60264_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60264, innerException)
		{
			ErrorCode = 60264
		};
	}

	public static VmoServerServiceException VMO60265_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60265)
		{
			ErrorCode = 60265
		};
	}

	public static VmoServerServiceException VMO60265_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60265, innerException)
		{
			ErrorCode = 60265
		};
	}

	public static VmoServerServiceException VMO60266_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60266)
		{
			ErrorCode = 60266
		};
	}

	public static VmoServerServiceException VMO60266_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60266, innerException)
		{
			ErrorCode = 60266
		};
	}

	public static VmoServerServiceException VMO60267_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60267)
		{
			ErrorCode = 60267
		};
	}

	public static VmoServerServiceException VMO60267_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60267, innerException)
		{
			ErrorCode = 60267
		};
	}

	public static VmoServerServiceException VMO60268_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60268)
		{
			ErrorCode = 60268
		};
	}

	public static VmoServerServiceException VMO60268_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60268, innerException)
		{
			ErrorCode = 60268
		};
	}

	public static VmoServerServiceException VMO60269_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60269)
		{
			ErrorCode = 60269
		};
	}

	public static VmoServerServiceException VMO60269_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60269, innerException)
		{
			ErrorCode = 60269
		};
	}

	public static VmoServerServiceException VMO60270_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60270)
		{
			ErrorCode = 60270
		};
	}

	public static VmoServerServiceException VMO60270_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60270, innerException)
		{
			ErrorCode = 60270
		};
	}

	public static VmoServerServiceException VMO60271_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60271)
		{
			ErrorCode = 60271
		};
	}

	public static VmoServerServiceException VMO60271_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60271, innerException)
		{
			ErrorCode = 60271
		};
	}

	public static VmoServerServiceException VMO60272_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60272)
		{
			ErrorCode = 60272
		};
	}

	public static VmoServerServiceException VMO60272_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60272, innerException)
		{
			ErrorCode = 60272
		};
	}

	public static VmoServerServiceException VMO60273_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60273)
		{
			ErrorCode = 60273
		};
	}

	public static VmoServerServiceException VMO60273_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60273, innerException)
		{
			ErrorCode = 60273
		};
	}

	public static VmoServerServiceException VMO60274_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60274)
		{
			ErrorCode = 60274
		};
	}

	public static VmoServerServiceException VMO60274_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60274, innerException)
		{
			ErrorCode = 60274
		};
	}

	public static VmoServerServiceException VMO60275_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60275)
		{
			ErrorCode = 60275
		};
	}

	public static VmoServerServiceException VMO60275_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60275, innerException)
		{
			ErrorCode = 60275
		};
	}

	public static VmoServerServiceException VMO60276_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60276)
		{
			ErrorCode = 60276
		};
	}

	public static VmoServerServiceException VMO60276_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60276, innerException)
		{
			ErrorCode = 60276
		};
	}

	public static VmoServerServiceException VMO60277_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60277)
		{
			ErrorCode = 60277
		};
	}

	public static VmoServerServiceException VMO60277_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60277, innerException)
		{
			ErrorCode = 60277
		};
	}

	public static VmoServerServiceException VMO60278_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60278)
		{
			ErrorCode = 60278
		};
	}

	public static VmoServerServiceException VMO60278_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60278, innerException)
		{
			ErrorCode = 60278
		};
	}

	public static VmoServerServiceException VMO60279_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60279)
		{
			ErrorCode = 60279
		};
	}

	public static VmoServerServiceException VMO60279_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60279, innerException)
		{
			ErrorCode = 60279
		};
	}

	public static VmoServerServiceException VMO60280_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60280)
		{
			ErrorCode = 60280
		};
	}

	public static VmoServerServiceException VMO60280_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60280, innerException)
		{
			ErrorCode = 60280
		};
	}

	public static VmoServerServiceException VMO60281_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60281)
		{
			ErrorCode = 60281
		};
	}

	public static VmoServerServiceException VMO60281_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60281, innerException)
		{
			ErrorCode = 60281
		};
	}

	public static VmoServerServiceException VMO60282_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60282)
		{
			ErrorCode = 60282
		};
	}

	public static VmoServerServiceException VMO60282_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60282, innerException)
		{
			ErrorCode = 60282
		};
	}

	public static VmoServerServiceException VMO60283_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60283)
		{
			ErrorCode = 60283
		};
	}

	public static VmoServerServiceException VMO60283_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60283, innerException)
		{
			ErrorCode = 60283
		};
	}

	public static VmoServerServiceException VMO60284_VmoServerServiceException(string message)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60284, message))
		{
			ErrorCode = 60284,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Message", message)
			} }
		};
	}

	public static VmoServerServiceException VMO60284_VmoServerServiceException(string message, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60284, message), innerException)
		{
			ErrorCode = 60284,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Message", message)
			} }
		};
	}

	public static VmoServerServiceException VMO60285_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60285)
		{
			ErrorCode = 60285
		};
	}

	public static VmoServerServiceException VMO60285_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60285, innerException)
		{
			ErrorCode = 60285
		};
	}

	public static VmoServerServiceException VMO60286_VmoServerServiceException(string productName, string productVersion)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60286, productName, productVersion))
		{
			ErrorCode = 60286,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60286_VmoServerServiceException(string productName, string productVersion, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60286, productName, productVersion), innerException)
		{
			ErrorCode = 60286,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60287_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60287)
		{
			ErrorCode = 60287
		};
	}

	public static VmoServerServiceException VMO60287_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60287, innerException)
		{
			ErrorCode = 60287
		};
	}

	public static VmoServerServiceException VMO60288_VmoServerServiceException(string productName, string productVersion)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60288, productName, productVersion))
		{
			ErrorCode = 60288,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60288_VmoServerServiceException(string productName, string productVersion, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60288, productName, productVersion), innerException)
		{
			ErrorCode = 60288,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60289_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60289)
		{
			ErrorCode = 60289
		};
	}

	public static VmoServerServiceException VMO60289_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60289, innerException)
		{
			ErrorCode = 60289
		};
	}

	public static VmoServerServiceException VMO60290_VmoServerServiceException(string productName, string productVersion)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60290, productName, productVersion))
		{
			ErrorCode = 60290,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60290_VmoServerServiceException(string productName, string productVersion, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60290, productName, productVersion), innerException)
		{
			ErrorCode = 60290,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60291_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60291)
		{
			ErrorCode = 60291
		};
	}

	public static VmoServerServiceException VMO60291_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60291, innerException)
		{
			ErrorCode = 60291
		};
	}

	public static VmoServerServiceException VMO60292_VmoServerServiceException(string productName, string productVersion)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60292, productName, productVersion))
		{
			ErrorCode = 60292,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60292_VmoServerServiceException(string productName, string productVersion, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60292, productName, productVersion), innerException)
		{
			ErrorCode = 60292,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "ProductName", productName)
				},
				{
					1,
					new MessageParameter(1, "ProductVersion", productVersion)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60293_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60293)
		{
			ErrorCode = 60293
		};
	}

	public static VmoServerServiceException VMO60293_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60293, innerException)
		{
			ErrorCode = 60293
		};
	}

	public static VmoServerServiceException VMO60294_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60294)
		{
			ErrorCode = 60294
		};
	}

	public static VmoServerServiceException VMO60294_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60294, innerException)
		{
			ErrorCode = 60294
		};
	}

	public static VmoServerServiceException VMO60295_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60295)
		{
			ErrorCode = 60295
		};
	}

	public static VmoServerServiceException VMO60295_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60295, innerException)
		{
			ErrorCode = 60295
		};
	}

	public static VmoServerServiceException VMO60296_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60296)
		{
			ErrorCode = 60296
		};
	}

	public static VmoServerServiceException VMO60296_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60296, innerException)
		{
			ErrorCode = 60296
		};
	}

	public static VmoServerServiceException VMO60297_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60297)
		{
			ErrorCode = 60297
		};
	}

	public static VmoServerServiceException VMO60297_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60297, innerException)
		{
			ErrorCode = 60297
		};
	}

	public static VmoServerServiceException VMO60298_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60298)
		{
			ErrorCode = 60298
		};
	}

	public static VmoServerServiceException VMO60298_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60298, innerException)
		{
			ErrorCode = 60298
		};
	}

	public static VmoServerServiceException VMO60299_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60299)
		{
			ErrorCode = 60299
		};
	}

	public static VmoServerServiceException VMO60299_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60299, innerException)
		{
			ErrorCode = 60299
		};
	}

	public static VmoServerServiceException VMO60300_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60300)
		{
			ErrorCode = 60300
		};
	}

	public static VmoServerServiceException VMO60300_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60300, innerException)
		{
			ErrorCode = 60300
		};
	}

	public static VmoServerServiceException VMO60301_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60301)
		{
			ErrorCode = 60301
		};
	}

	public static VmoServerServiceException VMO60301_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60301, innerException)
		{
			ErrorCode = 60301
		};
	}

	public static VmoServerServiceException VMO60302_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60302)
		{
			ErrorCode = 60302
		};
	}

	public static VmoServerServiceException VMO60302_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60302, innerException)
		{
			ErrorCode = 60302
		};
	}

	public static VmoServerServiceException VMO60303_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60303)
		{
			ErrorCode = 60303
		};
	}

	public static VmoServerServiceException VMO60303_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60303, innerException)
		{
			ErrorCode = 60303
		};
	}

	public static VmoServerServiceException VMO60304_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60304)
		{
			ErrorCode = 60304
		};
	}

	public static VmoServerServiceException VMO60304_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60304, innerException)
		{
			ErrorCode = 60304
		};
	}

	public static VmoServerServiceException VMO60305_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60305)
		{
			ErrorCode = 60305
		};
	}

	public static VmoServerServiceException VMO60305_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60305, innerException)
		{
			ErrorCode = 60305
		};
	}

	public static VmoServerServiceException VMO60306_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60306)
		{
			ErrorCode = 60306
		};
	}

	public static VmoServerServiceException VMO60306_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60306, innerException)
		{
			ErrorCode = 60306
		};
	}

	public static VmoServerServiceException VMO60307_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60307)
		{
			ErrorCode = 60307
		};
	}

	public static VmoServerServiceException VMO60307_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60307, innerException)
		{
			ErrorCode = 60307
		};
	}

	public static VmoServerServiceException VMO60308_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60308)
		{
			ErrorCode = 60308
		};
	}

	public static VmoServerServiceException VMO60308_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60308, innerException)
		{
			ErrorCode = 60308
		};
	}

	public static VmoServerServiceException VMO60309_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60309)
		{
			ErrorCode = 60309
		};
	}

	public static VmoServerServiceException VMO60309_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60309, innerException)
		{
			ErrorCode = 60309
		};
	}

	public static VmoServerServiceException VMO60310_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60310)
		{
			ErrorCode = 60310
		};
	}

	public static VmoServerServiceException VMO60310_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60310, innerException)
		{
			ErrorCode = 60310
		};
	}

	public static VmoFunctionNotSupportedException VMO60311_VmoFunctionNotSupportedException(string functionName)
	{
		return new VmoFunctionNotSupportedException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60311, functionName))
		{
			ErrorCode = 60311,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "FunctionName", functionName)
			} }
		};
	}

	public static VmoFunctionNotSupportedException VMO60311_VmoFunctionNotSupportedException(string functionName, Exception innerException)
	{
		return new VmoFunctionNotSupportedException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60311, functionName), innerException)
		{
			ErrorCode = 60311,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "FunctionName", functionName)
			} }
		};
	}

	public static VmoServerServiceException VMO60312_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60312)
		{
			ErrorCode = 60312
		};
	}

	public static VmoServerServiceException VMO60312_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60312, innerException)
		{
			ErrorCode = 60312
		};
	}

	public static VmoServerServiceException VMO60313_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60313)
		{
			ErrorCode = 60313
		};
	}

	public static VmoServerServiceException VMO60313_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60313, innerException)
		{
			ErrorCode = 60313
		};
	}

	public static VmoServerServiceException VMO60314_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60314)
		{
			ErrorCode = 60314
		};
	}

	public static VmoServerServiceException VMO60314_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60314, innerException)
		{
			ErrorCode = 60314
		};
	}

	public static VmoServerServiceException VMO60315_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60315)
		{
			ErrorCode = 60315
		};
	}

	public static VmoServerServiceException VMO60315_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60315, innerException)
		{
			ErrorCode = 60315
		};
	}

	public static ObjectNotFoundException VMO60316_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60316)
		{
			ErrorCode = 60316
		};
	}

	public static ObjectNotFoundException VMO60316_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60316, innerException)
		{
			ErrorCode = 60316
		};
	}

	public static VmoServerServiceException VMO60317_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60317)
		{
			ErrorCode = 60317
		};
	}

	public static VmoServerServiceException VMO60317_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60317, innerException)
		{
			ErrorCode = 60317
		};
	}

	public static VmoServerServiceException VMO60318_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60318)
		{
			ErrorCode = 60318
		};
	}

	public static VmoServerServiceException VMO60318_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60318, innerException)
		{
			ErrorCode = 60318
		};
	}

	public static VmoServerServiceException VMO60319_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60319)
		{
			ErrorCode = 60319
		};
	}

	public static VmoServerServiceException VMO60319_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60319, innerException)
		{
			ErrorCode = 60319
		};
	}

	public static DevicePtzPresetNameKeyConflictException VMO60320_DevicePtzPresetNameKeyConflictException(string name)
	{
		return new DevicePtzPresetNameKeyConflictException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60320, name))
		{
			ErrorCode = 60320,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static DevicePtzPresetNameKeyConflictException VMO60320_DevicePtzPresetNameKeyConflictException(string name, Exception innerException)
	{
		return new DevicePtzPresetNameKeyConflictException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60320, name), innerException)
		{
			ErrorCode = 60320,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60321_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60321)
		{
			ErrorCode = 60321
		};
	}

	public static VmoServerServiceException VMO60321_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60321, innerException)
		{
			ErrorCode = 60321
		};
	}

	public static VmoServerServiceException VMO60322_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60322)
		{
			ErrorCode = 60322
		};
	}

	public static VmoServerServiceException VMO60322_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60322, innerException)
		{
			ErrorCode = 60322
		};
	}

	public static VmoServerServiceException VMO60323_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60323)
		{
			ErrorCode = 60323
		};
	}

	public static VmoServerServiceException VMO60323_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60323, innerException)
		{
			ErrorCode = 60323
		};
	}

	public static VmoServerServiceException VMO60324_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60324)
		{
			ErrorCode = 60324
		};
	}

	public static VmoServerServiceException VMO60324_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60324, innerException)
		{
			ErrorCode = 60324
		};
	}

	public static VmoServerServiceException VMO60325_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60325)
		{
			ErrorCode = 60325
		};
	}

	public static VmoServerServiceException VMO60325_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60325, innerException)
		{
			ErrorCode = 60325
		};
	}

	public static VmoServerServiceException VMO60326_VmoServerServiceException(string hardwareName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60326, hardwareName))
		{
			ErrorCode = 60326,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "HardwareName", hardwareName)
			} }
		};
	}

	public static VmoServerServiceException VMO60326_VmoServerServiceException(string hardwareName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60326, hardwareName), innerException)
		{
			ErrorCode = 60326,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "HardwareName", hardwareName)
			} }
		};
	}

	public static VmoServerServiceException VMO60327_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60327)
		{
			ErrorCode = 60327
		};
	}

	public static VmoServerServiceException VMO60327_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60327, innerException)
		{
			ErrorCode = 60327
		};
	}

	public static VmoServerServiceException VMO60328_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60328)
		{
			ErrorCode = 60328
		};
	}

	public static VmoServerServiceException VMO60328_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60328, innerException)
		{
			ErrorCode = 60328
		};
	}

	public static VmoServerServiceException VMO60329_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60329)
		{
			ErrorCode = 60329
		};
	}

	public static VmoServerServiceException VMO60329_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60329, innerException)
		{
			ErrorCode = 60329
		};
	}

	public static VmoServerServiceException VMO60330_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60330)
		{
			ErrorCode = 60330
		};
	}

	public static VmoServerServiceException VMO60330_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60330, innerException)
		{
			ErrorCode = 60330
		};
	}

	public static VmoServerServiceException VMO60331_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60331)
		{
			ErrorCode = 60331
		};
	}

	public static VmoServerServiceException VMO60331_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60331, innerException)
		{
			ErrorCode = 60331
		};
	}

	public static VmoServerServiceException VMO60332_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60332)
		{
			ErrorCode = 60332
		};
	}

	public static VmoServerServiceException VMO60332_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60332, innerException)
		{
			ErrorCode = 60332
		};
	}

	public static VmoServerServiceException VMO60333_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60333)
		{
			ErrorCode = 60333
		};
	}

	public static VmoServerServiceException VMO60333_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60333, innerException)
		{
			ErrorCode = 60333
		};
	}

	public static VmoServerServiceException VMO60334_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60334)
		{
			ErrorCode = 60334
		};
	}

	public static VmoServerServiceException VMO60334_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60334, innerException)
		{
			ErrorCode = 60334
		};
	}

	public static VmoServerServiceException VMO60335_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60335, name))
		{
			ErrorCode = 60335,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60335_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60335, name), innerException)
		{
			ErrorCode = 60335,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60336_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60336)
		{
			ErrorCode = 60336
		};
	}

	public static VmoServerServiceException VMO60336_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60336, innerException)
		{
			ErrorCode = 60336
		};
	}

	public static VmoServerServiceException VMO60337_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60337)
		{
			ErrorCode = 60337
		};
	}

	public static VmoServerServiceException VMO60337_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60337, innerException)
		{
			ErrorCode = 60337
		};
	}

	public static VmoServerServiceException VMO60338_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60338)
		{
			ErrorCode = 60338
		};
	}

	public static VmoServerServiceException VMO60338_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60338, innerException)
		{
			ErrorCode = 60338
		};
	}

	public static VmoServerServiceException VMO60339_VmoServerServiceException(string idHardware, string deviceType, string deviceIndex)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60339, idHardware, deviceType, deviceIndex))
		{
			ErrorCode = 60339,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDHardware", idHardware)
				},
				{
					1,
					new MessageParameter(1, "DeviceType", deviceType)
				},
				{
					2,
					new MessageParameter(2, "DeviceIndex", deviceIndex)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60339_VmoServerServiceException(string idHardware, string deviceType, string deviceIndex, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60339, idHardware, deviceType, deviceIndex), innerException)
		{
			ErrorCode = 60339,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "IDHardware", idHardware)
				},
				{
					1,
					new MessageParameter(1, "DeviceType", deviceType)
				},
				{
					2,
					new MessageParameter(2, "DeviceIndex", deviceIndex)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60340_VmoServerServiceException(string loginProviderId, string claimName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60340, loginProviderId, claimName))
		{
			ErrorCode = 60340,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "LoginProviderId", loginProviderId)
				},
				{
					1,
					new MessageParameter(1, "ClaimName", claimName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60340_VmoServerServiceException(string loginProviderId, string claimName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60340, loginProviderId, claimName), innerException)
		{
			ErrorCode = 60340,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "LoginProviderId", loginProviderId)
				},
				{
					1,
					new MessageParameter(1, "ClaimName", claimName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60341_VmoServerServiceException(string loginProviderId, string claimName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60341, loginProviderId, claimName))
		{
			ErrorCode = 60341,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "LoginProviderId", loginProviderId)
				},
				{
					1,
					new MessageParameter(1, "ClaimName", claimName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60341_VmoServerServiceException(string loginProviderId, string claimName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60341, loginProviderId, claimName), innerException)
		{
			ErrorCode = 60341,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "LoginProviderId", loginProviderId)
				},
				{
					1,
					new MessageParameter(1, "ClaimName", claimName)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60342_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60342)
		{
			ErrorCode = 60342
		};
	}

	public static VmoServerServiceException VMO60342_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60342, innerException)
		{
			ErrorCode = 60342
		};
	}

	public static VmoServerServiceException VMO60343_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60343)
		{
			ErrorCode = 60343
		};
	}

	public static VmoServerServiceException VMO60343_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60343, innerException)
		{
			ErrorCode = 60343
		};
	}

	public static VmoServerServiceException VMO60344_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60344)
		{
			ErrorCode = 60344
		};
	}

	public static VmoServerServiceException VMO60344_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60344, innerException)
		{
			ErrorCode = 60344
		};
	}

	public static VmoServerServiceException VMO60345_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60345)
		{
			ErrorCode = 60345
		};
	}

	public static VmoServerServiceException VMO60345_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60345, innerException)
		{
			ErrorCode = 60345
		};
	}

	public static VmoServerServiceException VMO60346_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60346)
		{
			ErrorCode = 60346
		};
	}

	public static VmoServerServiceException VMO60346_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60346, innerException)
		{
			ErrorCode = 60346
		};
	}

	public static VmoServerServiceException VMO60347_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60347)
		{
			ErrorCode = 60347
		};
	}

	public static VmoServerServiceException VMO60347_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60347, innerException)
		{
			ErrorCode = 60347
		};
	}

	public static VmoServerServiceException VMO60348_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60348, name))
		{
			ErrorCode = 60348,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60348_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60348, name), innerException)
		{
			ErrorCode = 60348,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60349_VmoServerServiceException(string displayName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60349, displayName))
		{
			ErrorCode = 60349,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "DisplayName", displayName)
			} }
		};
	}

	public static VmoServerServiceException VMO60349_VmoServerServiceException(string displayName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60349, displayName), innerException)
		{
			ErrorCode = 60349,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "DisplayName", displayName)
			} }
		};
	}

	public static ObjectNotFoundException VMO60350_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60350)
		{
			ErrorCode = 60350
		};
	}

	public static ObjectNotFoundException VMO60350_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60350, innerException)
		{
			ErrorCode = 60350
		};
	}

	public static VmoServerServiceException VMO60351_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60351, name))
		{
			ErrorCode = 60351,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60351_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60351, name), innerException)
		{
			ErrorCode = 60351,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60352_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60352)
		{
			ErrorCode = 60352
		};
	}

	public static VmoServerServiceException VMO60352_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60352, innerException)
		{
			ErrorCode = 60352
		};
	}

	public static VmoServerServiceException VMO60353_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60353, name))
		{
			ErrorCode = 60353,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60353_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60353, name), innerException)
		{
			ErrorCode = 60353,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60354_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60354)
		{
			ErrorCode = 60354
		};
	}

	public static VmoServerServiceException VMO60354_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60354, innerException)
		{
			ErrorCode = 60354
		};
	}

	public static VmoServerServiceException VMO60355_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60355)
		{
			ErrorCode = 60355
		};
	}

	public static VmoServerServiceException VMO60355_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60355, innerException)
		{
			ErrorCode = 60355
		};
	}

	public static VmoServerServiceException VMO60356_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60356)
		{
			ErrorCode = 60356
		};
	}

	public static VmoServerServiceException VMO60356_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60356, innerException)
		{
			ErrorCode = 60356
		};
	}

	public static ObjectNotFoundException VMO60357_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60357)
		{
			ErrorCode = 60357
		};
	}

	public static ObjectNotFoundException VMO60357_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60357, innerException)
		{
			ErrorCode = 60357
		};
	}

	public static VmoServerServiceException VMO60358_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60358)
		{
			ErrorCode = 60358
		};
	}

	public static VmoServerServiceException VMO60358_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60358, innerException)
		{
			ErrorCode = 60358
		};
	}

	public static VmoServerServiceException VMO60359_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60359)
		{
			ErrorCode = 60359
		};
	}

	public static VmoServerServiceException VMO60359_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60359, innerException)
		{
			ErrorCode = 60359
		};
	}

	public static VmoServerServiceException VMO60360_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60360)
		{
			ErrorCode = 60360
		};
	}

	public static VmoServerServiceException VMO60360_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60360, innerException)
		{
			ErrorCode = 60360
		};
	}

	public static VmoServerServiceException VMO60361_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60361)
		{
			ErrorCode = 60361
		};
	}

	public static VmoServerServiceException VMO60361_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60361, innerException)
		{
			ErrorCode = 60361
		};
	}

	public static ObjectNotFoundException VMO60362_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60362)
		{
			ErrorCode = 60362
		};
	}

	public static ObjectNotFoundException VMO60362_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60362, innerException)
		{
			ErrorCode = 60362
		};
	}

	public static VmoServerServiceException VMO60363_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60363)
		{
			ErrorCode = 60363
		};
	}

	public static VmoServerServiceException VMO60363_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60363, innerException)
		{
			ErrorCode = 60363
		};
	}

	public static VmoServerServiceException VMO60364_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60364)
		{
			ErrorCode = 60364
		};
	}

	public static VmoServerServiceException VMO60364_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60364, innerException)
		{
			ErrorCode = 60364
		};
	}

	public static ObjectNotFoundException VMO60365_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60365)
		{
			ErrorCode = 60365
		};
	}

	public static ObjectNotFoundException VMO60365_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60365, innerException)
		{
			ErrorCode = 60365
		};
	}

	public static VmoServerServiceException VMO60366_VmoServerServiceException(string displayName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60366, displayName))
		{
			ErrorCode = 60366,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "DisplayName", displayName)
			} }
		};
	}

	public static VmoServerServiceException VMO60366_VmoServerServiceException(string displayName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60366, displayName), innerException)
		{
			ErrorCode = 60366,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "DisplayName", displayName)
			} }
		};
	}

	public static ObjectNotFoundException VMO60367_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60367)
		{
			ErrorCode = 60367
		};
	}

	public static ObjectNotFoundException VMO60367_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60367, innerException)
		{
			ErrorCode = 60367
		};
	}

	public static VmoServerServiceException VMO60368_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60368)
		{
			ErrorCode = 60368
		};
	}

	public static VmoServerServiceException VMO60368_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60368, innerException)
		{
			ErrorCode = 60368
		};
	}

	public static VmoServerServiceException VMO60369_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60369)
		{
			ErrorCode = 60369
		};
	}

	public static VmoServerServiceException VMO60369_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60369, innerException)
		{
			ErrorCode = 60369
		};
	}

	public static VmoServerServiceException VMO60370_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60370)
		{
			ErrorCode = 60370
		};
	}

	public static VmoServerServiceException VMO60370_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60370, innerException)
		{
			ErrorCode = 60370
		};
	}

	public static VmoServerServiceException VMO60371_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60371)
		{
			ErrorCode = 60371
		};
	}

	public static VmoServerServiceException VMO60371_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60371, innerException)
		{
			ErrorCode = 60371
		};
	}

	public static VmoServerServiceException VMO60372_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60372)
		{
			ErrorCode = 60372
		};
	}

	public static VmoServerServiceException VMO60372_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60372, innerException)
		{
			ErrorCode = 60372
		};
	}

	public static VmoServerServiceException VMO60373_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60373, name))
		{
			ErrorCode = 60373,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60373_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60373, name), innerException)
		{
			ErrorCode = 60373,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60374_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60374)
		{
			ErrorCode = 60374
		};
	}

	public static VmoServerServiceException VMO60374_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60374, innerException)
		{
			ErrorCode = 60374
		};
	}

	public static VmoServerServiceException VMO60375_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60375)
		{
			ErrorCode = 60375
		};
	}

	public static VmoServerServiceException VMO60375_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60375, innerException)
		{
			ErrorCode = 60375
		};
	}

	public static VmoServerServiceException VMO60376_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60376)
		{
			ErrorCode = 60376
		};
	}

	public static VmoServerServiceException VMO60376_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60376, innerException)
		{
			ErrorCode = 60376
		};
	}

	public static VmoServerServiceException VMO60377_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60377)
		{
			ErrorCode = 60377
		};
	}

	public static VmoServerServiceException VMO60377_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60377, innerException)
		{
			ErrorCode = 60377
		};
	}

	public static VmoServerServiceException VMO60378_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60378)
		{
			ErrorCode = 60378
		};
	}

	public static VmoServerServiceException VMO60378_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60378, innerException)
		{
			ErrorCode = 60378
		};
	}

	public static VmoServerServiceException VMO60379_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60379)
		{
			ErrorCode = 60379
		};
	}

	public static VmoServerServiceException VMO60379_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60379, innerException)
		{
			ErrorCode = 60379
		};
	}

	public static VmoServerServiceException VMO60380_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60380)
		{
			ErrorCode = 60380
		};
	}

	public static VmoServerServiceException VMO60380_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60380, innerException)
		{
			ErrorCode = 60380
		};
	}

	public static VmoServerServiceException VMO60381_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60381)
		{
			ErrorCode = 60381
		};
	}

	public static VmoServerServiceException VMO60381_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60381, innerException)
		{
			ErrorCode = 60381
		};
	}

	public static VmoServerServiceException VMO60382_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60382)
		{
			ErrorCode = 60382
		};
	}

	public static VmoServerServiceException VMO60382_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60382, innerException)
		{
			ErrorCode = 60382
		};
	}

	public static VmoServerServiceException VMO60383_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60383)
		{
			ErrorCode = 60383
		};
	}

	public static VmoServerServiceException VMO60383_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60383, innerException)
		{
			ErrorCode = 60383
		};
	}

	public static VmoServerServiceException VMO60384_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60384)
		{
			ErrorCode = 60384
		};
	}

	public static VmoServerServiceException VMO60384_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60384, innerException)
		{
			ErrorCode = 60384
		};
	}

	public static VmoServerServiceException VMO60385_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60385)
		{
			ErrorCode = 60385
		};
	}

	public static VmoServerServiceException VMO60385_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60385, innerException)
		{
			ErrorCode = 60385
		};
	}

	public static DeviceStreamConfigurationException VMO60386_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60386)
		{
			ErrorCode = 60386
		};
	}

	public static DeviceStreamConfigurationException VMO60386_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60386, innerException)
		{
			ErrorCode = 60386
		};
	}

	public static DeviceStreamConfigurationException VMO60387_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60387)
		{
			ErrorCode = 60387
		};
	}

	public static DeviceStreamConfigurationException VMO60387_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60387, innerException)
		{
			ErrorCode = 60387
		};
	}

	public static DeviceStreamConfigurationException VMO60388_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60388)
		{
			ErrorCode = 60388
		};
	}

	public static DeviceStreamConfigurationException VMO60388_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60388, innerException)
		{
			ErrorCode = 60388
		};
	}

	public static DeviceStreamConfigurationException VMO60389_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60389)
		{
			ErrorCode = 60389
		};
	}

	public static DeviceStreamConfigurationException VMO60389_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60389, innerException)
		{
			ErrorCode = 60389
		};
	}

	public static DeviceStreamConfigurationException VMO60390_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60390)
		{
			ErrorCode = 60390
		};
	}

	public static DeviceStreamConfigurationException VMO60390_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60390, innerException)
		{
			ErrorCode = 60390
		};
	}

	public static DeviceStreamConfigurationException VMO60391_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60391)
		{
			ErrorCode = 60391
		};
	}

	public static DeviceStreamConfigurationException VMO60391_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60391, innerException)
		{
			ErrorCode = 60391
		};
	}

	public static DeviceStreamConfigurationException VMO60392_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60392)
		{
			ErrorCode = 60392
		};
	}

	public static DeviceStreamConfigurationException VMO60392_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60392, innerException)
		{
			ErrorCode = 60392
		};
	}

	public static DeviceStreamConfigurationException VMO60393_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60393)
		{
			ErrorCode = 60393
		};
	}

	public static DeviceStreamConfigurationException VMO60393_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60393, innerException)
		{
			ErrorCode = 60393
		};
	}

	public static DeviceStreamConfigurationException VMO60394_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60394)
		{
			ErrorCode = 60394
		};
	}

	public static DeviceStreamConfigurationException VMO60394_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60394, innerException)
		{
			ErrorCode = 60394
		};
	}

	public static DeviceStreamConfigurationException VMO60395_DeviceStreamConfigurationException()
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60395)
		{
			ErrorCode = 60395
		};
	}

	public static DeviceStreamConfigurationException VMO60395_DeviceStreamConfigurationException(Exception innerException)
	{
		return new DeviceStreamConfigurationException(VmoResources.VMO60395, innerException)
		{
			ErrorCode = 60395
		};
	}

	public static VmoServerServiceException VMO60396_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60396)
		{
			ErrorCode = 60396
		};
	}

	public static VmoServerServiceException VMO60396_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60396, innerException)
		{
			ErrorCode = 60396
		};
	}

	public static VmoServerServiceException VMO60397_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60397)
		{
			ErrorCode = 60397
		};
	}

	public static VmoServerServiceException VMO60397_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60397, innerException)
		{
			ErrorCode = 60397
		};
	}

	public static VmoServerServiceException VMO60398_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60398)
		{
			ErrorCode = 60398
		};
	}

	public static VmoServerServiceException VMO60398_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60398, innerException)
		{
			ErrorCode = 60398
		};
	}

	public static VmoServerServiceException VMO60399_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60399)
		{
			ErrorCode = 60399
		};
	}

	public static VmoServerServiceException VMO60399_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60399, innerException)
		{
			ErrorCode = 60399
		};
	}

	public static VmoServerServiceException VMO60400_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60400)
		{
			ErrorCode = 60400
		};
	}

	public static VmoServerServiceException VMO60400_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60400, innerException)
		{
			ErrorCode = 60400
		};
	}

	public static VmoServerServiceException VMO60401_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60401)
		{
			ErrorCode = 60401
		};
	}

	public static VmoServerServiceException VMO60401_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60401, innerException)
		{
			ErrorCode = 60401
		};
	}

	public static VmoServerServiceException VMO60402_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60402)
		{
			ErrorCode = 60402
		};
	}

	public static VmoServerServiceException VMO60402_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60402, innerException)
		{
			ErrorCode = 60402
		};
	}

	public static VmoServerServiceException VMO60403_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60403)
		{
			ErrorCode = 60403
		};
	}

	public static VmoServerServiceException VMO60403_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60403, innerException)
		{
			ErrorCode = 60403
		};
	}

	public static VmoServerServiceException VMO60404_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60404)
		{
			ErrorCode = 60404
		};
	}

	public static VmoServerServiceException VMO60404_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60404, innerException)
		{
			ErrorCode = 60404
		};
	}

	public static VmoServerServiceException VMO60405_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60405)
		{
			ErrorCode = 60405
		};
	}

	public static VmoServerServiceException VMO60405_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60405, innerException)
		{
			ErrorCode = 60405
		};
	}

	public static VmoServerServiceException VMO60406_VmoServerServiceException(string fromSite, string toSite)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60406, fromSite, toSite))
		{
			ErrorCode = 60406,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "FromSite", fromSite)
				},
				{
					1,
					new MessageParameter(1, "ToSite", toSite)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60406_VmoServerServiceException(string fromSite, string toSite, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60406, fromSite, toSite), innerException)
		{
			ErrorCode = 60406,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "FromSite", fromSite)
				},
				{
					1,
					new MessageParameter(1, "ToSite", toSite)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60407_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60407)
		{
			ErrorCode = 60407
		};
	}

	public static VmoServerServiceException VMO60407_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60407, innerException)
		{
			ErrorCode = 60407
		};
	}

	public static VmoServerServiceException VMO60408_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60408)
		{
			ErrorCode = 60408
		};
	}

	public static VmoServerServiceException VMO60408_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60408, innerException)
		{
			ErrorCode = 60408
		};
	}

	public static VmoServerServiceException VMO60409_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60409)
		{
			ErrorCode = 60409
		};
	}

	public static VmoServerServiceException VMO60409_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60409, innerException)
		{
			ErrorCode = 60409
		};
	}

	public static VmoServerServiceException VMO60410_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60410)
		{
			ErrorCode = 60410
		};
	}

	public static VmoServerServiceException VMO60410_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60410, innerException)
		{
			ErrorCode = 60410
		};
	}

	public static ObjectNotFoundException VMO60411_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60411)
		{
			ErrorCode = 60411
		};
	}

	public static ObjectNotFoundException VMO60411_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60411, innerException)
		{
			ErrorCode = 60411
		};
	}

	public static ObjectNotFoundException VMO60412_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60412)
		{
			ErrorCode = 60412
		};
	}

	public static ObjectNotFoundException VMO60412_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60412, innerException)
		{
			ErrorCode = 60412
		};
	}

	public static VmoServerServiceException VMO60413_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60413)
		{
			ErrorCode = 60413
		};
	}

	public static VmoServerServiceException VMO60413_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60413, innerException)
		{
			ErrorCode = 60413
		};
	}

	public static ObjectNotFoundException VMO60414_ObjectNotFoundException()
	{
		return new ObjectNotFoundException(VmoResources.VMO60414)
		{
			ErrorCode = 60414
		};
	}

	public static ObjectNotFoundException VMO60414_ObjectNotFoundException(Exception innerException)
	{
		return new ObjectNotFoundException(VmoResources.VMO60414, innerException)
		{
			ErrorCode = 60414
		};
	}

	public static VmoServerServiceException VMO60415_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60415)
		{
			ErrorCode = 60415
		};
	}

	public static VmoServerServiceException VMO60415_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60415, innerException)
		{
			ErrorCode = 60415
		};
	}

	public static VmoServerServiceException VMO60416_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60416)
		{
			ErrorCode = 60416
		};
	}

	public static VmoServerServiceException VMO60416_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60416, innerException)
		{
			ErrorCode = 60416
		};
	}

	public static VmoServerServiceException VMO60417_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60417)
		{
			ErrorCode = 60417
		};
	}

	public static VmoServerServiceException VMO60417_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60417, innerException)
		{
			ErrorCode = 60417
		};
	}

	public static VmoServerServiceException VMO60451_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60451, id))
		{
			ErrorCode = 60451,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO60451_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60451, id), innerException)
		{
			ErrorCode = 60451,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO60452_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60452)
		{
			ErrorCode = 60452
		};
	}

	public static VmoServerServiceException VMO60452_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60452, innerException)
		{
			ErrorCode = 60452
		};
	}

	public static VmoServerServiceException VMO60453_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60453)
		{
			ErrorCode = 60453
		};
	}

	public static VmoServerServiceException VMO60453_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60453, innerException)
		{
			ErrorCode = 60453
		};
	}

	public static VmoServerServiceException VMO60454_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60454)
		{
			ErrorCode = 60454
		};
	}

	public static VmoServerServiceException VMO60454_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60454, innerException)
		{
			ErrorCode = 60454
		};
	}

	public static VmoServerServiceException VMO60455_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60455)
		{
			ErrorCode = 60455
		};
	}

	public static VmoServerServiceException VMO60455_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60455, innerException)
		{
			ErrorCode = 60455
		};
	}

	public static VmoServerServiceException VMO60456_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60456)
		{
			ErrorCode = 60456
		};
	}

	public static VmoServerServiceException VMO60456_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60456, innerException)
		{
			ErrorCode = 60456
		};
	}

	public static VmoServerServiceException VMO60460_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60460)
		{
			ErrorCode = 60460
		};
	}

	public static VmoServerServiceException VMO60460_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60460, innerException)
		{
			ErrorCode = 60460
		};
	}

	public static VmoServerServiceException VMO60461_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60461)
		{
			ErrorCode = 60461
		};
	}

	public static VmoServerServiceException VMO60461_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60461, innerException)
		{
			ErrorCode = 60461
		};
	}

	public static VmoServerServiceException VMO60464_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60464)
		{
			ErrorCode = 60464
		};
	}

	public static VmoServerServiceException VMO60464_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60464, innerException)
		{
			ErrorCode = 60464
		};
	}

	public static VmoServerServiceException VMO60465_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60465)
		{
			ErrorCode = 60465
		};
	}

	public static VmoServerServiceException VMO60465_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60465, innerException)
		{
			ErrorCode = 60465
		};
	}

	public static VmoServerServiceException VMO60470_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60470)
		{
			ErrorCode = 60470
		};
	}

	public static VmoServerServiceException VMO60470_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60470, innerException)
		{
			ErrorCode = 60470
		};
	}

	public static VmoServerServiceException VMO60471_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60471)
		{
			ErrorCode = 60471
		};
	}

	public static VmoServerServiceException VMO60471_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60471, innerException)
		{
			ErrorCode = 60471
		};
	}

	public static VmoServerServiceException VMO60472_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60472)
		{
			ErrorCode = 60472
		};
	}

	public static VmoServerServiceException VMO60472_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60472, innerException)
		{
			ErrorCode = 60472
		};
	}

	public static VmoServerServiceException VMO60473_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60473)
		{
			ErrorCode = 60473
		};
	}

	public static VmoServerServiceException VMO60473_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60473, innerException)
		{
			ErrorCode = 60473
		};
	}

	public static VmoServerServiceException VMO60474_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60474)
		{
			ErrorCode = 60474
		};
	}

	public static VmoServerServiceException VMO60474_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60474, innerException)
		{
			ErrorCode = 60474
		};
	}

	public static VmoServerServiceException VMO60475_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60475)
		{
			ErrorCode = 60475
		};
	}

	public static VmoServerServiceException VMO60475_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60475, innerException)
		{
			ErrorCode = 60475
		};
	}

	public static VmoServerServiceException VMO60476_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60476)
		{
			ErrorCode = 60476
		};
	}

	public static VmoServerServiceException VMO60476_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60476, innerException)
		{
			ErrorCode = 60476
		};
	}

	public static VmoServerServiceException VMO60477_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60477)
		{
			ErrorCode = 60477
		};
	}

	public static VmoServerServiceException VMO60477_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60477, innerException)
		{
			ErrorCode = 60477
		};
	}

	public static VmoServerServiceException VMO60480_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60480)
		{
			ErrorCode = 60480
		};
	}

	public static VmoServerServiceException VMO60480_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60480, innerException)
		{
			ErrorCode = 60480
		};
	}

	public static VmoServerServiceException VMO60481_VmoServerServiceException(string id, string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60481, id, name))
		{
			ErrorCode = 60481,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Id", id)
				},
				{
					1,
					new MessageParameter(1, "Name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60481_VmoServerServiceException(string id, string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60481, id, name), innerException)
		{
			ErrorCode = 60481,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Id", id)
				},
				{
					1,
					new MessageParameter(1, "Name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60482_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60482, name))
		{
			ErrorCode = 60482,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60482_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60482, name), innerException)
		{
			ErrorCode = 60482,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60483_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60483)
		{
			ErrorCode = 60483
		};
	}

	public static VmoServerServiceException VMO60483_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60483, innerException)
		{
			ErrorCode = 60483
		};
	}

	public static VmoServerServiceException VMO60484_VmoServerServiceException(string idEventType)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60484, idEventType))
		{
			ErrorCode = 60484,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDEventType", idEventType)
			} }
		};
	}

	public static VmoServerServiceException VMO60484_VmoServerServiceException(string idEventType, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60484, idEventType), innerException)
		{
			ErrorCode = 60484,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDEventType", idEventType)
			} }
		};
	}

	public static VmoServerServiceException VMO60501_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60501)
		{
			ErrorCode = 60501
		};
	}

	public static VmoServerServiceException VMO60501_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60501, innerException)
		{
			ErrorCode = 60501
		};
	}

	public static VmoServerServiceException VMO60502_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60502)
		{
			ErrorCode = 60502
		};
	}

	public static VmoServerServiceException VMO60502_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60502, innerException)
		{
			ErrorCode = 60502
		};
	}

	public static VmoServerServiceException VMO60510_VmoServerServiceException(string address, string name, string exceptionMessage)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60510, address, name, exceptionMessage))
		{
			ErrorCode = 60510,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				},
				{
					2,
					new MessageParameter(2, "exceptionMessage", exceptionMessage)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60510_VmoServerServiceException(string address, string name, string exceptionMessage, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60510, address, name, exceptionMessage), innerException)
		{
			ErrorCode = 60510,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				},
				{
					2,
					new MessageParameter(2, "exceptionMessage", exceptionMessage)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60511_VmoServerServiceException(string address, string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60511, address, name))
		{
			ErrorCode = 60511,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60511_VmoServerServiceException(string address, string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60511, address, name), innerException)
		{
			ErrorCode = 60511,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO60512_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60512)
		{
			ErrorCode = 60512
		};
	}

	public static VmoServerServiceException VMO60512_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60512, innerException)
		{
			ErrorCode = 60512
		};
	}

	public static VmoServerServiceException VMO60513_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60513)
		{
			ErrorCode = 60513
		};
	}

	public static VmoServerServiceException VMO60513_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60513, innerException)
		{
			ErrorCode = 60513
		};
	}

	public static VmoServerServiceException VMO60514_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60514)
		{
			ErrorCode = 60514
		};
	}

	public static VmoServerServiceException VMO60514_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60514, innerException)
		{
			ErrorCode = 60514
		};
	}

	public static VmoServerServiceException VMO60515_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60515)
		{
			ErrorCode = 60515
		};
	}

	public static VmoServerServiceException VMO60515_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60515, innerException)
		{
			ErrorCode = 60515
		};
	}

	public static VmoServerServiceException VMO60516_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60516)
		{
			ErrorCode = 60516
		};
	}

	public static VmoServerServiceException VMO60516_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60516, innerException)
		{
			ErrorCode = 60516
		};
	}

	public static VmoServerServiceException VMO60517_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60517)
		{
			ErrorCode = 60517
		};
	}

	public static VmoServerServiceException VMO60517_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60517, innerException)
		{
			ErrorCode = 60517
		};
	}

	public static VmoServerServiceException VMO60518_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60518)
		{
			ErrorCode = 60518
		};
	}

	public static VmoServerServiceException VMO60518_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60518, innerException)
		{
			ErrorCode = 60518
		};
	}

	public static VmoServerServiceException VMO60519_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60519)
		{
			ErrorCode = 60519
		};
	}

	public static VmoServerServiceException VMO60519_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60519, innerException)
		{
			ErrorCode = 60519
		};
	}

	public static VmoServerServiceException VMO60520_VmoServerServiceException(string error)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60520, error))
		{
			ErrorCode = 60520,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "error", error)
			} }
		};
	}

	public static VmoServerServiceException VMO60520_VmoServerServiceException(string error, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60520, error), innerException)
		{
			ErrorCode = 60520,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "error", error)
			} }
		};
	}

	public static VmoServerServiceException VMO60521_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60521)
		{
			ErrorCode = 60521
		};
	}

	public static VmoServerServiceException VMO60521_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60521, innerException)
		{
			ErrorCode = 60521
		};
	}

	public static VmoServerServiceException VMO60530_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60530)
		{
			ErrorCode = 60530
		};
	}

	public static VmoServerServiceException VMO60530_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60530, innerException)
		{
			ErrorCode = 60530
		};
	}

	public static VmoServerServiceException VMO60531_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60531)
		{
			ErrorCode = 60531
		};
	}

	public static VmoServerServiceException VMO60531_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60531, innerException)
		{
			ErrorCode = 60531
		};
	}

	public static VmoServerServiceException VMO60601_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60601, name))
		{
			ErrorCode = 60601,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60601_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60601, name), innerException)
		{
			ErrorCode = 60601,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60602_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60602, name))
		{
			ErrorCode = 60602,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60602_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60602, name), innerException)
		{
			ErrorCode = 60602,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60603_VmoServerServiceException(string pkid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60603, pkid))
		{
			ErrorCode = 60603,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid)
			} }
		};
	}

	public static VmoServerServiceException VMO60603_VmoServerServiceException(string pkid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60603, pkid), innerException)
		{
			ErrorCode = 60603,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid)
			} }
		};
	}

	public static VmoServerServiceException VMO60604_VmoServerServiceException(int pkid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60604, pkid))
		{
			ErrorCode = 60604,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60604_VmoServerServiceException(int pkid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60604, pkid), innerException)
		{
			ErrorCode = 60604,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60605_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60605, name))
		{
			ErrorCode = 60605,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60605_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60605, name), innerException)
		{
			ErrorCode = 60605,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60611_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60611, name))
		{
			ErrorCode = 60611,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60611_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60611, name), innerException)
		{
			ErrorCode = 60611,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60612_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60612, name))
		{
			ErrorCode = 60612,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60612_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60612, name), innerException)
		{
			ErrorCode = 60612,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60613_VmoServerServiceException(int pkid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60613, pkid))
		{
			ErrorCode = 60613,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60613_VmoServerServiceException(int pkid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60613, pkid), innerException)
		{
			ErrorCode = 60613,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60614_VmoServerServiceException(int pkid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60614, pkid))
		{
			ErrorCode = 60614,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60614_VmoServerServiceException(int pkid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60614, pkid), innerException)
		{
			ErrorCode = 60614,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "pkid", pkid.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60615_VmoServerServiceException(int id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60615, id))
		{
			ErrorCode = 60615,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60615_VmoServerServiceException(int id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60615, id), innerException)
		{
			ErrorCode = 60615,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Id", id.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO60616_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60616)
		{
			ErrorCode = 60616
		};
	}

	public static VmoServerServiceException VMO60616_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60616, innerException)
		{
			ErrorCode = 60616
		};
	}

	public static VmoServerServiceException VMO60630_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60630)
		{
			ErrorCode = 60630
		};
	}

	public static VmoServerServiceException VMO60630_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60630, innerException)
		{
			ErrorCode = 60630
		};
	}

	public static VmoServerServiceException VMO60631_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60631)
		{
			ErrorCode = 60631
		};
	}

	public static VmoServerServiceException VMO60631_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60631, innerException)
		{
			ErrorCode = 60631
		};
	}

	public static VmoServerServiceException VMO60632_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60632)
		{
			ErrorCode = 60632
		};
	}

	public static VmoServerServiceException VMO60632_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60632, innerException)
		{
			ErrorCode = 60632
		};
	}

	public static VmoServerServiceException VMO60633_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60633)
		{
			ErrorCode = 60633
		};
	}

	public static VmoServerServiceException VMO60633_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60633, innerException)
		{
			ErrorCode = 60633
		};
	}

	public static VmoServerServiceException VMO60634_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60634)
		{
			ErrorCode = 60634
		};
	}

	public static VmoServerServiceException VMO60634_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60634, innerException)
		{
			ErrorCode = 60634
		};
	}

	public static VmoServerServiceException VMO60635_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60635)
		{
			ErrorCode = 60635
		};
	}

	public static VmoServerServiceException VMO60635_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60635, innerException)
		{
			ErrorCode = 60635
		};
	}

	public static VmoServerServiceException VMO60636_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60636)
		{
			ErrorCode = 60636
		};
	}

	public static VmoServerServiceException VMO60636_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60636, innerException)
		{
			ErrorCode = 60636
		};
	}

	public static VmoServerServiceException VMO60637_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60637)
		{
			ErrorCode = 60637
		};
	}

	public static VmoServerServiceException VMO60637_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60637, innerException)
		{
			ErrorCode = 60637
		};
	}

	public static VmoServerServiceException VMO60638_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60638)
		{
			ErrorCode = 60638
		};
	}

	public static VmoServerServiceException VMO60638_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60638, innerException)
		{
			ErrorCode = 60638
		};
	}

	public static VmoServerServiceException VMO60650_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60650)
		{
			ErrorCode = 60650
		};
	}

	public static VmoServerServiceException VMO60650_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60650, innerException)
		{
			ErrorCode = 60650
		};
	}

	public static VmoServerServiceException VMO60651_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60651)
		{
			ErrorCode = 60651
		};
	}

	public static VmoServerServiceException VMO60651_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60651, innerException)
		{
			ErrorCode = 60651
		};
	}

	public static VmoServerServiceException VMO60660_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60660)
		{
			ErrorCode = 60660
		};
	}

	public static VmoServerServiceException VMO60660_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60660, innerException)
		{
			ErrorCode = 60660
		};
	}

	public static VmoServerServiceException VMO60661_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60661)
		{
			ErrorCode = 60661
		};
	}

	public static VmoServerServiceException VMO60661_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60661, innerException)
		{
			ErrorCode = 60661
		};
	}

	public static VmoServerServiceException VMO60701_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60701, name))
		{
			ErrorCode = 60701,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60701_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60701, name), innerException)
		{
			ErrorCode = 60701,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60702_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60702)
		{
			ErrorCode = 60702
		};
	}

	public static VmoServerServiceException VMO60702_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60702, innerException)
		{
			ErrorCode = 60702
		};
	}

	public static VmoServerServiceException VMO60703_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60703, name))
		{
			ErrorCode = 60703,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60703_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60703, name), innerException)
		{
			ErrorCode = 60703,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO60704_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60704)
		{
			ErrorCode = 60704
		};
	}

	public static VmoServerServiceException VMO60704_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60704, innerException)
		{
			ErrorCode = 60704
		};
	}

	public static VmoServerServiceException VMO60705_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO60705)
		{
			ErrorCode = 60705
		};
	}

	public static VmoServerServiceException VMO60705_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO60705, innerException)
		{
			ErrorCode = 60705
		};
	}

	public static VmoServerServiceException VMO61000_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61000)
		{
			ErrorCode = 61000
		};
	}

	public static VmoServerServiceException VMO61000_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61000, innerException)
		{
			ErrorCode = 61000
		};
	}

	public static VmoServerServiceException VMO61001_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61001)
		{
			ErrorCode = 61001
		};
	}

	public static VmoServerServiceException VMO61001_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61001, innerException)
		{
			ErrorCode = 61001
		};
	}

	public static VmoServerServiceException VMO61002_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61002)
		{
			ErrorCode = 61002
		};
	}

	public static VmoServerServiceException VMO61002_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61002, innerException)
		{
			ErrorCode = 61002
		};
	}

	public static VmoServerServiceException VMO61003_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61003)
		{
			ErrorCode = 61003
		};
	}

	public static VmoServerServiceException VMO61003_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61003, innerException)
		{
			ErrorCode = 61003
		};
	}

	public static VmoServerServiceException VMO61004_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61004)
		{
			ErrorCode = 61004
		};
	}

	public static VmoServerServiceException VMO61004_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61004, innerException)
		{
			ErrorCode = 61004
		};
	}

	public static VmoServerServiceException VMO61005_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61005)
		{
			ErrorCode = 61005
		};
	}

	public static VmoServerServiceException VMO61005_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61005, innerException)
		{
			ErrorCode = 61005
		};
	}

	public static VmoServerServiceException VMO61006_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61006, name))
		{
			ErrorCode = 61006,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61006_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61006, name), innerException)
		{
			ErrorCode = 61006,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61007_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61007, name))
		{
			ErrorCode = 61007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61007_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61007, name), innerException)
		{
			ErrorCode = 61007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61008_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61008)
		{
			ErrorCode = 61008
		};
	}

	public static VmoServerServiceException VMO61008_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61008, innerException)
		{
			ErrorCode = 61008
		};
	}

	public static VmoServerServiceException VMO61009_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61009, name))
		{
			ErrorCode = 61009,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61009_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61009, name), innerException)
		{
			ErrorCode = 61009,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61010_VmoServerServiceException(string domain)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61010, domain))
		{
			ErrorCode = 61010,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "domain", domain)
			} }
		};
	}

	public static VmoServerServiceException VMO61010_VmoServerServiceException(string domain, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61010, domain), innerException)
		{
			ErrorCode = 61010,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "domain", domain)
			} }
		};
	}

	public static VmoServerServiceException VMO61011_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61011)
		{
			ErrorCode = 61011
		};
	}

	public static VmoServerServiceException VMO61011_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61011, innerException)
		{
			ErrorCode = 61011
		};
	}

	public static VmoServerServiceException VMO61012_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61012)
		{
			ErrorCode = 61012
		};
	}

	public static VmoServerServiceException VMO61012_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61012, innerException)
		{
			ErrorCode = 61012
		};
	}

	public static VmoServerServiceException VMO61013_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61013)
		{
			ErrorCode = 61013
		};
	}

	public static VmoServerServiceException VMO61013_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61013, innerException)
		{
			ErrorCode = 61013
		};
	}

	public static VmoServerServiceException VMO61015_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61015)
		{
			ErrorCode = 61015
		};
	}

	public static VmoServerServiceException VMO61015_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61015, innerException)
		{
			ErrorCode = 61015
		};
	}

	public static VmoServerServiceException VMO61021_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61021)
		{
			ErrorCode = 61021
		};
	}

	public static VmoServerServiceException VMO61021_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61021, innerException)
		{
			ErrorCode = 61021
		};
	}

	public static VmoServerServiceException VMO61022_VmoServerServiceException(string command, string status)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61022, command, status))
		{
			ErrorCode = 61022,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "command", command)
				},
				{
					1,
					new MessageParameter(1, "status", status)
				}
			}
		};
	}

	public static VmoServerServiceException VMO61022_VmoServerServiceException(string command, string status, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61022, command, status), innerException)
		{
			ErrorCode = 61022,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "command", command)
				},
				{
					1,
					new MessageParameter(1, "status", status)
				}
			}
		};
	}

	public static VmoServerServiceException VMO61023_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61023)
		{
			ErrorCode = 61023
		};
	}

	public static VmoServerServiceException VMO61023_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61023, innerException)
		{
			ErrorCode = 61023
		};
	}

	public static VmoServerServiceException VMO61024_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61024)
		{
			ErrorCode = 61024
		};
	}

	public static VmoServerServiceException VMO61024_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61024, innerException)
		{
			ErrorCode = 61024
		};
	}

	public static VmoServerServiceException VMO61025_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61025)
		{
			ErrorCode = 61025
		};
	}

	public static VmoServerServiceException VMO61025_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61025, innerException)
		{
			ErrorCode = 61025
		};
	}

	public static VmoServerServiceException VMO61026_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61026)
		{
			ErrorCode = 61026
		};
	}

	public static VmoServerServiceException VMO61026_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61026, innerException)
		{
			ErrorCode = 61026
		};
	}

	public static VmoServerServiceException VMO61027_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61027)
		{
			ErrorCode = 61027
		};
	}

	public static VmoServerServiceException VMO61027_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61027, innerException)
		{
			ErrorCode = 61027
		};
	}

	public static VmoServerServiceException VMO61028_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61028)
		{
			ErrorCode = 61028
		};
	}

	public static VmoServerServiceException VMO61028_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61028, innerException)
		{
			ErrorCode = 61028
		};
	}

	public static VmoServerServiceException VMO61029_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61029)
		{
			ErrorCode = 61029
		};
	}

	public static VmoServerServiceException VMO61029_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61029, innerException)
		{
			ErrorCode = 61029
		};
	}

	public static VmoServerServiceException VMO61030_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61030)
		{
			ErrorCode = 61030
		};
	}

	public static VmoServerServiceException VMO61030_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61030, innerException)
		{
			ErrorCode = 61030
		};
	}

	public static VmoServerServiceException VMO61031_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61031)
		{
			ErrorCode = 61031
		};
	}

	public static VmoServerServiceException VMO61031_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61031, innerException)
		{
			ErrorCode = 61031
		};
	}

	public static VmoServerServiceException VMO61032_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61032)
		{
			ErrorCode = 61032
		};
	}

	public static VmoServerServiceException VMO61032_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61032, innerException)
		{
			ErrorCode = 61032
		};
	}

	public static VmoServerServiceException VMO61033_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61033)
		{
			ErrorCode = 61033
		};
	}

	public static VmoServerServiceException VMO61033_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61033, innerException)
		{
			ErrorCode = 61033
		};
	}

	public static VmoServerServiceException VMO61034_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61034)
		{
			ErrorCode = 61034
		};
	}

	public static VmoServerServiceException VMO61034_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61034, innerException)
		{
			ErrorCode = 61034
		};
	}

	public static VmoServerServiceException VMO61035_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61035)
		{
			ErrorCode = 61035
		};
	}

	public static VmoServerServiceException VMO61035_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61035, innerException)
		{
			ErrorCode = 61035
		};
	}

	public static VmoServerServiceException VMO61100_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61100)
		{
			ErrorCode = 61100
		};
	}

	public static VmoServerServiceException VMO61100_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61100, innerException)
		{
			ErrorCode = 61100
		};
	}

	public static VmoServerServiceException VMO61103_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61103)
		{
			ErrorCode = 61103
		};
	}

	public static VmoServerServiceException VMO61103_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61103, innerException)
		{
			ErrorCode = 61103
		};
	}

	public static VmoServerServiceException VMO61104_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61104)
		{
			ErrorCode = 61104
		};
	}

	public static VmoServerServiceException VMO61104_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61104, innerException)
		{
			ErrorCode = 61104
		};
	}

	public static VmoServerServiceException VMO61105_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61105)
		{
			ErrorCode = 61105
		};
	}

	public static VmoServerServiceException VMO61105_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61105, innerException)
		{
			ErrorCode = 61105
		};
	}

	public static VmoServerServiceException VMO61106_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61106)
		{
			ErrorCode = 61106
		};
	}

	public static VmoServerServiceException VMO61106_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61106, innerException)
		{
			ErrorCode = 61106
		};
	}

	public static VmoServerServiceException VMO61107_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61107)
		{
			ErrorCode = 61107
		};
	}

	public static VmoServerServiceException VMO61107_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61107, innerException)
		{
			ErrorCode = 61107
		};
	}

	public static VmoServerServiceException VMO61108_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61108, name))
		{
			ErrorCode = 61108,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61108_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61108, name), innerException)
		{
			ErrorCode = 61108,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO61109_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61109)
		{
			ErrorCode = 61109
		};
	}

	public static VmoServerServiceException VMO61109_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61109, innerException)
		{
			ErrorCode = 61109
		};
	}

	public static VmoServerServiceException VMO61110_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO61110)
		{
			ErrorCode = 61110
		};
	}

	public static VmoServerServiceException VMO61110_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO61110, innerException)
		{
			ErrorCode = 61110
		};
	}

	public static VmoServerServiceException VMO62001_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62001)
		{
			ErrorCode = 62001
		};
	}

	public static VmoServerServiceException VMO62001_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62001, innerException)
		{
			ErrorCode = 62001
		};
	}

	public static VmoServerServiceException VMO62002_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62002)
		{
			ErrorCode = 62002
		};
	}

	public static VmoServerServiceException VMO62002_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62002, innerException)
		{
			ErrorCode = 62002
		};
	}

	public static VmoServerServiceException VMO62003_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62003)
		{
			ErrorCode = 62003
		};
	}

	public static VmoServerServiceException VMO62003_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62003, innerException)
		{
			ErrorCode = 62003
		};
	}

	public static VmoServerServiceException VMO62004_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62004)
		{
			ErrorCode = 62004
		};
	}

	public static VmoServerServiceException VMO62004_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62004, innerException)
		{
			ErrorCode = 62004
		};
	}

	public static VmoServerServiceException VMO62005_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62005)
		{
			ErrorCode = 62005
		};
	}

	public static VmoServerServiceException VMO62005_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62005, innerException)
		{
			ErrorCode = 62005
		};
	}

	public static VmoServerServiceException VMO62006_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62006)
		{
			ErrorCode = 62006
		};
	}

	public static VmoServerServiceException VMO62006_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62006, innerException)
		{
			ErrorCode = 62006
		};
	}

	public static VmoServerServiceException VMO62007_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62007)
		{
			ErrorCode = 62007
		};
	}

	public static VmoServerServiceException VMO62007_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62007, innerException)
		{
			ErrorCode = 62007
		};
	}

	public static VmoServerServiceException VMO62008_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62008)
		{
			ErrorCode = 62008
		};
	}

	public static VmoServerServiceException VMO62008_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62008, innerException)
		{
			ErrorCode = 62008
		};
	}

	public static VmoServerServiceException VMO62010_VmoServerServiceException(string argumentName, string sid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62010, argumentName, sid))
		{
			ErrorCode = 62010,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "sid", sid)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62010_VmoServerServiceException(string argumentName, string sid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62010, argumentName, sid), innerException)
		{
			ErrorCode = 62010,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "sid", sid)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62011_VmoServerServiceException(string argumentName, string action)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62011, argumentName, action))
		{
			ErrorCode = 62011,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "action", action)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62011_VmoServerServiceException(string argumentName, string action, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62011, argumentName, action), innerException)
		{
			ErrorCode = 62011,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "action", action)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62012_VmoServerServiceException(string argumentName, string classId)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62012, argumentName, classId))
		{
			ErrorCode = 62012,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "classId", classId)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62012_VmoServerServiceException(string argumentName, string classId, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62012, argumentName, classId), innerException)
		{
			ErrorCode = 62012,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "classId", classId)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62013_VmoServerServiceException(string argumentName, string sid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62013, argumentName, sid))
		{
			ErrorCode = 62013,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "sid", sid)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62013_VmoServerServiceException(string argumentName, string sid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62013, argumentName, sid), innerException)
		{
			ErrorCode = 62013,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "sid", sid)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62014_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62014, name))
		{
			ErrorCode = 62014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO62014_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62014, name), innerException)
		{
			ErrorCode = 62014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO62015_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62015, name))
		{
			ErrorCode = 62015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO62015_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62015, name), innerException)
		{
			ErrorCode = 62015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO62016_VmoServerServiceException(string sid)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62016, sid))
		{
			ErrorCode = 62016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "sid", sid)
			} }
		};
	}

	public static VmoServerServiceException VMO62016_VmoServerServiceException(string sid, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62016, sid), innerException)
		{
			ErrorCode = 62016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "sid", sid)
			} }
		};
	}

	public static ActiveDirectoryAccessException VMO62017_ActiveDirectoryAccessException()
	{
		return new ActiveDirectoryAccessException(VmoResources.VMO62017)
		{
			ErrorCode = 62017
		};
	}

	public static ActiveDirectoryAccessException VMO62017_ActiveDirectoryAccessException(Exception innerException)
	{
		return new ActiveDirectoryAccessException(VmoResources.VMO62017, innerException)
		{
			ErrorCode = 62017
		};
	}

	public static VmoServerServiceException VMO62018_VmoServerServiceException(string domain)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62018, domain))
		{
			ErrorCode = 62018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "domain", domain)
			} }
		};
	}

	public static VmoServerServiceException VMO62018_VmoServerServiceException(string domain, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62018, domain), innerException)
		{
			ErrorCode = 62018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "domain", domain)
			} }
		};
	}

	public static VmoServerServiceException VMO62019_VmoServerServiceException(string argumentName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62019, argumentName))
		{
			ErrorCode = 62019,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62019_VmoServerServiceException(string argumentName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62019, argumentName), innerException)
		{
			ErrorCode = 62019,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62020_VmoServerServiceException(string argumentName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62020, argumentName))
		{
			ErrorCode = 62020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62020_VmoServerServiceException(string argumentName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62020, argumentName), innerException)
		{
			ErrorCode = 62020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62021_VmoServerServiceException(string argumentName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62021, argumentName))
		{
			ErrorCode = 62021,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62021_VmoServerServiceException(string argumentName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62021, argumentName), innerException)
		{
			ErrorCode = 62021,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62022_VmoServerServiceException(string argumentName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62022, argumentName))
		{
			ErrorCode = 62022,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62022_VmoServerServiceException(string argumentName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62022, argumentName), innerException)
		{
			ErrorCode = 62022,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62023_VmoServerServiceException(string argumentName, string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62023, argumentName, name))
		{
			ErrorCode = 62023,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62023_VmoServerServiceException(string argumentName, string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62023, argumentName, name), innerException)
		{
			ErrorCode = 62023,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "argumentName", argumentName)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO62024_VmoServerServiceException(string argumentName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62024, argumentName))
		{
			ErrorCode = 62024,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62024_VmoServerServiceException(string argumentName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62024, argumentName), innerException)
		{
			ErrorCode = 62024,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62025_VmoServerServiceException(string argumentName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62025, argumentName))
		{
			ErrorCode = 62025,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62025_VmoServerServiceException(string argumentName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62025, argumentName), innerException)
		{
			ErrorCode = 62025,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62026_VmoServerServiceException(string argumentName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62026, argumentName))
		{
			ErrorCode = 62026,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62026_VmoServerServiceException(string argumentName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62026, argumentName), innerException)
		{
			ErrorCode = 62026,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "argumentName", argumentName)
			} }
		};
	}

	public static VmoServerServiceException VMO62027_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62027)
		{
			ErrorCode = 62027
		};
	}

	public static VmoServerServiceException VMO62027_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62027, innerException)
		{
			ErrorCode = 62027
		};
	}

	public static VmoServerServiceException VMO62500_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO62500)
		{
			ErrorCode = 62500
		};
	}

	public static VmoServerServiceException VMO62500_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO62500, innerException)
		{
			ErrorCode = 62500
		};
	}

	public static VmoServerServiceException VMO63007_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63007, name))
		{
			ErrorCode = 63007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO63007_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63007, name), innerException)
		{
			ErrorCode = 63007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO63008_VmoServerServiceException(string file, string error)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63008, file, error))
		{
			ErrorCode = 63008,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "file", file)
				},
				{
					1,
					new MessageParameter(1, "error", error)
				}
			}
		};
	}

	public static VmoServerServiceException VMO63008_VmoServerServiceException(string file, string error, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63008, file, error), innerException)
		{
			ErrorCode = 63008,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "file", file)
				},
				{
					1,
					new MessageParameter(1, "error", error)
				}
			}
		};
	}

	public static VmoServerServiceException VMO63009_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO63009)
		{
			ErrorCode = 63009
		};
	}

	public static VmoServerServiceException VMO63009_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO63009, innerException)
		{
			ErrorCode = 63009
		};
	}

	public static VmoServerServiceException VMO63010_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO63010)
		{
			ErrorCode = 63010
		};
	}

	public static VmoServerServiceException VMO63010_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO63010, innerException)
		{
			ErrorCode = 63010
		};
	}

	public static VmoServerServiceException VMO63011_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO63011)
		{
			ErrorCode = 63011
		};
	}

	public static VmoServerServiceException VMO63011_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO63011, innerException)
		{
			ErrorCode = 63011
		};
	}

	public static VmoServerServiceException VMO63012_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO63012)
		{
			ErrorCode = 63012
		};
	}

	public static VmoServerServiceException VMO63012_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO63012, innerException)
		{
			ErrorCode = 63012
		};
	}

	public static VmoServerServiceException VMO63013_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO63013)
		{
			ErrorCode = 63013
		};
	}

	public static VmoServerServiceException VMO63013_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO63013, innerException)
		{
			ErrorCode = 63013
		};
	}

	public static VmoServerServiceException VMO63014_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO63014)
		{
			ErrorCode = 63014
		};
	}

	public static VmoServerServiceException VMO63014_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO63014, innerException)
		{
			ErrorCode = 63014
		};
	}

	public static VmoServerServiceException VMO63015_VmoServerServiceException(string eventTypeName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63015, eventTypeName))
		{
			ErrorCode = 63015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "eventTypeName", eventTypeName)
			} }
		};
	}

	public static VmoServerServiceException VMO63015_VmoServerServiceException(string eventTypeName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63015, eventTypeName), innerException)
		{
			ErrorCode = 63015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "eventTypeName", eventTypeName)
			} }
		};
	}

	public static VmoServerServiceException VMO63016_VmoServerServiceException(string featureName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63016, featureName))
		{
			ErrorCode = 63016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "FeatureName", featureName)
			} }
		};
	}

	public static VmoServerServiceException VMO63016_VmoServerServiceException(string featureName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63016, featureName), innerException)
		{
			ErrorCode = 63016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "FeatureName", featureName)
			} }
		};
	}

	public static VmoServerServiceException VMO64000_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64000)
		{
			ErrorCode = 64000
		};
	}

	public static VmoServerServiceException VMO64000_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64000, innerException)
		{
			ErrorCode = 64000
		};
	}

	public static VmoServerServiceException VMO64001_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64001)
		{
			ErrorCode = 64001
		};
	}

	public static VmoServerServiceException VMO64001_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64001, innerException)
		{
			ErrorCode = 64001
		};
	}

	public static VmoServerServiceException VMO64002_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64002)
		{
			ErrorCode = 64002
		};
	}

	public static VmoServerServiceException VMO64002_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64002, innerException)
		{
			ErrorCode = 64002
		};
	}

	public static VmoServerServiceException VMO64003_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64003)
		{
			ErrorCode = 64003
		};
	}

	public static VmoServerServiceException VMO64003_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64003, innerException)
		{
			ErrorCode = 64003
		};
	}

	public static VmoServerServiceException VMO64004_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64004)
		{
			ErrorCode = 64004
		};
	}

	public static VmoServerServiceException VMO64004_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64004, innerException)
		{
			ErrorCode = 64004
		};
	}

	public static VmoServerServiceException VMO64005_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64005)
		{
			ErrorCode = 64005
		};
	}

	public static VmoServerServiceException VMO64005_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64005, innerException)
		{
			ErrorCode = 64005
		};
	}

	public static VmoServerServiceException VMO64006_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64006)
		{
			ErrorCode = 64006
		};
	}

	public static VmoServerServiceException VMO64006_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64006, innerException)
		{
			ErrorCode = 64006
		};
	}

	public static LicenseActivationException VMO64007_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64007)
		{
			ErrorCode = 64007
		};
	}

	public static LicenseActivationException VMO64007_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64007, innerException)
		{
			ErrorCode = 64007
		};
	}

	public static LicenseActivationException VMO64008_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64008)
		{
			ErrorCode = 64008
		};
	}

	public static LicenseActivationException VMO64008_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64008, innerException)
		{
			ErrorCode = 64008
		};
	}

	public static LicenseActivationException VMO64009_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64009)
		{
			ErrorCode = 64009
		};
	}

	public static LicenseActivationException VMO64009_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64009, innerException)
		{
			ErrorCode = 64009
		};
	}

	public static VmoServerServiceException VMO64010_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64010)
		{
			ErrorCode = 64010
		};
	}

	public static VmoServerServiceException VMO64010_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64010, innerException)
		{
			ErrorCode = 64010
		};
	}

	public static VmoServerServiceException VMO64011_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64011)
		{
			ErrorCode = 64011
		};
	}

	public static VmoServerServiceException VMO64011_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64011, innerException)
		{
			ErrorCode = 64011
		};
	}

	public static VmoServerServiceException VMO64012_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64012)
		{
			ErrorCode = 64012
		};
	}

	public static VmoServerServiceException VMO64012_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64012, innerException)
		{
			ErrorCode = 64012
		};
	}

	public static VmoServerServiceException VMO64013_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64013)
		{
			ErrorCode = 64013
		};
	}

	public static VmoServerServiceException VMO64013_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64013, innerException)
		{
			ErrorCode = 64013
		};
	}

	public static VmoServerServiceException VMO64014_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64014)
		{
			ErrorCode = 64014
		};
	}

	public static VmoServerServiceException VMO64014_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64014, innerException)
		{
			ErrorCode = 64014
		};
	}

	public static VmoServerServiceException VMO64015_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64015)
		{
			ErrorCode = 64015
		};
	}

	public static VmoServerServiceException VMO64015_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64015, innerException)
		{
			ErrorCode = 64015
		};
	}

	public static VmoServerServiceException VMO64016_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64016)
		{
			ErrorCode = 64016
		};
	}

	public static VmoServerServiceException VMO64016_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64016, innerException)
		{
			ErrorCode = 64016
		};
	}

	public static VmoServerServiceException VMO64017_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64017)
		{
			ErrorCode = 64017
		};
	}

	public static VmoServerServiceException VMO64017_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64017, innerException)
		{
			ErrorCode = 64017
		};
	}

	public static VmoServerServiceException VMO64018_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64018)
		{
			ErrorCode = 64018
		};
	}

	public static VmoServerServiceException VMO64018_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64018, innerException)
		{
			ErrorCode = 64018
		};
	}

	public static VmoServerServiceException VMO64019_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64019)
		{
			ErrorCode = 64019
		};
	}

	public static VmoServerServiceException VMO64019_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64019, innerException)
		{
			ErrorCode = 64019
		};
	}

	public static VmoServerServiceException VMO64020_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64020)
		{
			ErrorCode = 64020
		};
	}

	public static VmoServerServiceException VMO64020_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64020, innerException)
		{
			ErrorCode = 64020
		};
	}

	public static VmoServerServiceException VMO64021_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64021)
		{
			ErrorCode = 64021
		};
	}

	public static VmoServerServiceException VMO64021_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64021, innerException)
		{
			ErrorCode = 64021
		};
	}

	public static VmoServerServiceException VMO64022_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64022)
		{
			ErrorCode = 64022
		};
	}

	public static VmoServerServiceException VMO64022_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64022, innerException)
		{
			ErrorCode = 64022
		};
	}

	public static VmoServerServiceException VMO64023_VmoServerServiceException(string smartClientOEMName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64023, smartClientOEMName))
		{
			ErrorCode = 64023,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "SmartClientOEMName", smartClientOEMName)
			} }
		};
	}

	public static VmoServerServiceException VMO64023_VmoServerServiceException(string smartClientOEMName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64023, smartClientOEMName), innerException)
		{
			ErrorCode = 64023,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "SmartClientOEMName", smartClientOEMName)
			} }
		};
	}

	public static VmoServerServiceException VMO64024_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64024)
		{
			ErrorCode = 64024
		};
	}

	public static VmoServerServiceException VMO64024_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64024, innerException)
		{
			ErrorCode = 64024
		};
	}

	public static VmoServerServiceException VMO64025_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64025)
		{
			ErrorCode = 64025
		};
	}

	public static VmoServerServiceException VMO64025_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64025, innerException)
		{
			ErrorCode = 64025
		};
	}

	public static VmoServerServiceException VMO64026_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64026)
		{
			ErrorCode = 64026
		};
	}

	public static VmoServerServiceException VMO64026_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64026, innerException)
		{
			ErrorCode = 64026
		};
	}

	public static VmoServerServiceException VMO64027_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64027)
		{
			ErrorCode = 64027
		};
	}

	public static VmoServerServiceException VMO64027_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64027, innerException)
		{
			ErrorCode = 64027
		};
	}

	public static VmoServerServiceException VMO64028_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64028)
		{
			ErrorCode = 64028
		};
	}

	public static VmoServerServiceException VMO64028_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64028, innerException)
		{
			ErrorCode = 64028
		};
	}

	public static VmoServerServiceException VMO64029_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64029)
		{
			ErrorCode = 64029
		};
	}

	public static VmoServerServiceException VMO64029_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64029, innerException)
		{
			ErrorCode = 64029
		};
	}

	public static VmoServerServiceException VMO64030_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64030)
		{
			ErrorCode = 64030
		};
	}

	public static VmoServerServiceException VMO64030_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64030, innerException)
		{
			ErrorCode = 64030
		};
	}

	public static VmoServerServiceException VMO64031_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64031)
		{
			ErrorCode = 64031
		};
	}

	public static VmoServerServiceException VMO64031_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64031, innerException)
		{
			ErrorCode = 64031
		};
	}

	public static VmoServerServiceException VMO64032_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64032)
		{
			ErrorCode = 64032
		};
	}

	public static VmoServerServiceException VMO64032_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64032, innerException)
		{
			ErrorCode = 64032
		};
	}

	public static VmoServerServiceException VMO64033_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64033)
		{
			ErrorCode = 64033
		};
	}

	public static VmoServerServiceException VMO64033_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64033, innerException)
		{
			ErrorCode = 64033
		};
	}

	public static VmoServerServiceException VMO64034_VmoServerServiceException(string smartClientOEMName)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64034, smartClientOEMName))
		{
			ErrorCode = 64034,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "SmartClientOEMName", smartClientOEMName)
			} }
		};
	}

	public static VmoServerServiceException VMO64034_VmoServerServiceException(string smartClientOEMName, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64034, smartClientOEMName), innerException)
		{
			ErrorCode = 64034,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "SmartClientOEMName", smartClientOEMName)
			} }
		};
	}

	public static VmoServerServiceException VMO64035_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64035)
		{
			ErrorCode = 64035
		};
	}

	public static VmoServerServiceException VMO64035_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64035, innerException)
		{
			ErrorCode = 64035
		};
	}

	public static VmoServerServiceException VMO64036_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64036)
		{
			ErrorCode = 64036
		};
	}

	public static VmoServerServiceException VMO64036_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64036, innerException)
		{
			ErrorCode = 64036
		};
	}

	public static VmoServerServiceException VMO64037_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64037)
		{
			ErrorCode = 64037
		};
	}

	public static VmoServerServiceException VMO64037_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64037, innerException)
		{
			ErrorCode = 64037
		};
	}

	public static VmoServerServiceException VMO64038_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64038)
		{
			ErrorCode = 64038
		};
	}

	public static VmoServerServiceException VMO64038_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64038, innerException)
		{
			ErrorCode = 64038
		};
	}

	public static VmoServerServiceException VMO64039_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64039)
		{
			ErrorCode = 64039
		};
	}

	public static VmoServerServiceException VMO64039_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64039, innerException)
		{
			ErrorCode = 64039
		};
	}

	public static VmoServerServiceException VMO64040_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64040)
		{
			ErrorCode = 64040
		};
	}

	public static VmoServerServiceException VMO64040_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64040, innerException)
		{
			ErrorCode = 64040
		};
	}

	public static VmoServerServiceException VMO64041_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64041)
		{
			ErrorCode = 64041
		};
	}

	public static VmoServerServiceException VMO64041_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64041, innerException)
		{
			ErrorCode = 64041
		};
	}

	public static VmoServerServiceException VMO64042_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64042)
		{
			ErrorCode = 64042
		};
	}

	public static VmoServerServiceException VMO64042_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64042, innerException)
		{
			ErrorCode = 64042
		};
	}

	public static VmoServerServiceException VMO64043_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64043)
		{
			ErrorCode = 64043
		};
	}

	public static VmoServerServiceException VMO64043_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64043, innerException)
		{
			ErrorCode = 64043
		};
	}

	public static LicenseActivationException VMO64050_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64050)
		{
			ErrorCode = 64050
		};
	}

	public static LicenseActivationException VMO64050_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64050, innerException)
		{
			ErrorCode = 64050
		};
	}

	public static LicenseActivationException VMO64051_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64051)
		{
			ErrorCode = 64051
		};
	}

	public static LicenseActivationException VMO64051_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64051, innerException)
		{
			ErrorCode = 64051
		};
	}

	public static LicenseActivationException VMO64052_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64052)
		{
			ErrorCode = 64052
		};
	}

	public static LicenseActivationException VMO64052_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64052, innerException)
		{
			ErrorCode = 64052
		};
	}

	public static LicenseActivationException VMO64053_LicenseActivationException(string oemIcpName)
	{
		return new LicenseActivationException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64053, oemIcpName))
		{
			ErrorCode = 64053,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "OemIcpName", oemIcpName)
			} }
		};
	}

	public static LicenseActivationException VMO64053_LicenseActivationException(string oemIcpName, Exception innerException)
	{
		return new LicenseActivationException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64053, oemIcpName), innerException)
		{
			ErrorCode = 64053,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "OemIcpName", oemIcpName)
			} }
		};
	}

	public static LicenseActivationException VMO64054_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64054)
		{
			ErrorCode = 64054
		};
	}

	public static LicenseActivationException VMO64054_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64054, innerException)
		{
			ErrorCode = 64054
		};
	}

	public static LicenseActivationException VMO64055_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64055)
		{
			ErrorCode = 64055
		};
	}

	public static LicenseActivationException VMO64055_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64055, innerException)
		{
			ErrorCode = 64055
		};
	}

	public static LicenseActivationException VMO64056_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64056)
		{
			ErrorCode = 64056
		};
	}

	public static LicenseActivationException VMO64056_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64056, innerException)
		{
			ErrorCode = 64056
		};
	}

	public static LicenseActivationException VMO64057_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64057)
		{
			ErrorCode = 64057
		};
	}

	public static LicenseActivationException VMO64057_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64057, innerException)
		{
			ErrorCode = 64057
		};
	}

	public static LicenseActivationException VMO64058_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64058)
		{
			ErrorCode = 64058
		};
	}

	public static LicenseActivationException VMO64058_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64058, innerException)
		{
			ErrorCode = 64058
		};
	}

	public static LicenseActivationException VMO64059_LicenseActivationException()
	{
		return new LicenseActivationException(VmoResources.VMO64059)
		{
			ErrorCode = 64059
		};
	}

	public static LicenseActivationException VMO64059_LicenseActivationException(Exception innerException)
	{
		return new LicenseActivationException(VmoResources.VMO64059, innerException)
		{
			ErrorCode = 64059
		};
	}

	public static VmoServerServiceException VMO64060_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64060)
		{
			ErrorCode = 64060
		};
	}

	public static VmoServerServiceException VMO64060_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64060, innerException)
		{
			ErrorCode = 64060
		};
	}

	public static VmoServerServiceException VMO64100_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64100, name))
		{
			ErrorCode = 64100,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO64100_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64100, name), innerException)
		{
			ErrorCode = 64100,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO64101_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64101)
		{
			ErrorCode = 64101
		};
	}

	public static VmoServerServiceException VMO64101_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64101, innerException)
		{
			ErrorCode = 64101
		};
	}

	public static VmoServerServiceException VMO64102_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64102)
		{
			ErrorCode = 64102
		};
	}

	public static VmoServerServiceException VMO64102_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64102, innerException)
		{
			ErrorCode = 64102
		};
	}

	public static VmoServerServiceException VMO64103_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64103, name))
		{
			ErrorCode = 64103,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO64103_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64103, name), innerException)
		{
			ErrorCode = 64103,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO64104_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64104)
		{
			ErrorCode = 64104
		};
	}

	public static VmoServerServiceException VMO64104_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64104, innerException)
		{
			ErrorCode = 64104
		};
	}

	public static VmoServerServiceException VMO64105_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO64105)
		{
			ErrorCode = 64105
		};
	}

	public static VmoServerServiceException VMO64105_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO64105, innerException)
		{
			ErrorCode = 64105
		};
	}

	public static VmoServerServiceException VMO65000_VmoServerServiceException(int status, string @namespace, string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65000, status, @namespace, id))
		{
			ErrorCode = 65000,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Status", status.ToString(CultureInfo.InvariantCulture))
				},
				{
					1,
					new MessageParameter(1, "Namespace", @namespace)
				},
				{
					2,
					new MessageParameter(2, "ID", id)
				}
			}
		};
	}

	public static VmoServerServiceException VMO65000_VmoServerServiceException(int status, string @namespace, string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65000, status, @namespace, id), innerException)
		{
			ErrorCode = 65000,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Status", status.ToString(CultureInfo.InvariantCulture))
				},
				{
					1,
					new MessageParameter(1, "Namespace", @namespace)
				},
				{
					2,
					new MessageParameter(2, "ID", id)
				}
			}
		};
	}

	public static VmoServerServiceException VMO65001_VmoServerServiceException(int status, string @namespace, string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65001, status, @namespace, id))
		{
			ErrorCode = 65001,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Status", status.ToString(CultureInfo.InvariantCulture))
				},
				{
					1,
					new MessageParameter(1, "Namespace", @namespace)
				},
				{
					2,
					new MessageParameter(2, "ID", id)
				}
			}
		};
	}

	public static VmoServerServiceException VMO65001_VmoServerServiceException(int status, string @namespace, string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65001, status, @namespace, id), innerException)
		{
			ErrorCode = 65001,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Status", status.ToString(CultureInfo.InvariantCulture))
				},
				{
					1,
					new MessageParameter(1, "Namespace", @namespace)
				},
				{
					2,
					new MessageParameter(2, "ID", id)
				}
			}
		};
	}

	public static VmoServerServiceException VMO65002_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65002)
		{
			ErrorCode = 65002
		};
	}

	public static VmoServerServiceException VMO65002_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65002, innerException)
		{
			ErrorCode = 65002
		};
	}

	public static VmoServerServiceException VMO65003_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65003)
		{
			ErrorCode = 65003
		};
	}

	public static VmoServerServiceException VMO65003_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65003, innerException)
		{
			ErrorCode = 65003
		};
	}

	public static VmoServerServiceException VMO65004_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65004)
		{
			ErrorCode = 65004
		};
	}

	public static VmoServerServiceException VMO65004_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65004, innerException)
		{
			ErrorCode = 65004
		};
	}

	public static VmoServerServiceException VMO65005_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65005, name))
		{
			ErrorCode = 65005,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO65005_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65005, name), innerException)
		{
			ErrorCode = 65005,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO65006_VmoServerServiceException(string value, string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65006, value, name))
		{
			ErrorCode = 65006,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Value", value)
				},
				{
					1,
					new MessageParameter(1, "Name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO65006_VmoServerServiceException(string value, string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65006, value, name), innerException)
		{
			ErrorCode = 65006,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "Value", value)
				},
				{
					1,
					new MessageParameter(1, "Name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO65007_VmoServerServiceException(string message)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65007, message))
		{
			ErrorCode = 65007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Message", message)
			} }
		};
	}

	public static VmoServerServiceException VMO65007_VmoServerServiceException(string message, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65007, message), innerException)
		{
			ErrorCode = 65007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Message", message)
			} }
		};
	}

	public static VmoServerServiceException VMO65008_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65008, name))
		{
			ErrorCode = 65008,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO65008_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65008, name), innerException)
		{
			ErrorCode = 65008,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "Name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO65009_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65009)
		{
			ErrorCode = 65009
		};
	}

	public static VmoServerServiceException VMO65009_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65009, innerException)
		{
			ErrorCode = 65009
		};
	}

	public static VmoServerServiceException VMO65010_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65010)
		{
			ErrorCode = 65010
		};
	}

	public static VmoServerServiceException VMO65010_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65010, innerException)
		{
			ErrorCode = 65010
		};
	}

	public static VmoServerServiceException VMO65011_VmoServerServiceException(string idTimeProfile)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65011, idTimeProfile))
		{
			ErrorCode = 65011,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDTimeProfile", idTimeProfile)
			} }
		};
	}

	public static VmoServerServiceException VMO65011_VmoServerServiceException(string idTimeProfile, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65011, idTimeProfile), innerException)
		{
			ErrorCode = 65011,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "IDTimeProfile", idTimeProfile)
			} }
		};
	}

	public static VmoServerServiceException VMO65012_VmoServerServiceException(string macaddress)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65012, macaddress))
		{
			ErrorCode = 65012,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "macaddress", macaddress)
			} }
		};
	}

	public static VmoServerServiceException VMO65012_VmoServerServiceException(string macaddress, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65012, macaddress), innerException)
		{
			ErrorCode = 65012,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "macaddress", macaddress)
			} }
		};
	}

	public static VmoServerServiceException VMO65013_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65013, id))
		{
			ErrorCode = 65013,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65013_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65013, id), innerException)
		{
			ErrorCode = 65013,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65014_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65014, id))
		{
			ErrorCode = 65014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65014_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65014, id), innerException)
		{
			ErrorCode = 65014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65015_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65015, id))
		{
			ErrorCode = 65015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65015_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65015, id), innerException)
		{
			ErrorCode = 65015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65016_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65016)
		{
			ErrorCode = 65016
		};
	}

	public static VmoServerServiceException VMO65016_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65016, innerException)
		{
			ErrorCode = 65016
		};
	}

	public static VmoServerServiceException VMO65017_VmoServerServiceException(int drivernumber)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65017, drivernumber))
		{
			ErrorCode = 65017,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "drivernumber", drivernumber.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO65017_VmoServerServiceException(int drivernumber, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65017, drivernumber), innerException)
		{
			ErrorCode = 65017,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "drivernumber", drivernumber.ToString(CultureInfo.InvariantCulture))
			} }
		};
	}

	public static VmoServerServiceException VMO65018_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65018, id))
		{
			ErrorCode = 65018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65018_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65018, id), innerException)
		{
			ErrorCode = 65018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65019_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65019, id))
		{
			ErrorCode = 65019,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65019_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65019, id), innerException)
		{
			ErrorCode = 65019,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65020_VmoServerServiceException(string id)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65020, id))
		{
			ErrorCode = 65020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65020_VmoServerServiceException(string id, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65020, id), innerException)
		{
			ErrorCode = 65020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "id", id)
			} }
		};
	}

	public static VmoServerServiceException VMO65021_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65021)
		{
			ErrorCode = 65021
		};
	}

	public static VmoServerServiceException VMO65021_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65021, innerException)
		{
			ErrorCode = 65021
		};
	}

	public static VmoServerServiceException VMO65022_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO65022)
		{
			ErrorCode = 65022
		};
	}

	public static VmoServerServiceException VMO65022_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO65022, innerException)
		{
			ErrorCode = 65022
		};
	}

	public static VmoServerServiceException VMO66000_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66000, name))
		{
			ErrorCode = 66000,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66000_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66000, name), innerException)
		{
			ErrorCode = 66000,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66001_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66001, name))
		{
			ErrorCode = 66001,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66001_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66001, name), innerException)
		{
			ErrorCode = 66001,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66002_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66002, name))
		{
			ErrorCode = 66002,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66002_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66002, name), innerException)
		{
			ErrorCode = 66002,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66003_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66003, name))
		{
			ErrorCode = 66003,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66003_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66003, name), innerException)
		{
			ErrorCode = 66003,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66004_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66004, name))
		{
			ErrorCode = 66004,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66004_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66004, name), innerException)
		{
			ErrorCode = 66004,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66005_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66005, name))
		{
			ErrorCode = 66005,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66005_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66005, name), innerException)
		{
			ErrorCode = 66005,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66006_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66006, name))
		{
			ErrorCode = 66006,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66006_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66006, name), innerException)
		{
			ErrorCode = 66006,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66007_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66007, name))
		{
			ErrorCode = 66007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66007_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66007, name), innerException)
		{
			ErrorCode = 66007,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66008_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66008, name))
		{
			ErrorCode = 66008,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66008_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66008, name), innerException)
		{
			ErrorCode = 66008,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66009_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66009, name))
		{
			ErrorCode = 66009,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66009_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66009, name), innerException)
		{
			ErrorCode = 66009,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66010_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66010, name))
		{
			ErrorCode = 66010,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66010_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66010, name), innerException)
		{
			ErrorCode = 66010,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66011_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66011, name))
		{
			ErrorCode = 66011,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66011_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66011, name), innerException)
		{
			ErrorCode = 66011,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66012_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66012, name))
		{
			ErrorCode = 66012,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66012_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66012, name), innerException)
		{
			ErrorCode = 66012,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66013_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66013, name))
		{
			ErrorCode = 66013,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66013_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66013, name), innerException)
		{
			ErrorCode = 66013,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66014_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66014, name))
		{
			ErrorCode = 66014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66014_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66014, name), innerException)
		{
			ErrorCode = 66014,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66015_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66015, name))
		{
			ErrorCode = 66015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66015_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66015, name), innerException)
		{
			ErrorCode = 66015,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66016_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66016, name))
		{
			ErrorCode = 66016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66016_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66016, name), innerException)
		{
			ErrorCode = 66016,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66017_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66017, name))
		{
			ErrorCode = 66017,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66017_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66017, name), innerException)
		{
			ErrorCode = 66017,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66018_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66018, name))
		{
			ErrorCode = 66018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66018_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66018, name), innerException)
		{
			ErrorCode = 66018,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66019_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66019, name))
		{
			ErrorCode = 66019,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66019_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66019, name), innerException)
		{
			ErrorCode = 66019,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66020_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66020, name))
		{
			ErrorCode = 66020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66020_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66020, name), innerException)
		{
			ErrorCode = 66020,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO66030_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66030)
		{
			ErrorCode = 66030
		};
	}

	public static VmoServerServiceException VMO66030_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66030, innerException)
		{
			ErrorCode = 66030
		};
	}

	public static VmoServerServiceException VMO66031_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66031)
		{
			ErrorCode = 66031
		};
	}

	public static VmoServerServiceException VMO66031_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66031, innerException)
		{
			ErrorCode = 66031
		};
	}

	public static VmoServerServiceException VMO66032_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66032)
		{
			ErrorCode = 66032
		};
	}

	public static VmoServerServiceException VMO66032_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66032, innerException)
		{
			ErrorCode = 66032
		};
	}

	public static VmoServerServiceException VMO66033_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66033)
		{
			ErrorCode = 66033
		};
	}

	public static VmoServerServiceException VMO66033_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66033, innerException)
		{
			ErrorCode = 66033
		};
	}

	public static VmoServerServiceException VMO66034_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66034)
		{
			ErrorCode = 66034
		};
	}

	public static VmoServerServiceException VMO66034_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66034, innerException)
		{
			ErrorCode = 66034
		};
	}

	public static VmoServerServiceException VMO66035_VmoServerServiceException(string minHardwarePasswordLength, string maxHardwarePasswordLength)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66035, minHardwarePasswordLength, maxHardwarePasswordLength))
		{
			ErrorCode = 66035,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "minHardwarePasswordLength", minHardwarePasswordLength)
				},
				{
					1,
					new MessageParameter(1, "maxHardwarePasswordLength", maxHardwarePasswordLength)
				}
			}
		};
	}

	public static VmoServerServiceException VMO66035_VmoServerServiceException(string minHardwarePasswordLength, string maxHardwarePasswordLength, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66035, minHardwarePasswordLength, maxHardwarePasswordLength), innerException)
		{
			ErrorCode = 66035,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "minHardwarePasswordLength", minHardwarePasswordLength)
				},
				{
					1,
					new MessageParameter(1, "maxHardwarePasswordLength", maxHardwarePasswordLength)
				}
			}
		};
	}

	public static VmoServerServiceException VMO66040_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66040)
		{
			ErrorCode = 66040
		};
	}

	public static VmoServerServiceException VMO66040_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66040, innerException)
		{
			ErrorCode = 66040
		};
	}

	public static VmoServerServiceException VMO66050_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66050)
		{
			ErrorCode = 66050
		};
	}

	public static VmoServerServiceException VMO66050_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66050, innerException)
		{
			ErrorCode = 66050
		};
	}

	public static VmoServerServiceException VMO66051_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66051)
		{
			ErrorCode = 66051
		};
	}

	public static VmoServerServiceException VMO66051_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66051, innerException)
		{
			ErrorCode = 66051
		};
	}

	public static VmoServerServiceException VMO66052_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66052)
		{
			ErrorCode = 66052
		};
	}

	public static VmoServerServiceException VMO66052_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66052, innerException)
		{
			ErrorCode = 66052
		};
	}

	public static VmoServerServiceException VMO66101_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66101)
		{
			ErrorCode = 66101
		};
	}

	public static VmoServerServiceException VMO66101_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66101, innerException)
		{
			ErrorCode = 66101
		};
	}

	public static VmoServerServiceException VMO66102_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66102)
		{
			ErrorCode = 66102
		};
	}

	public static VmoServerServiceException VMO66102_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66102, innerException)
		{
			ErrorCode = 66102
		};
	}

	public static VmoServerServiceException VMO66103_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66103)
		{
			ErrorCode = 66103
		};
	}

	public static VmoServerServiceException VMO66103_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66103, innerException)
		{
			ErrorCode = 66103
		};
	}

	public static VmoServerServiceException VMO66104_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66104)
		{
			ErrorCode = 66104
		};
	}

	public static VmoServerServiceException VMO66104_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66104, innerException)
		{
			ErrorCode = 66104
		};
	}

	public static VmoServerServiceException VMO66105_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66105)
		{
			ErrorCode = 66105
		};
	}

	public static VmoServerServiceException VMO66105_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66105, innerException)
		{
			ErrorCode = 66105
		};
	}

	public static VmoServerServiceException VMO66106_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66106)
		{
			ErrorCode = 66106
		};
	}

	public static VmoServerServiceException VMO66106_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66106, innerException)
		{
			ErrorCode = 66106
		};
	}

	public static VmoServerServiceException VMO66107_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66107)
		{
			ErrorCode = 66107
		};
	}

	public static VmoServerServiceException VMO66107_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66107, innerException)
		{
			ErrorCode = 66107
		};
	}

	public static VmoServerServiceException VMO66108_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66108)
		{
			ErrorCode = 66108
		};
	}

	public static VmoServerServiceException VMO66108_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66108, innerException)
		{
			ErrorCode = 66108
		};
	}

	public static VmoServerServiceException VMO66109_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66109)
		{
			ErrorCode = 66109
		};
	}

	public static VmoServerServiceException VMO66109_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66109, innerException)
		{
			ErrorCode = 66109
		};
	}

	public static VmoServerServiceException VMO66110_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66110)
		{
			ErrorCode = 66110
		};
	}

	public static VmoServerServiceException VMO66110_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66110, innerException)
		{
			ErrorCode = 66110
		};
	}

	public static VmoServerServiceException VMO66111_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66111)
		{
			ErrorCode = 66111
		};
	}

	public static VmoServerServiceException VMO66111_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66111, innerException)
		{
			ErrorCode = 66111
		};
	}

	public static VmoServerServiceException VMO66112_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66112)
		{
			ErrorCode = 66112
		};
	}

	public static VmoServerServiceException VMO66112_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66112, innerException)
		{
			ErrorCode = 66112
		};
	}

	public static VmoServerServiceException VMO66113_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66113)
		{
			ErrorCode = 66113
		};
	}

	public static VmoServerServiceException VMO66113_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66113, innerException)
		{
			ErrorCode = 66113
		};
	}

	public static VmoServerServiceException VMO66114_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66114)
		{
			ErrorCode = 66114
		};
	}

	public static VmoServerServiceException VMO66114_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66114, innerException)
		{
			ErrorCode = 66114
		};
	}

	public static VmoServerServiceException VMO66115_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO66115)
		{
			ErrorCode = 66115
		};
	}

	public static VmoServerServiceException VMO66115_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO66115, innerException)
		{
			ErrorCode = 66115
		};
	}

	public static VmoServerServiceException VMO67001_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67001)
		{
			ErrorCode = 67001
		};
	}

	public static VmoServerServiceException VMO67001_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67001, innerException)
		{
			ErrorCode = 67001
		};
	}

	public static VmoServerServiceException VMO67002_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67002, name))
		{
			ErrorCode = 67002,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO67002_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67002, name), innerException)
		{
			ErrorCode = 67002,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO67003_VmoServerServiceException(string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67003, name))
		{
			ErrorCode = 67003,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO67003_VmoServerServiceException(string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67003, name), innerException)
		{
			ErrorCode = 67003,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "name", name)
			} }
		};
	}

	public static VmoServerServiceException VMO67004_VmoServerServiceException(string address, string name, string exceptionMessage)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67004, address, name, exceptionMessage))
		{
			ErrorCode = 67004,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				},
				{
					2,
					new MessageParameter(2, "exceptionMessage", exceptionMessage)
				}
			}
		};
	}

	public static VmoServerServiceException VMO67004_VmoServerServiceException(string address, string name, string exceptionMessage, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67004, address, name, exceptionMessage), innerException)
		{
			ErrorCode = 67004,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				},
				{
					2,
					new MessageParameter(2, "exceptionMessage", exceptionMessage)
				}
			}
		};
	}

	public static VmoServerServiceException VMO67005_VmoServerServiceException(string address, string name)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67005, address, name))
		{
			ErrorCode = 67005,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO67005_VmoServerServiceException(string address, string name, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67005, address, name), innerException)
		{
			ErrorCode = 67005,
			MessageParameters = 
			{
				{
					0,
					new MessageParameter(0, "address", address)
				},
				{
					1,
					new MessageParameter(1, "name", name)
				}
			}
		};
	}

	public static VmoServerServiceException VMO67006_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67006)
		{
			ErrorCode = 67006
		};
	}

	public static VmoServerServiceException VMO67006_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67006, innerException)
		{
			ErrorCode = 67006
		};
	}

	public static VmoServerServiceException VMO67007_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67007)
		{
			ErrorCode = 67007
		};
	}

	public static VmoServerServiceException VMO67007_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67007, innerException)
		{
			ErrorCode = 67007
		};
	}

	public static VmoServerServiceException VMO67008_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67008)
		{
			ErrorCode = 67008
		};
	}

	public static VmoServerServiceException VMO67008_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67008, innerException)
		{
			ErrorCode = 67008
		};
	}

	public static VmoServerServiceException VMO67009_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67009)
		{
			ErrorCode = 67009
		};
	}

	public static VmoServerServiceException VMO67009_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67009, innerException)
		{
			ErrorCode = 67009
		};
	}

	public static VmoServerServiceException VMO67010_VmoServerServiceException(string message)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67010, message))
		{
			ErrorCode = 67010,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "message", message)
			} }
		};
	}

	public static VmoServerServiceException VMO67010_VmoServerServiceException(string message, Exception innerException)
	{
		return new VmoServerServiceException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67010, message), innerException)
		{
			ErrorCode = 67010,
			MessageParameters = { 
			{
				0,
				new MessageParameter(0, "message", message)
			} }
		};
	}

	public static VmoServerServiceException VMO67011_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67011)
		{
			ErrorCode = 67011
		};
	}

	public static VmoServerServiceException VMO67011_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67011, innerException)
		{
			ErrorCode = 67011
		};
	}

	public static VmoServerServiceException VMO67012_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67012)
		{
			ErrorCode = 67012
		};
	}

	public static VmoServerServiceException VMO67012_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67012, innerException)
		{
			ErrorCode = 67012
		};
	}

	public static VmoServerServiceException VMO67013_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67013)
		{
			ErrorCode = 67013
		};
	}

	public static VmoServerServiceException VMO67013_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67013, innerException)
		{
			ErrorCode = 67013
		};
	}

	public static VmoServerServiceException VMO67014_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67014)
		{
			ErrorCode = 67014
		};
	}

	public static VmoServerServiceException VMO67014_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67014, innerException)
		{
			ErrorCode = 67014
		};
	}

	public static VmoServerServiceException VMO67015_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67015)
		{
			ErrorCode = 67015
		};
	}

	public static VmoServerServiceException VMO67015_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67015, innerException)
		{
			ErrorCode = 67015
		};
	}

	public static VmoServerServiceException VMO67016_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67016)
		{
			ErrorCode = 67016
		};
	}

	public static VmoServerServiceException VMO67016_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67016, innerException)
		{
			ErrorCode = 67016
		};
	}

	public static VmoServerServiceException VMO67017_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO67017)
		{
			ErrorCode = 67017
		};
	}

	public static VmoServerServiceException VMO67017_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO67017, innerException)
		{
			ErrorCode = 67017
		};
	}

	public static VmoServerServiceException VMO68000_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO68000)
		{
			ErrorCode = 68000
		};
	}

	public static VmoServerServiceException VMO68000_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO68000, innerException)
		{
			ErrorCode = 68000
		};
	}

	public static VmoServerServiceException VMO69000_VmoServerServiceException()
	{
		return new VmoServerServiceException(VmoResources.VMO69000)
		{
			ErrorCode = 69000
		};
	}

	public static VmoServerServiceException VMO69000_VmoServerServiceException(Exception innerException)
	{
		return new VmoServerServiceException(VmoResources.VMO69000, innerException)
		{
			ErrorCode = 69000
		};
	}
}
