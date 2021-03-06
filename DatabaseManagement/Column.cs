using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace DatabaseManagement
{
    public interface IMetaColumn : IDatabaseObject
    {
        ITable Table { get; set; }

        bool IsSelectable { get; set; }

        bool IsSelected { get; set; }

        bool IsOrderable { get; set; }

        bool IsOrdered { get; set; }

        bool IsFilterable { get; set; }

        bool IsFiltered { get; set; }

        IFilter Filter { get; set; }

        IMetaColumn SetTable(ITable table);

    }

    public interface IColumn : IDatabaseObject
    {
        IRow Row { get; set; }

        object Value { get; set; }

        IColumn SetRow(IRow row);

        IColumn SetValue(object value);
    }

    public class Column : DatabaseObject,IColumn
    {
        #region Properties

        public IRow Row { get; set; }

        public object Value { get; set; }

        #endregion

        #region Collections



        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor

        public Column()
        {

        }

        #endregion

        #region SetFunctions

        public IColumn SetRow(IRow row)
        {
            Row = row;

            return this;
        }

        public IColumn SetValue(object value)
        {
            Value = value;

            return this;
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
