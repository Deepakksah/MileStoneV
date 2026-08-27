using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VideoOS.Toolkit;

public interface IRendererToolkit : IToolkit
{
	public enum SourceState
	{
		Disconnected,
		Connecting,
		Connected,
		ConnectionFailed,
		ConnectionLost,
		Reconnecting
	}

	public enum SourceFlags
	{
		LiveNotSupported = 1,
		PlaybackNotSupported = 2,
		NotAuthorized = 4,
		LicenseInvalid = 8,
		MaxNumberOfConnectionsReached = 0x10,
		DewarpingFailed = 0x20
	}

	public enum ImageModel
	{
		BGR32Image,
		D3D9Surface
	}

	public enum DataAvailabilityAtRequestedTime
	{
		ImageAvailable,
		BeforeDatabaseStart,
		AfterDatabaseEnd,
		InGap,
		NoData,
		BlankFiller,
		DecodingFailed
	}

	public enum HardwareAcceleration
	{
		Nvidia,
		Intel,
		Off
	}

	public interface IRenderingInformation : IDisposable
	{
	}

	public interface IImageRenderingInformation : IRenderingInformation
	{
		int GetImageSynchronizationTag();

		ImageModel GetImageModel();

		HardwareAcceleration GetHardwareAccelerationType();

		[return: MarshalAs(UnmanagedType.U1)]
		bool IsLive();

		[return: MarshalAs(UnmanagedType.U1)]
		bool IsFlippedHorizontally();

		[return: MarshalAs(UnmanagedType.U1)]
		bool IsFlippedVertically();

		DateTime GetImageTime();

		DateTime GetNextImageTime();

		DateTime GetPreviousImageTime();

		void GetOriginalImageSize(ref int width, ref int height);

		DataAvailabilityAtRequestedTime GetDataAvailabilityAtRequestedTime();

		string GetVideoCodec();
	}

	public interface IBGR32ImageRenderingInformation : IImageRenderingInformation
	{
		new ImageModel GetImageModel();

		IntPtr GetBuffer();

		void GetImageSize(ref int width, ref int height, ref int stride);
	}

	public interface ID3D9SurfaceRenderingInformation : IImageRenderingInformation
	{
		new ImageModel GetImageModel();

		IntPtr GetSurface();

		void GetImageViewport(ref int left, ref int top, ref int right, ref int bottom);

		void GetWindowViewport(ref int left, ref int top, ref int right, ref int bottom);
	}

	public interface IRenderingHandler
	{
		void HandleRenderedData(Dictionary<int, IRenderingInformation> renderingInformation);
	}

	public interface ISourceStateHandler
	{
		void HandleSourceStateChanged(int sourceId, SourceState sourceState, SourceFlags sourceFlags);
	}

	public interface IDiagnosticsHandler
	{
		void HandleDiagnosticsData(IList<KeyValuePair<string, string>> diagnosticsData);

		void HandleDiagnosticsData(int sourceId, IList<KeyValuePair<string, string>> diagnosticsData);
	}

	public interface IOutOfBandHandler
	{
		void HandleOutOfBandData(int sourceId, IOutOfBandData outOfBandData);
	}

	public enum PerformanceCounterType
	{
		NotPerformanceCounted,
		RateOfCountsPerSecond,
		NumberOfItems
	}

	public interface IDiagnosticsKeyInfo
	{
		PerformanceCounterType GetPerformanceCounterType();

		string GetPerformanceCounterDescription();

		int GetDisplayLevel();
	}

	void AddReferenceToSurface(IntPtr @object);

	void ReleaseReferenceToSurface(IntPtr @object);

	int[] AddSources(KeyValuePair<string, IRenderingParameters>[] sourcesToAdd);

	void RemoveSources(ISet<int> sourcesToRemove);

	void UpdateSourceConfiguration(int sourceId, IList<KeyValuePair<string, string>> configurationParameterList);

	void UpdateSourceConfiguration(int sourceId, string configurationParameterName, string configurationParameterValue);

	void UpdateSourceRenderingParameters(Dictionary<int, IRenderingParameters> renderingParameters);

	void SetImageSynchronizationTag(int sourceId, int tag);

	void ConnectSources(ISet<int> sourcesToConnect);

	void DisconnectSources(ISet<int> sourcesToDisconnect);

	IDigitalZoomController GetDigitalZoomController(int sourceId, string configurationXML);

	Bitmap GetCurrentImage(int sourceId);

	void MoveFirst(int sourceToMove);

	void MoveLast(int sourceToMove);

	void MoveNext(int sourceToMove);

	void MovePrevious(int sourceToMove);

	void MoveNextSequence(int sourceToMove);

	void MovePreviousSequence(int sourceToMove);

	void MoveTo(int sourceToMove, DateTime timeStamp);

	void DoPlayback(int sourceToDoPlayback, DateTime timeStamp, double speed);

	void DoLive(int sourceToDoLive, int maxFrameRate);

	void ClearCache(int source, DateTime beginTimestamp, DateTime endTimestamp);

	void SetRenderingHandler(IRenderingHandler renderingHandler);

	void SetOutOfBandHandler(IOutOfBandHandler outOfBandHandler);

	void SetSourceStateHandler(ISourceStateHandler sourceStateHandler);

	void SetDiagnosticsHandler(IDiagnosticsHandler diagnosticsHandler);

	Dictionary<string, IDiagnosticsKeyInfo> GetDiagnosticsKeyInfo();
}
