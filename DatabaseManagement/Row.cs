using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace DatabaseManagement
{
    public interface IRow : IBaseObject
    {
        List<IColumn> Columns { get; set; }

        IRow SetColumns(params IColumn[] columns);
    }
    
    public class Row : BaseObject,IRow
    {
        #region Properties



        #endregion

        #region Collections

        public List<IColumn> Columns { get; set; }

        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor

        public Row()
        {

        }

        #endregion

        #region SetFunctions

        public IRow SetColumns(params IColumn[] columns)
        {
            Columns = columns.ToList();

            Columns.ForEach(c => c.SetRow(this));

            return this;
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
