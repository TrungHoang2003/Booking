namespace BuildingBlocks.Interfaces;

public interface IEntity;

public abstract class Entity : IEntity
{
    public int Id { get;set; }
    
    protected Entity(){}

    protected Entity(int id)
    {
        Id = id;
    }
}