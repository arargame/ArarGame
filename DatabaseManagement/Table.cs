using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Model;

namespace DatabaseManagement
{
    public interface ITable : IDatabaseObject
    {
        IClassInfo ClassInfo { get; set; }

        List<IMetaColumn> MetaColumns { get; set; }

        List<IJoinTable> JoinTables { get; set; }

        List<IRow> Rows { get; set; }

        public ITable SetMetaColumns(params IMetaColumn[] columns);

        public ITable SetJoinTables(params IJoinTable[] joinTables);

        public ITable SetJoinTables(Expression<Func<Table, List<JoinTable>>> expression);
    }

    public interface IJoinTable : ITable
    {
        ITable LeftTable { get; set; }

        IJoinTable SetLeftTable(ITable table);
    }

    public class Table : DatabaseObject, ITable
    {
        #region Properties

        public IClassInfo ClassInfo { get; set; }

        #endregion

        #region Collections

        public List<IMetaColumn> MetaColumns { get; set; }

        public List<IJoinTable> JoinTables { get; set; }

        public List<IRow> Rows { get; set; }

        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor

        public Table()
        {
            Initialize();
        }

        public Table(string name)
        {
            SetName(name);

            Initialize();
        }

        #endregion

        #region SetFunctions

        public ITable SetJoinTables(params IJoinTable[] joinTables)
        {
            JoinTables.AddRange(joinTables);

            foreach (var joinTable in joinTables)
            {
                joinTable.SetLeftTable(this);
            }

            return this;
        }

        public ITable SetJoinTables(Expression<Func<Table, List<JoinTable>>> expression)
        {
            var joinTables = expression.Compile().Invoke(this);

            return SetJoinTables(joinTables.ToArray());
        }

        public ITable SetMetaColumns(params IMetaColumn[] metaColumns)
        {
            MetaColumns.AddRange(metaColumns);

            foreach (var metaColumn in metaColumns)
            {
                metaColumn.SetTable(this);
            }

            return this;
        }

        #endregion

        #region Functions

        public override IBaseObject Initialize()
        {
            if (MetaColumns == null)
                MetaColumns = new List<IMetaColumn>();

            if (JoinTables == null)
                JoinTables = new List<IJoinTable>();

            if (Rows == null)
                Rows = new List<IRow>();

            return base.Initialize();
        }

        #endregion

        #region StaticFunctions



        #endregion
    }

    public class JoinTable : Table,IJoinTable
    {
        #region Properties

        public ITable LeftTable { get; set; }

        #endregion

        #region Collections



        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor



        #endregion

        #region SetFunctions

        public IJoinTable SetLeftTable(ITable table)
        {
            return this;
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
