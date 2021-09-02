using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Interfaces
{
    public interface IDbObject
    {
        public string[] Includes { get; }
    }
}
