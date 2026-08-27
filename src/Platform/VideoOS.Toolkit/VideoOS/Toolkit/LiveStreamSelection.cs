using System;
using System.Runtime.InteropServices;
using NmToolkit;

namespace VideoOS.Toolkit;

public class LiveStreamSelection : IDisposable
{
	private void _007ELiveStreamSelection()
	{
	}

	public unsafe static LiveStreamSelection CreateLiveStreamSelection_AdaptToResolution(int widthHint, int heightHint)
	{
		//IL_001a: Expected I, but got I8
		CmAdaptiveStreamingByResolution* ptr = (CmAdaptiveStreamingByResolution*)global::_003CModule_003E.@new(16uL);
		CmAdaptiveStreamingByResolution* liveStreamSelection;
		try
		{
			liveStreamSelection = ((ptr == null) ? null : global::_003CModule_003E.NmToolkit_002ECmAdaptiveStreamingByResolution_002E_007Bctor_007D(ptr, widthHint, heightHint));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 16uL);
			throw;
		}
		return new InternalLiveStreamSelection((ImStreamSelection*)liveStreamSelection);
	}

	public unsafe static LiveStreamSelection CreateLiveStreamSelection_MaxResolution()
	{
		//IL_0017: Expected I, but got I8
		CmMaxResolution* ptr = (CmMaxResolution*)global::_003CModule_003E.@new(8uL);
		CmMaxResolution* liveStreamSelection;
		try
		{
			liveStreamSelection = ((ptr == null) ? null : global::_003CModule_003E.NmToolkit_002ECmMaxResolution_002E_007Bctor_007D(ptr));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 8uL);
			throw;
		}
		return new InternalLiveStreamSelection((ImStreamSelection*)liveStreamSelection);
	}

	public unsafe static LiveStreamSelection CreateLiveStreamSelection_DefaultStream()
	{
		//IL_0017: Expected I, but got I8
		CmDefaultStream* ptr = (CmDefaultStream*)global::_003CModule_003E.@new(8uL);
		CmDefaultStream* liveStreamSelection;
		try
		{
			liveStreamSelection = ((ptr == null) ? null : global::_003CModule_003E.NmToolkit_002ECmDefaultStream_002E_007Bctor_007D(ptr));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 8uL);
			throw;
		}
		return new InternalLiveStreamSelection((ImStreamSelection*)liveStreamSelection);
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (!A_0)
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(A_0: true);
		GC.SuppressFinalize(this);
	}
}
