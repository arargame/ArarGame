using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace DatabaseManagement
{
    public interface IFilter : IBaseObject
    {
        List<IFilterParameter> Parameters { get; set; }

        IFilter SetParameters(params IFilterParameter[] parameters);
    }

    public interface IJoinFilter : IFilter
    {

    }

    public class Filter : BaseObject, IFilter
    {
        #region Properties



        #endregion

        #region Collections

        public List<IFilterParameter> Parameters { get; set; }

        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor



        #endregion

        #region SetFunctions

        public IFilter SetParameters(params IFilterParameter[] parameters)
        {
            Parameters.AddRange(parameters);

            Parameters.ForEach(p => p.SetFilter(this));

            return this;
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
