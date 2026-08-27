using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class Box : IBoxParse, IBox
{
	private uint _dataStartIndex;

	private byte[] _databuffer;

	public uint Size => HeaderSize + DataSize + ChildrenSize;

	public string BoxType { get; protected set; }

	public IBoxParse Parent { get; set; }

	public IList<IBoxParse> Children { get; }

	public virtual bool CanHaveChildren => false;

	public virtual bool CanHaveData => false;

	public uint HeaderSize { get; protected set; }

	public virtual uint DataSize { get; private set; }

	public uint ChildrenSize => (uint)Children.Sum((IBoxParse box) => box.Size);

	public Box()
	{
		HeaderSize = 8u;
		BoxType = "    ";
		Parent = null;
		Children = new List<IBoxParse>();
	}

	public uint InitFromBuffer(byte[] buffer, uint startIndex, uint maxSize, IBoxFactory boxFactory)
	{
		uint num = 0u;
		uint size = 0u;
		string type = "    ";
		HeaderSize = ParseHeader(buffer, startIndex, maxSize, ref size, ref type);
		num += HeaderSize;
		BoxType = type;
		DataSize = ParseData(buffer, startIndex + num, size - num);
		num += DataSize;
		uint num2 = ParseChildren(buffer, startIndex + num, size - num, boxFactory);
		num += num2;
		if (num != size)
		{
			Logger.Instance.Log(LogContent.Error, "Box.InitFromBuffer()", "Parsed data is less than box size");
		}
		return size;
	}

	public void WriteToStream(Stream stream)
	{
		WriteHeader(stream);
		WriteData(stream);
		WriteChildren(stream);
	}

	public virtual void AddChild(IBoxParse child)
	{
		Children.Add(child);
		child.Parent = this;
	}

	protected virtual uint ParseHeader(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint size, ref string type)
	{
		uint result = BoxHelper.ParseSizeAndType(buffer, startIndex, restInBoxSize, ref size, ref type);
		if (BoxType != type && BoxType != "    ")
		{
			throw new Exception("Parse error");
		}
		return result;
	}

	protected virtual uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		if (!CanHaveData)
		{
			return 0u;
		}
		_dataStartIndex = startIndex;
		_databuffer = buffer;
		return restInBoxSize;
	}

	protected virtual uint ParseChildren(byte[] buffer, uint startIndex, uint restInBoxSize, IBoxFactory boxFactory)
	{
		if (!CanHaveChildren)
		{
			return 0u;
		}
		uint result = restInBoxSize;
		while (restInBoxSize != 0)
		{
			uint size = 0u;
			string type = "    ";
			BoxHelper.ParseSizeAndType(buffer, startIndex, restInBoxSize, ref size, ref type);
			IBoxParse boxParse = boxFactory.CreateBox(type);
			uint num = boxParse.InitFromBuffer(buffer, startIndex, restInBoxSize, boxFactory);
			restInBoxSize -= num;
			startIndex += num;
			boxParse.Parent = this;
			Children.Add(boxParse);
		}
		return result;
	}

	protected virtual void WriteHeader(Stream stream)
	{
		stream.Write(BoxHelper.Int32ToBuffer(Size));
		stream.Write(BoxHelper.BoxTypeToBuffer(BoxType));
	}

	protected virtual void WriteData(Stream stream)
	{
		if (_databuffer != null && DataSize != 0)
		{
			stream.Write(_databuffer, (int)_dataStartIndex, (int)DataSize);
		}
	}

	protected virtual void WriteChildren(Stream stream)
	{
		foreach (IBoxParse child in Children)
		{
			child.WriteToStream(stream);
		}
	}

	public override string ToString()
	{
		return BoxType + " (" + Size + ", " + GetType().Name + ")";
	}
}
