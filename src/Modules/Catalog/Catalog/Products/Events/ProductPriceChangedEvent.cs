
namespace Catalog.Products.Events
{
    public record ProductPriceChengedEvent(Product product) : IDomainEvent;
}
