using System.Collections.Generic;
using OsosOracle.Framework.DataAccess;
namespace OsosOracle.Entities.ComplexType.RPTDASHBOARDComplexTypes
{
	public class RPTDASHBOARDDataTable : DataTable
	{
		public List<RPTDASHBOARDDetay> RPTDASHBOARDDetayList { get; set; }
	}
}