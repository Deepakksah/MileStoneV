using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.MediaFoundation;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

namespace VideoOS.Mobile.VideoChannel.Encoding;

public abstract class TransformWrapper : IControl, IDisposable
{
	private readonly Queue<byte[]> _inputData = new Queue<byte[]>();

	private readonly object _inputDataLocker = new object();

	private Transform _transformMediaFoundation;

	private readonly CycleTaskMember _processingTask;

	private Sample _outputSample;

	private TOutputDataBuffer _outputBuffer;

	public const int MaxTimeToWaitForStopMs = 10000;

	public Action<byte[]> OnTransformedData;

	private bool _disposed;

	protected bool NeedsTransformRecreation { get; set; } = true;

	protected TransformWrapper()
	{
		_processingTask = new CycleTaskMember((Action)ProcessingMethod, (Action)OnProcessingStart, (Action)OnProcessingStop, 10);
	}

	public void Start()
	{
		_processingTask.StartTask();
	}

	public void Stop()
	{
		_processingTask.SignalTaskToStop();
	}

	public void StopAndWait()
	{
		Stop();
		_processingTask.WaitTaskToStop(10000);
	}

	public void OnInputData(byte[] data)
	{
		lock (_inputDataLocker)
		{
			_inputData.Enqueue(data);
		}
	}

	protected abstract Transform CreateTransform();

	private void RecreateTransform()
	{
		_transformMediaFoundation = CreateTransform();
		_transformMediaFoundation.SignalInputDataStarting();
	}

	private void OnProcessingStart()
	{
		SampleExtensions.InitOutputMediaSample(out _outputSample, out _outputBuffer);
	}

	private void OnProcessingStop()
	{
		_transformMediaFoundation.SignalInputDataEnd();
		ProcessOutput();
		Disposing.DisposeMember(ref _transformMediaFoundation);
	}

	private void ProcessingMethod()
	{
		CheckIfTransformRecreationIsNeeded();
		ProcessInput(_transformMediaFoundation);
		ProcessOutput();
	}

	private void CheckIfTransformRecreationIsNeeded()
	{
		if (NeedsTransformRecreation)
		{
			NeedsTransformRecreation = false;
			if (_transformMediaFoundation != null)
			{
				OnProcessingStop();
			}
			RecreateTransform();
		}
	}

	protected virtual void SetInputSampleProperties(Sample inputSample)
	{
	}

	private bool ProcessInput(Transform audioEncoder)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		byte[] array = GetNextData();
		try
		{
			if (array != null)
			{
				Sample mediaSample = SampleExtensions.GetMediaSample(array);
				try
				{
					SetInputSampleProperties(mediaSample);
					audioEncoder.ProcessInput(0, mediaSample, 0);
				}
				finally
				{
					((IDisposable)mediaSample)?.Dispose();
				}
			}
		}
		catch (SharpDXException ex)
		{
			Log.Exception((Exception)ex);
			if (ex.ResultCode != ResultDescriptor.op_Implicit(ResultCode.NotAccepting))
			{
				array = null;
			}
		}
		if (array != null)
		{
			DequeueNextData();
		}
		return array != null;
	}

	private byte[] GetNextData()
	{
		lock (_inputDataLocker)
		{
			if (_inputData.Count > 0)
			{
				return _inputData.Peek();
			}
		}
		return null;
	}

	private void DequeueNextData()
	{
		lock (_inputDataLocker)
		{
			if (_inputData.Count > 0)
			{
				_inputData.Dequeue();
			}
		}
	}

	private void ProcessOutput()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_transformMediaFoundation.ProcessOutput(_outputBuffer, _outputSample, OnTransformedData);
	}

	~TransformWrapper()
	{
		Dispose(disposing: true);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				StopAndWait();
				Disposing.DisposeMember(ref _outputSample);
				Disposing.DisposeMember(ref _transformMediaFoundation);
			}
			_disposed = true;
		}
	}
}
