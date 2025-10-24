namespace ProjectIndependence.API.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public Type EntityType { get; set; }
        public object Key { get; set; }

        public NotFoundException(Type entityType, object key)
            : base($"{entityType.Name} with id: {key} was not found")
        {
            EntityType = entityType;
            Key = key;
        }
    }

    public class NotFoundException<TEntity> : NotFoundException
    {
        public NotFoundException(object key)
            : base(typeof(TEntity), key)
        {
            {
            }
        }
    }
}