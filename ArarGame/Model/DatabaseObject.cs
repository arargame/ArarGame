namespace Core.Model
{
    public interface IDbObject
    {
        public string[] Includes { get; }
    }

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
