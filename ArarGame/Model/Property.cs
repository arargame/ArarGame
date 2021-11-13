using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Model
{
    public interface IProperty : IBaseObject
    {
        ClassInfo ClassInfo { get; set; }

        object Value { get; set; }

        Property SetClassInfo(ClassInfo classInfo);
    }

    public class Property : BaseObject, IProperty
    {
        #region Properties

        public ClassInfo ClassInfo { get; set; }

        public object Value { get; set; }

        #endregion

        #region Collections



        #endregion

        #region CalculatedProperties




        #endregion

        #region Constructor

        public Property()
        {

        }

        public Property(string name,object value)
        {
            SetName(name);

            SetValue(value);
        }

        #endregion

        #region SetFunctions

        public Property SetClassInfo(ClassInfo classInfo)
        {
            ClassInfo = classInfo;

            return this;
        }

        public Property SetValue(object value)
        {
            Value = value;

            return this;
        }

        #endregion

        #region Functions



        #endregion

        #region StaticFunctions

        public static List<Property> LoadPropertiesFromObject(IBaseObject baseObject)
        {
            return baseObject.GetProperties().Select(p => new Property(p.Name,p.GetValue(baseObject))).ToList();
        } 

        #endregion
    }
}
