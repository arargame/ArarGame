using Core.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public abstract class DatabaseObject : BaseObject, IDbObject
    {
        public virtual string[] Includes
        {
            get
            {
                return null;
            }
        }
    }
}
