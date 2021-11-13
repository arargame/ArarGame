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

    }

    public interface IColumn : IMetaColumn
    {

    }

    public class Column : DatabaseObject,IColumn
    {
        #region Properties



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



        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
