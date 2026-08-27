using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IMatrixViewDataComponent : IDisposable
{
	IEnumerable<MatrixViewData> QueryMatrixViews();

	MatrixViewData QueryMatrix(string id);

	void UpdateMatrix(MatrixViewData matrix);

	void CreateMatrix(MatrixViewData matrix);

	void DeleteMatrix(string id);
}
