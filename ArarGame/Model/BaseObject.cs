using Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public abstract class BaseObject : IBaseObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public BaseObject() { }

        public virtual IBaseObject Initialize()
        {
            return this;
        }
    }
}
