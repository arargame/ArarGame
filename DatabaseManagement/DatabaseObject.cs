using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace DatabaseManagement
{
    public interface IDatabaseObject : IBaseObject
    {
        string Alias { get; set; }

        DatabaseObject SetAlias(string alias);
    }

    public abstract class DatabaseObject : BaseObject,IDatabaseObject
    {
        #region Properties

        public string Alias { get; set; }

        #endregion

        #region Collections



        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor



        #endregion

        #region SetFunctions

        public DatabaseObject SetAlias(string alias)
        {
            Alias = alias;

            return this;
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
