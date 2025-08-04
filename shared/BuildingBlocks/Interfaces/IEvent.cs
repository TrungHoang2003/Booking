namespace BuildingBlocks.Interfaces;

public interface IEvent
{
   public Guid CorrelationId { get; set; }
}

public interface IMessage
{
   public Guid CorrelationId { get; set; }
}