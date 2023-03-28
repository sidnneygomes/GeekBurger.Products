using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace ServiceBusTopic
{
    public class ProductChangedService : IProductChangedService
    {
        public void AddToMessageList
           (IEnumerable<EntityEntry<Product>> changes)
        {
            _messages.AddRange(changes
                .Where(entity =>
                  entity.State != EntityState.Detached
                  && entity.State != EntityState.Unchanged)
                .Select(entity => GetMessage(entity)));
        }

        private Message GetMessage(EntityEntry<Product> entity)
        {
            var productChanged =
                Mapper.Map<ProductChanged>(entity);
            var productChangedSerialized =
                JsonConvert.SerializeObject(productChanged);
            var productChangedByteArray =
               Encoding.UTF8.GetBytes(productChangedSerialized);

            return new Message
            {
                Body = productChangedByteArray,
                MessageId = Guid.NewGuid().ToString(),
                Label = productChanged.Product.StoreId.ToString()
            };
        }
    }

    public interface IProductChangedService
    {
    }
}
