using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.VideoSource;

public class RsStreamProxyGraph<TData> : RsStreamProxy<TData>, IControl, IRsControl, IVideoSourceQuality, IDisposable
{
	private class OutputConnector : IRsStreamSource<TData>
	{
		public RsStreamProxyGraph<TData> Parrent { private get; set; }

		public void SetSource(IRsStream<TData> source)
		{
			Parrent.SetSourceInternal(source);
		}
	}

	private IRsStream<TData> _postProcessor;

	private IRsStreamSource<TData> _multiplexor;

	private readonly List<IRsStream<TData>> _sources = new List<IRsStream<TData>>();

	public void SetPostProcessor(IRsStream<TData> postProcessor)
	{
		_postProcessor = postProcessor;
	}

	public void SetMultiplexor(IRsStreamSource<TData> multiplexor)
	{
		_multiplexor = multiplexor;
	}

	public override void SetSource(IRsStream<TData> source)
	{
		_sources.Add(source);
	}

	~RsStreamProxyGraph()
	{
		Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		Disposing.DisposeMember(ref _postProcessor);
		Disposing.DisposeMember(ref _multiplexor);
		foreach (IRsStream<TData> source in _sources)
		{
			IRsStream<TData> obj = source;
			Disposing.DisposeMember(ref obj);
		}
		base.Dispose(disposing);
	}

	protected void DoOnAll<TO>(Action<TO> onSupported) where TO : class
	{
		foreach (IRsStream<TData> source in _sources)
		{
			DoOnSigle(source, onSupported);
		}
		DoOnSigle(_multiplexor, onSupported);
		DoOnSigle(_postProcessor, onSupported);
	}

	protected void DoOnSigle<TI, TO>(TI stream, Action<TO> onSupported) where TI : class where TO : class
	{
		if (stream is TO obj)
		{
			onSupported?.Invoke(obj);
		}
	}

	public void ConnectNodes()
	{
		IRsStream<TData> rsStream = ConnectNodes(_multiplexor, _sources);
		rsStream = ConnectNodes(_postProcessor, new IRsStream<TData>[1] { rsStream });
		rsStream = ConnectNodes(new OutputConnector
		{
			Parrent = this
		}, new IRsStream<TData>[1] { rsStream });
	}

	protected IRsStream<TData> ConnectNodes<TI>(TI nextNodeIface, IEnumerable<IRsStream<TData>> prevNodes)
	{
		if (nextNodeIface is IRsStreamSource<TData> rsStreamSource)
		{
			foreach (IRsStream<TData> prevNode in prevNodes)
			{
				rsStreamSource.SetSource(prevNode);
			}
			return rsStreamSource as IRsStream<TData>;
		}
		return prevNodes.First();
	}

	public void DisconnectNodes()
	{
	}

	public void Start()
	{
		DoOnAll(delegate(IControl iControl)
		{
			iControl.Start();
		});
	}

	public void Stop()
	{
		DoOnAll(delegate(IControl iControl)
		{
			iControl.Stop();
		});
	}

	public bool DoOnAllRsControl(Func<IRsControl, bool> rsCtrlAction)
	{
		bool result = false;
		DoOnAll(delegate(IRsControl iRsControl)
		{
			result |= rsCtrlAction(iRsControl);
		});
		return result;
	}

	public bool CreateRsChannel()
	{
		return DoOnAllRsControl((IRsControl iRsControl) => iRsControl.CreateRsChannel());
	}

	public bool InitRsChannel()
	{
		return DoOnAllRsControl((IRsControl iRsControl) => iRsControl.InitRsChannel());
	}

	public bool CloseRsChannel()
	{
		return DoOnAllRsControl((IRsControl iRsControl) => iRsControl.CloseRsChannel());
	}

	public ConnectionQuality GetInputConnectionQuality()
	{
		uint recivedCount = 0u;
		ConnectionQuality result = new ConnectionQuality();
		result.Initialize();
		DoOnAll(delegate(IVideoSourceQuality srcQuality)
		{
			recivedCount++;
			result.Append(srcQuality.GetInputConnectionQuality());
		});
		if (recivedCount != 0)
		{
			result.FPSInTausents /= recivedCount;
		}
		return result;
	}
}
