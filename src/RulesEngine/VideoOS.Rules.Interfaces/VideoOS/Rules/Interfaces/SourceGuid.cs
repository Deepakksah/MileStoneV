using System;

namespace VideoOS.Rules.Interfaces;

public static class SourceGuid
{
	public static Guid PeriodicTimerEvent { get; } = new Guid("F07AC0E3-9662-4781-9176-EC9E488AA494");

	public static Guid TimeInterval => PeriodicTimerEvent;

	public static Guid RelativeTimerEvent { get; } = new Guid("6A00FD9A-A7F1-446C-ABC2-29639CECD595");

	public static Guid Timer => RelativeTimerEvent;

	public static Guid Server { get; } = new Guid("1DBB5909-6464-4cfa-ACE0-35A380BEDF83");

	public static Guid External { get; } = new Guid("E8BD6CEE-1119-4296-BA91-E3803E2C591E");

	public static Guid AnySource { get; } = new Guid("318D9CB3-F315-4FB0-A4F4-1BC9AFC4DAC8");

	public static Guid AccessControl { get; } = new Guid("FE9AA43F-508C-469F-93E7-94AB47F8E823");
}
