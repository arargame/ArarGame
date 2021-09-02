using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Interfaces
{
    public interface IBaseObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public IBaseObject Initialize();
    }
}
