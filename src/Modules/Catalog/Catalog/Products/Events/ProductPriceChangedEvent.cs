
namespace Catalog.Products.Events
{
    public record ProductPriceChengedEvent(Product Product) : IDomainEvent;
}
