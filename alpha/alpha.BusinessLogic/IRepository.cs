using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alpha.BusinessLogic
{
    public interface IRepository
    {
        public Task<BusinessLogic.Number> GetNumberAsync(int Id);

        public Task PlaceNumberAsync(Number num);

        public Task UpdateNumberAsync(BusinessLogic.Number num);

        public Task RemoveNumberAtIndexAsync(BusinessLogic.Number num);
    }
}
