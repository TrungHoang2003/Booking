namespace BuildingBlocks.Interfaces;

public interface IEntity
{
    int Id { get; set; }
}

public abstract class Entity : IEntity
{
    public int Id { get;set; }
    
    protected Entity(){}

    protected Entity(int id)
    {
        Id = id;
    }
}