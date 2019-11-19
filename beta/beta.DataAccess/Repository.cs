using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using beta.BusinessLogic;


using Microsoft.EntityFrameworkCore;

using System.Linq;



namespace beta.DataAccess
{
    public class Repository : IRepository
    {
        private readonly Entities.NumDBContext _dbContext;

        Mapper Mapper;

        public Repository(Entities.NumDBContext dbContext /*, reference a logger here _logger */ )
        {

            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            //we use this one here.
            this.Mapper = new Mapper();
        }

        public async Task<BusinessLogic.Number> GetNumberAsync(int id)
        {
            //get the number
            var num = await _dbContext.Number.FirstOrDefaultAsync(x => x.Id == id);
            Number numb = num;
            
            return this.Mapper.MapNumber(numb);
        }

        public async Task<BusinessLogic.Number> GetLastNumberAsync()
        {
            //get the last number on the database
            var num = await _dbContext.Number.LastAsync();
            Number numb = num;

            return this.Mapper.MapNumber(numb);
        }



        public async Task PlaceNumberAsync(BusinessLogic.Number num)
        {
            DataAccess.Number numb = new DataAccess.Number() { IntNum = num.IntNumber };
            await _dbContext.AddAsync(numb);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateNumberAsync(BusinessLogic.Number num)
        {
            Number updated = await _dbContext.Number.FirstOrDefaultAsync(x => num.Id == x.Id);

            updated.IntNum = num.IntNumber;
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveNumberAtIndexAsync(BusinessLogic.Number num)
        {

            //get the entity to be removed
            Number deleted = await _dbContext.Number.FirstOrDefaultAsync(x => num.Id == x.Id);

            //flag it for deletion
            _dbContext.Number.Remove(deleted);

            //do the deletion
            await _dbContext.SaveChangesAsync();
        }



        public BusinessLogic.Number AddTwoNums(Number num1, Number num2)
        {
            return  new BusinessLogic.Number ( num1.IntNum + num2.IntNum ) ;
        }
    }
}
