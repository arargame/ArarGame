using System;
using System.Collections.Generic;
using Core.Model;

namespace DatabaseManagement
{
    public interface ITable : IDatabaseObject
    {
        List<IMetaColumn> MetaColumns { get; set; }

        List<IJoinTable> JoinTables { get; set; }
    }

    public interface IJoinTable : ITable
    {
        ITable LeftTable { get; set; }

        IJoinTable SetLeftTable(ITable table);
    }

    public class Table : DatabaseObject, ITable
    {
        #region Properties



        #endregion

        #region Collections

        public List<IMetaColumn> MetaColumns { get; set; }

        public List<IJoinTable> JoinTables { get; set; }

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

        public ITable LoadJoinTables(List<JoinTable> joinTables)
        {
            JoinTables.AddRange(joinTables);

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
