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
