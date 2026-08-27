using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Utilities;

namespace VideoOS.Mobile.VideoSource;

public class RsStreamMuxer<TData> : IRsStream<TData>, IRsStreamSource<TData>, IControl
{
	public const int ClockTicksPerSeconds = 20;

	private readonly Clock _clock = new Clock(DateTime.UtcNow, ClockDirection.Forward, 20);

	private readonly List<RsStreamMuxerItem<TData>> _sources = new List<RsStreamMuxerItem<TData>>();

	private readonly IRsStreamMuxerImplementer<TData> _implementer;

	public OnStreamData<TData> OnData { get; set; }

	public OnEventData OnEvent { get; set; }

	public OnFailCallback OnFail { get; set; }

	public bool IsPaused { get; set; }

	public RsStreamMuxer(IRsStreamMuxerImplementer<TData> implementer)
	{
		_clock.Tick += ClockOnTick;
		_implementer = implementer;
	}

	private void ClockOnTick(DateTime dateTime)
	{
		Process();
	}

	private void Process()
	{
		List<IRsStreamMuxerItemConsumer<TData>> datas = _sources.Select((RsStreamMuxerItem<TData> source) => source.ConsumerIface).ToList();
		IList<IRsStreamMuxerItemConsumer<TData>> list = _implementer.Process(datas, OnData);
		for (int num = 0; num < Math.Min(_sources.Count, list.Count); num++)
		{
			_sources[num].ConsumerIface = list[num];
		}
	}

	public void SetSource(IRsStream<TData> source)
	{
		_sources.Add(new RsStreamMuxerItem<TData>(source));
		source.OnEvent = (OnEventData)Delegate.Combine(source.OnEvent, new OnEventData(OnSourceEvent));
		source.OnFail = (OnFailCallback)Delegate.Combine(source.OnFail, new OnFailCallback(OnSourceFail));
	}

	private void OnSourceFail(object data)
	{
		OnFail?.Invoke(data);
	}

	private void OnSourceEvent(object data)
	{
		OnEvent?.Invoke(data);
	}

	public void Start()
	{
		_clock.RestartFromTime(DateTime.UtcNow, ClockDirection.Forward, 20);
	}

	public void Stop()
	{
		_clock.Stop();
	}
}
