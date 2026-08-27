using System;
using VideoOS.IM.Common.Service.Interfaces;

namespace VideoOS.IM.Server.Service;

public class GlobalWebApiAppDALInstanceSelector
{
	public static Func<IDtoDAL> GetDtoDALImplementation;

	public static Func<IDtoDAL> GetDtoDALImplementationForBackground;
}
