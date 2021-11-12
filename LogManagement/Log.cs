using System;
using System.Linq.Expressions;
using Core.Model;

namespace LogManagement
{
    public interface ILog : IBaseObject
    {

    }

    public class Log : BaseObject, ILog
    {
        #region Properties



        #endregion

        #region Collections



        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor



        #endregion

        #region SetFunctions

        public new ILog Set(Expression<Func<IBaseObject, object>> property,object value)
        {
            return (ILog)base.Set(property, value);
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
