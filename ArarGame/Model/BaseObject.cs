using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Core.Utils;

namespace Core.Model
{
    public interface IBaseObject
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }

        DateTime AddedDate { get; set; }

        DateTime ModifiedDate { get; set; }

        Action<Exception> LogAction { get; set; }

        PropertyInfo? GetProperty(string propertyName);

        PropertyInfo[] GetProperties();
        IBaseObject Initialize();

        IBaseObject SetAddedDate(DateTime dateTime);

        IBaseObject SetDescription(string description);

        IBaseObject SetId(Guid id);

        IBaseObject SetLogAction(Action<Exception> action);

        IBaseObject SetModifiedDate(DateTime dateTime);
        IBaseObject SetName(string name);
    }

    public abstract class BaseObject : IBaseObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Action<Exception> LogAction { get; set; }

        #region Constructor

        public BaseObject()
        {

        }

        #endregion

        #region SetFunctions

        public virtual T Set<T>(Expression<Func<T, object>> property, object value) where T : IBaseObject
        {
            if (property.Body is MemberExpression)
            {
                var memberInfo = (property.Body as MemberExpression).Member;

                SetPropertyValue(memberInfo.Name,value);
            }

            return (T)(IBaseObject)this;
        }

        public virtual IBaseObject SetAddedDate(DateTime dateTime)
        {
            AddedDate = dateTime;

            return this;
        }

        public virtual IBaseObject SetId(Guid id)
        {
            Id = id;

            return this;
        }

        public virtual IBaseObject SetDescription(string description)
        {
            Description = description;

            return this;
        }

        public virtual IBaseObject SetLogAction(Action<Exception> action)
        {
            LogAction = action;

            return this;
        }

        public virtual IBaseObject SetModifiedDate(DateTime dateTime)
        {
            ModifiedDate = dateTime;

            return this;
        }

        public virtual IBaseObject SetName(string name)
        {
            Name = name;

            return this;
        }

        #endregion

        #region Functions

        public PropertyInfo? GetProperty(string propertyName)
        {
            return Helper.GetPropertyOf(GetType(),propertyName);
        }

        public PropertyInfo[] GetProperties()
        {
            return Helper.GetPropertiesOf(GetType(), LogAction);
        }

        public virtual IBaseObject Initialize()
        {
            return this;
        }

        public IBaseObject SetPropertyValue(string propertyName,object value)
        {
            GetProperty(propertyName).SetValue(this,value);

            return this;
        }

        #endregion

        #region StaticFunctions

        #endregion
    }

    public static class BaseObjectExtension
    {
        public static IBaseObject Add<Master, Detail,IMaster,IDetail>(this BaseObject baseObject,List<IDetail> collection, Expression<Func<Master, List<IDetail>>> detailReference, Expression<Func<Detail, IMaster>> masterReference) where Master : BaseObject where Detail : BaseObject where IMaster : IBaseObject where IDetail : IBaseObject
        {
            foreach (var item in collection)
            {
                var member = (masterReference.Body as MemberExpression).Member;

                if (member == null)
                    continue;

                var itemProperty = item.GetType().GetProperties().FirstOrDefault(p => p.Name == member.Name);

                if (itemProperty == null)
                    continue;

                itemProperty.SetValue(item, baseObject);

                var details = detailReference.Compile().Invoke(baseObject as Master);

                details.AddRange(collection);
            }

            return baseObject;
        }
    }
}
