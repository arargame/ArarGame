using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public interface IClassInfo
    {
        string FullName { get; set; }
    }

    public class ClassInfo : BaseObject
    {

        #region Properties

        public string FullName { get; private set; }

        public Type Type { get; set; }

        #endregion

        #region Collections

        public List<Property> Properties { get; set; }

        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor

        public ClassInfo()
        {

        }

        public ClassInfo(string typeName)
        {

        }

        public ClassInfo(Type type)
        {

        }

        #endregion

        #region SetFunctions

        public ClassInfo SetFullName(string fullName)
        {
            FullName = fullName;

            return this;
        }


        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }

    public class Property : BaseObject
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



        #endregion

        #region Functions



        #endregion

        #region StaticFunctions



        #endregion
    }
}
