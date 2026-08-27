using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VideoOS.Management.Common;

public abstract class MaskingMethod
{
	private PrivacyMask _parent;

	public MaskType MaskType { get; }

	public int Id { get; }

	public bool IncludedInMotionDetection { get; }

	public bool Removable { get; }

	public IEnumerable<PrivacyMask.Cell> Cells => _parent.Cells.Where((PrivacyMask.Cell c) => c.MethodId == Id).ToArray();

	protected MaskingMethod(PrivacyMask parent, int methodId, MaskType maskType, bool includeInMotionDetection, bool removable)
	{
		_parent = parent;
		Id = methodId;
		MaskType = maskType;
		IncludedInMotionDetection = includeInMotionDetection;
		Removable = removable;
	}

	public void AddCell(Point point)
	{
		AddCell(point.X, point.Y);
	}

	public void AddCell(int x, int y)
	{
		_parent.CellList.Add(x, y, Id);
	}

	public void AddCells(IEnumerable<Point> cellCoordinates)
	{
		_parent.CellList.AddRange(cellCoordinates, Id);
	}

	public void RemoveCell(Point point)
	{
		RemoveCell(point.X, point.Y);
	}

	public void RemoveCell(int x, int y)
	{
		_parent.CellList.Remove(x, y, Id);
	}
}
