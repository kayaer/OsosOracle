using OsosOracle.Framework.DataAccess.Filter;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.SharedModels
{
    /// <summary>
    /// Jquery Datatable handling model
    /// </summary>
    public class DtParameterModel
    {
        /// <summary>
        /// Draw counter. This is used by DataTables to ensure that the Ajax returns from 
        /// server-side processing requests are drawn in sequence by DataTables 
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Paging first record indicator. This is the start point in the current data set 
        /// (0 index based - i.e. 0 is the first record)
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw. It is expected
        /// that the number of records returned will be equal to this number, unless the 
        /// server has fewer records to return. Note that this can be -1 to indicate that 
        /// all records should be returned (although that negates any benefits of 
        /// server-side processing!)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Global Search for the table
        /// </summary>
        public DtSearch Search { get; set; }

        /// <summary>
        /// Collection of all column indexes and their sort directions
        /// </summary>
        public List<DtOrder> Order { get; set; }

        /// <summary>
        /// Collection of all columns in the table
        /// </summary>
        public List<DtColumn> Columns { get; set; }


        public string OrderColumnName
        {
            get { return Columns[Order[0].Column].Data.Replace('_', '.'); }
        }


        public EnumSiralamaTuru OrderDirection
        {
            get
            {

                return Order[0].Dir.ToEnum<EnumSiralamaTuru>();
                //    return Order[0].Dir;
            }
        }

        public Ara AramaKriteri
        {
            get
            {
                var arama = new Ara()
                {
                    Baslangic = Start,
                    Uzunluk = Length,
                    Siralama = new List<Siralama>()
                };

                foreach (var ord in Order)
                {

                    //arama.Siralama.Add(new Siralama() {SiralamaTipi = ord.Dir.ToEnum<EnumSiralamaTuru>(),KolonAdi = Columns[Order[ord.Column].Column].Data.Replace('_', '.')
                    arama.Siralama.Add(new Siralama() { SiralamaTipi = ord.Dir.ToEnum<EnumSiralamaTuru>(), KolonAdi = Columns[Order[Order.IndexOf(ord)].Column].Data.Replace('_', '.') });
                }



                return arama;
            }
        }
    }

    /// <summary>
    /// Represents search values entered into the table
    /// </summary>
    public sealed class DtSearch
    {
        /// <summary>
        /// Global search value. To be applied to all columns which have searchable as true
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// true if the global filter should be treated as a regular expression for advanced 
        /// searching, false otherwise. Note that normally server-side processing scripts 
        /// will not perform regular expression searching for performance reasons on large 
        /// data sets, but it is technically possible and at the discretion of your script
        /// </summary>
        public bool Regex { get; set; }
    }

    /// <summary>
    /// Represents a column and it's order direction
    /// </summary>
    public sealed class DtOrder
    {
        /// <summary>
        /// Column to which ordering should be applied. This is an index reference to the 
        /// columns array of information that is also submitted to the server
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Ordering direction for this column. It will be asc or desc to indicate ascending
        /// ordering or descending ordering, respectively
        /// </summary>
        public string Dir { get; set; }
    }

    /// <summary>
    /// Represents an individual column in the table
    /// </summary>
    public sealed class DtColumn
    {
        /// <summary>
        /// Column's data source
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Column's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false)
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        /// Search to apply to this specific column.
        /// </summary>
        public DtSearch Search { get; set; }
    }
}
