using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Core.Utils;

namespace Core.Model
{
    public interface IClassInfo : IBaseObject
    {
        Assembly Assembly { get; set; }

        string FullName { get; set; }

        Type Type { get; set; }

        List<Property> Properties { get; set; }

        ClassInfo AddProperty(Property property);

        Property GetPropertyByName(string name);

        public ClassInfo LoadProperties(List<Property> properties);

        ClassInfo SetAssembly(Assembly assembly);

        public ClassInfo SetFullName(string fullName);

        public ClassInfo SetType(Type type);
    }

    public class ClassInfo : BaseObject
    {

        #region Properties

        public string FullName { get; set; }

        public Assembly Assembly { get; set; }

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
            var assembly = type.Assembly;

            SetAssembly(assembly);

            SetName(type.Name);

            SetFullName(type.FullName);
        }

        #endregion

        #region SetFunctions

        public ClassInfo AddProperty(Property property)
        {
            Initialize();

            Properties.Add(property);

            return this;
        }

        public ClassInfo LoadProperties(List<Property> properties)
        {
            Initialize();

            Properties.AddRange(properties);

            return this;
        }

        public ClassInfo SetAssembly(Assembly assembly)
        {
            Assembly = assembly;

            return this;
        }

        public ClassInfo SetFullName(string fullName)
        {
            FullName = fullName;

            return this;
        }

        public ClassInfo SetType(Type type)
        {
            Type = type;

            return this;
        }

        #endregion

        #region Functions

        public Property GetPropertyByName(string name)
        {
            return Properties.SingleOrDefault(p => p.Name == name);
        }

        public override IBaseObject Initialize()
        {
            if (Properties == null)
                Properties = new List<Property>();

            return base.Initialize();
        }

        #endregion

        #region StaticFunctions

        public static ClassInfo CreateFromObject(IBaseObject baseObject)
        {
            var classInfo =  new ClassInfo(baseObject.GetType())
                    .LoadProperties(Property.LoadPropertiesFromObject(baseObject));

            classInfo.Properties.ForEach(p=>p.SetClassInfo(classInfo));

            return classInfo;
        }

        #endregion
    }
}
