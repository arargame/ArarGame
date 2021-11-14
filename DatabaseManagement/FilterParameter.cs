using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace DatabaseManagement
{
    public interface IFilterParameter : IBaseObject
    {
        IFilter Filter { get; set; }

        int Index { get; set; }

        object Value { get; set; }

        IFilterParameter SetFilter(IFilter filter);

        IFilterParameter SetIndex(int index);

        IFilterParameter SetValue(object value);
    }

    public class FilterParameter : BaseObject, IFilterParameter
    {
        #region Properties

        public IFilter Filter { get; set; }
        public int Index { get; set; }

        public object Value { get; set; }

        #endregion

        #region Collections



        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor



        #endregion

        #region SetFunctions

        public IFilterParameter SetFilter(IFilter filter)
        {
            Filter = filter;

            return this;
        }

        public IFilterParameter SetIndex(int index)
        {
            Index = index;

            return this;
        }

        public IFilterParameter SetValue(object value)
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
