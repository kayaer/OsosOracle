using System.Collections.Generic;
using OsosOracle.Framework.DataAccess;
namespace OsosOracle.Entities.ComplexType.CONDILComplexTypes
{
	public class CONDILDataTable : DataTable
	{
		public List<CONDILDetay> CONDILDetayList { get; set; }
	}
}