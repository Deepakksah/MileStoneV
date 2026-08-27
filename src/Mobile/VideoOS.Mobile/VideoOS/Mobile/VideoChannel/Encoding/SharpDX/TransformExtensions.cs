using System;
using System.Collections.Generic;
using SharpDX.MediaFoundation;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class TransformExtensions
{
	public delegate bool GetAvailableTypeDelegate(int typeIndex, out MediaType outMediaType);

	public static IEnumerable<MediaType> GetInputAvailableTypes(this Transform transform, int streamNumber)
	{
		return GetAvailableTypes(delegate(int typeIndex, out MediaType outMediaType)
		{
			outMediaType = null;
			try
			{
				transform.GetInputAvailableType(streamNumber, typeIndex, ref outMediaType);
			}
			catch (Exception)
			{
			}
			return outMediaType != null;
		}, "Input");
	}

	public static IEnumerable<MediaType> GetOutputAvailableTypes(this Transform transform, int streamNumber)
	{
		return GetAvailableTypes(delegate(int typeIndex, out MediaType outMediaType)
		{
			return transform.TryGetOutputAvailableType(streamNumber, typeIndex, ref outMediaType);
		}, null);
	}

	public static IEnumerable<MediaType> GetAvailableTypes(GetAvailableTypeDelegate getter, string printNames)
	{
		List<MediaType> list = new List<MediaType>();
		MediaType outMediaType;
		for (int i = 0; i < int.MaxValue && getter(i, out outMediaType); i++)
		{
			list.Add(outMediaType);
		}
		if (!string.IsNullOrWhiteSpace(printNames))
		{
			MediaTypeHelper.PrintMediaTypes(list, printNames + " available media types");
		}
		return list;
	}

	public static void GetBuffersRequirement(this Transform transform)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		TOutputStreamInformation val = default(TOutputStreamInformation);
		transform.GetOutputStreamInfo(0, ref val);
		((Enum)(object)(MftOutputStreamInformationFlags)256).IsSetIn(val.DwFlags);
		TInputStreamInformation val2 = default(TInputStreamInformation);
		transform.GetInputStreamInfo(0, ref val2);
		((Enum)(object)(MftInputStreamInformationFlags)256).IsSetIn(val2.DwFlags);
		((Enum)(object)(MftInputStreamInformationFlags)8).IsSetIn(val2.DwFlags);
		((Enum)(object)(MftInputStreamInformationFlags)2048).IsSetIn(val2.DwFlags);
	}

	public static void SignalInputDataStarting(this Transform transform)
	{
		transform.ProcessMessage((TMessageType)268435456, IntPtr.Zero);
	}

	public static void SignalInputDataEnd(this Transform transform)
	{
		transform.ProcessMessage((TMessageType)268435458, IntPtr.Zero);
		transform.ProcessMessage((TMessageType)1, IntPtr.Zero);
	}

	public static void ProcessOutput(this Transform audioEncoder, TOutputDataBuffer outputBuffer, Sample outputSample, Action<byte[]> onOutData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		TransformProcessOutputStatus val = default(TransformProcessOutputStatus);
		while (flag)
		{
			flag = audioEncoder.ProcessOutput((TransformProcessOutputFlags)0, (TOutputDataBuffer[])(object)new TOutputDataBuffer[1] { outputBuffer }, ref val);
			if (!flag)
			{
				continue;
			}
			for (int i = 0; i < outputSample.BufferCount; i++)
			{
				byte[] array = outputSample.GetBufferByIndex(0).CopyData();
				if (array.Length != 0)
				{
					onOutData?.Invoke(array);
				}
			}
		}
	}
}
