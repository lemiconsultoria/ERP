namespace ERP.Core.Entities
{
    public abstract class Entity : IEntity
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        protected Entity()
        {
            Id = 0;
        }
    }
}