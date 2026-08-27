using System;
using System.IO;
using System.Text;
using System.Threading;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class AudioStreamPlayer : IAudioRecorder
{
	private bool _readFormatFromStream;

	private const double _outgoingAudioMeterAmplifyingFactor = 1.333;

	private const string RiffChunkId = "RIFF";

	private const string WaveChunkId = "WAVE";

	private const string FmtChunkId = "fmt ";

	private const string DataChunkId = "data";

	private const int ExtendedFmtSize = 18;

	private int _bytesRead;

	private int _forwardSampleBlockSize = 128;

	private int _bufferFactor = 2;

	private Thread _captureThread;

	private bool _capturing;

	private volatile bool _captureThreadRunning;

	private readonly object _captureThreadLock = new object();

	private short _blockAlign;

	private Stream _audioStream;

	private BinaryReader _reader;

	public virtual short BlockAlign
	{
		get
		{
			return _blockAlign;
		}
		set
		{
			if (_capturing)
			{
				throw new InvalidOperationException();
			}
			_blockAlign = value;
		}
	}

	public virtual int DataSize { get; set; }

	public double Progress
	{
		get
		{
			if (DataSize > 0)
			{
				return (double)_bytesRead / (double)DataSize;
			}
			return -1.0;
		}
	}

	public virtual int ForwardSampleBlockSize
	{
		get
		{
			return _forwardSampleBlockSize;
		}
		set
		{
			if (_capturing)
			{
				throw new InvalidOperationException();
			}
			_forwardSampleBlockSize = value;
		}
	}

	public event EventHandler ProgressCompleteEvent;

	public AudioStreamPlayer(Stream audioStream, bool readFormatFromStream = true)
	{
		_audioStream = audioStream;
		_readFormatFromStream = readFormatFromStream;
		DataSize = 0;
		OutgoingAudioWaveFormatSamplesPerSecond = 8000;
		OutgoingWaveFormatBitsPerSample = 16;
		OutgoingWaveFormatChannels = 1;
		new TrackPageViewData(typeof(AudioStreamPlayer)).Send();
	}

	public void ReadFormatFromStream()
	{
		if (_capturing)
		{
			throw new InvalidOperationException();
		}
		if (_audioStream == null)
		{
			throw new InvalidParameterMIPException("Missing input stream!");
		}
		if (_reader == null)
		{
			_reader = new BinaryReader(_audioStream);
		}
		ValidateWavHeader();
		bool flag = false;
		bool flag2 = false;
		while (_audioStream.Position < _audioStream.Length)
		{
			string text = Encoding.ASCII.GetString(_reader.ReadBytes(4));
			int num = _reader.ReadInt32();
			if (!(text == "fmt "))
			{
				if (text == "data")
				{
					if (flag2)
					{
						throw new InvalidParameterMIPException("Multiple data chunks encountered while parsing .wav file");
					}
					ProcessDataChunk(num);
					return;
				}
				_reader.ReadBytes(num);
			}
			else
			{
				if (flag)
				{
					throw new InvalidParameterMIPException("Multiple fmt chunks encountered while parsing .wav file");
				}
				ProcessFmtChunk(num);
				flag = true;
			}
		}
		if (!flag || !flag2)
		{
			throw new InvalidParameterMIPException("Required chunk(s) missing while processing .wav file");
		}
	}

	public override bool Initialize()
	{
		if (_audioStream == null)
		{
			throw new InvalidParameterMIPException("Missing input stream!");
		}
		if (_reader == null)
		{
			_reader = new BinaryReader(_audioStream);
		}
		if (_readFormatFromStream)
		{
			ReadFormatFromStream();
		}
		return true;
	}

	public override void Free()
	{
		StopRecording();
	}

	public override void StartRecording()
	{
		EnvironmentManager.Instance.Log(error: false, "AudioStreamPlayer", "Start playing ");
		lock (_captureThreadLock)
		{
			if (!_captureThreadRunning)
			{
				_captureThreadRunning = true;
				_captureThread = new Thread(CaptureThread);
				_captureThread.Name = "AudioStreamPlayer CaptureThread";
				_captureThread.IsBackground = true;
				_captureThread.Start();
			}
		}
	}

	public override void StopRecording()
	{
		_capturing = false;
	}

	public override bool IsRecording()
	{
		if (_capturing)
		{
			return _captureThreadRunning;
		}
		return false;
	}

	public override int CalcMeterValue(byte[] data, int dataLength)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		for (int i = 0; i < dataLength / OutgoingWaveFormatBitsPerSample; i += 5)
		{
			num = 0.0;
			for (int j = 0; j < OutgoingWaveFormatBitsPerSample / 8; j++)
			{
				num += (double)(data[i * OutgoingWaveFormatBitsPerSample + j] << j * 8);
			}
			if (num > (double)(1 << OutgoingWaveFormatBitsPerSample - 1))
			{
				num -= (double)(1 << OutgoingWaveFormatBitsPerSample);
			}
			num2 += num * num;
			num3 += 1.0;
		}
		int num4 = (int)(Math.Sqrt(num2 / num3) / (double)(1 << OutgoingWaveFormatBitsPerSample - 1) * 100.0 * 1.333);
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (num4 > 100)
		{
			num4 = 100;
		}
		return num4;
	}

	private void SendSampleBlock()
	{
		try
		{
			int num = _forwardSampleBlockSize * BlockAlign;
			if (DataSize > 0 && DataSize - _bytesRead < num)
			{
				num = 0;
				DataSize = _bytesRead;
			}
			if (num <= 0)
			{
				_capturing = false;
				return;
			}
			byte[] array = _reader.ReadBytes(num);
			if (array.Length == 0)
			{
				_capturing = false;
				return;
			}
			FireAudioDataReadyEvent(array);
			_bytesRead += array.Length;
		}
		catch (Exception)
		{
			_capturing = false;
		}
	}

	private void CaptureThread()
	{
		_bytesRead = 0;
		_capturing = true;
		int num = 0;
		while (_capturing && (DataSize <= 0 || _bytesRead < DataSize) && num < _bufferFactor)
		{
			SendSampleBlock();
			num++;
		}
		long num2 = DateTime.Now.Ticks;
		long num3 = Convert.ToInt64(10000000.0 * (double)_forwardSampleBlockSize / (double)OutgoingAudioWaveFormatSamplesPerSecond);
		while (_capturing)
		{
			long ticks = DateTime.Now.Ticks;
			if (ticks < num2)
			{
				Thread.Sleep((int)((num2 - ticks) / 10000));
			}
			SendSampleBlock();
			num2 += num3;
		}
		_reader.Close();
		_reader.Dispose();
		_audioStream.Close();
		_audioStream.Dispose();
		EnvironmentManager.Instance.Log(error: false, "AudioStreamPlayer", "End playing ");
		this.ProgressCompleteEvent?.Invoke(this, new EventArgs());
		lock (_captureThreadLock)
		{
			_captureThreadRunning = false;
		}
	}

	private void ValidateWavHeader()
	{
		byte[] bytes = _reader.ReadBytes(4);
		_reader.ReadInt32();
		byte[] bytes2 = _reader.ReadBytes(4);
		if (!Encoding.ASCII.GetString(bytes).Equals("RIFF"))
		{
			throw new InvalidParameterMIPException("File not RIFF/WAVE formatted");
		}
		if (!Encoding.ASCII.GetString(bytes2).Equals("WAVE"))
		{
			throw new InvalidParameterMIPException("File not RIFF/WAVE formatted");
		}
	}

	private void ProcessFmtChunk(int size)
	{
		_reader.ReadInt16();
		int outgoingWaveFormatChannels = _reader.ReadInt16();
		int outgoingAudioWaveFormatSamplesPerSecond = _reader.ReadInt32();
		_reader.ReadInt32();
		short blockAlign = _reader.ReadInt16();
		short outgoingWaveFormatBitsPerSample = _reader.ReadInt16();
		if (size == 18)
		{
			int count = _reader.ReadInt16();
			_reader.ReadBytes(count);
		}
		OutgoingWaveFormatChannels = outgoingWaveFormatChannels;
		OutgoingWaveFormatBitsPerSample = outgoingWaveFormatBitsPerSample;
		OutgoingAudioWaveFormatSamplesPerSecond = outgoingAudioWaveFormatSamplesPerSecond;
		BlockAlign = blockAlign;
	}

	private void ProcessDataChunk(int size)
	{
		DataSize = size;
		ForwardSampleBlockSize = 256;
	}
}
