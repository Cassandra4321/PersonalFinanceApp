namespace UserService.Core.Common
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; } = default!;
    }
}
