using BuildingBlocks.Interfaces;
using Contracts.Events;
using Contracts.Messages;
using MassTransit;
using Property.Domain.Aggregates.BedroomAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.Consumers;

public class AddBedroomConsumer(
    IUnitOfWork unitOfWork,
    IRentalUnitRepository rentalUnitRepo,
    IBedroomRepository bedroomRepo) : IConsumer<AddBedroom>
{
    public async Task Consume(ConsumeContext<AddBedroom> context)
    {
        var rentalUnit = await rentalUnitRepo.GetById(context.Message.RentalUnitId);
        if (rentalUnit == null) throw new Exception("RentalUnitNotFound");

        if (rentalUnit.Type == RentalUnitType.Room())
        {
            var bedroom = context.Message.ListBedrooms.First();

            var newBedroom = new Bedroom(context.Message.RentalUnitId, bedroom.SingleBedCount,
                bedroom.DoubleBedCount,
                bedroom.KingBedCount, bedroom.SofaBedCount);

            await bedroomRepo.Create(newBedroom);
        }

        if (rentalUnit.Type == RentalUnitType.EntireProperty())
        {
            var listBedroom = new List<Bedroom>();
            foreach (var bedroom in context.Message.ListBedrooms)
            {
                var newBedroom = new Bedroom(context.Message.RentalUnitId, bedroom.SingleBedCount,
                    bedroom.DoubleBedCount,
                    bedroom.KingBedCount, bedroom.SofaBedCount);

                listBedroom.Add(newBedroom);
            }

            await bedroomRepo.AddRangeAsync(listBedroom);
        }
        await unitOfWork.SaveChangesAsync();

        await context.RespondAsync(new BedroomAdded
        {
            CorrelationId = context.Message.CorrelationId
        });
    }
}