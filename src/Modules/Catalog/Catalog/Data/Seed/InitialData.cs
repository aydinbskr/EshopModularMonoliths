using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Data.Seed
{
    public static class InitialData
    {
        public static IEnumerable<Product> Products =>
            new List<Product>
            {
                Product.Create(new Guid("be4a13e8-3612-433e-a379-397e1be9ea82"),"IPhone",["Category1"],"Long Description","image file", 500),
                Product.Create(new Guid("14c14476-67e8-48e9-81a0-de432a92e1fe"),"Samsung",["Category1"],"Long Description","image file", 400),
                Product.Create(new Guid("c4b8c1e5-64de-4ca2-9ae2-f1036e720e9b"),"Huawei",["Category1"],"Long Description","image file", 300),
                Product.Create(new Guid("817b35f5-3230-4018-bd84-de63055723be"),"General Mobile",["Category1"],"Long Description","image file", 600),
            };
    }
}
