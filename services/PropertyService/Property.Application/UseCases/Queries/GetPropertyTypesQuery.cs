using System.Windows.Input;
using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using Property.Domain.Aggregates.AggregateRoot;
using Property.Infrastructure.Repositories;

namespace Property.Application.CQRS.Queries;

public sealed record GetPropertyTypesQuery(
    bool isRoomBased): 
    ICommand<List<PropertyType>>;

public class GetPropertyTypesQueryHandler(
    IPropertyTypeRepository propertyTypeRepository)
    : ICommandHandler<GetPropertyTypesQuery, List<PropertyType>>
{
    public async Task<Result<List<PropertyType>>> Handle(GetPropertyTypesQuery query,
        CancellationToken cancellationToken)
    {
        switch (query.isRoomBased)
        {
            case true:
            {
                var list = await propertyTypeRepository.GetRoomBasedPropertyTypes();
                return Result<List<PropertyType>>.Success(list);
            }
            case false:
            {
                var list = await propertyTypeRepository.GetEntirePropertyTypes();
                return Result<List<PropertyType>>.Success(list);
            }
        }
    }
}