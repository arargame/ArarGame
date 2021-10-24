using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Model
{
    public interface IBaseObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        Action<Exception> LogAction { get; set; }

        public IBaseObject Initialize();
    }

    public abstract class BaseObject : IBaseObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Action<Exception> LogAction { get; set; }

        public string Text 
        {
            get {
                return Description;
            }
            set {
                Description = value;
            }
        }

        #region Constructor

        public BaseObject()
        {

        }

        #endregion

        #region SetFunctions

        public virtual IBaseObject SetName(string name)
        {
            Name = name;

            return this;
        }

        public virtual IBaseObject SetLogAction(Action<Exception> action)
        {
            LogAction = action;

            return this;
        }

        public virtual IBaseObject SetText(string text)
        {
            Text = text;

            return this;
        }

        #endregion

        #region Functions

        public virtual void Add<T2>(List<T2> collection, Expression<List<T2>> expression,bool twoWayBinding = true)  where T2 : IBaseObject
        {
            List<T2> listProperty = null;

            var member = (expression.Body as MemberExpression).Member;

            var collectionProperty = GetType().GetProperties().FirstOrDefault(p => p.Name == member.Name);

            collectionProperty.PropertyType.GetMethod("AddRange").Invoke(collectionProperty.GetValue(this), new object[] { collection });

            //if (expression != null)
            //{
            //    listProperty = expression.Compile().Invoke((T1)this);

            //    listProperty.AddRange(collection);
            //}
            //else
            //{
            //    var collectionProperty = GetType().GetProperties().FirstOrDefault(p => p.PropertyType.FullName == typeof(List<T2>).FullName);

            //    if (collectionProperty == null)
            //        return;

            //    collectionProperty.PropertyType.GetMethod("AddRange").Invoke(collectionProperty.GetValue(this), new object[] { collection });
            //}

            if (!twoWayBinding)
                return;

            foreach (var item in collection)
            {
                //var navigationProperty = item.GetType().GetProperties().FirstOrDefault(p => p.DeclaringType.Name == typeof(T1).Name);

                //if (navigationProperty == null)
                //    return;

                //navigationProperty.SetValue(item, this);
            }
        }

        public virtual IBaseObject Initialize()
        {
            return this;
        }

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
