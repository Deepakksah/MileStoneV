using System;

namespace VideoOS.Rules.Interfaces;

public abstract class SpecialActionTargets
{
	public static readonly Guid TriggeringDevice = Guid.Empty;

	public static readonly Guid DevicesFromMetaData = new Guid("11111111-1111-1111-1111-111111111111");

	public static readonly Guid AllDevicesStartedByRule = new Guid("22222222-2222-2222-2222-222222222222");

	public static readonly Guid NoneDevice = new Guid("99999999-9999-9999-9999-999999999999");
}
