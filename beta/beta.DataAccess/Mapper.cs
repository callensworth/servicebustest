using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using beta.BusinessLogic;

namespace beta.DataAccess
{
    ///<summary>
    /// Map one value to and from data-access to business-logic 
    ///</summary>
    public class Mapper
    {
        public BusinessLogic.Number MapNumber(DataAccess.Number number)
        {
            //get the value and id from the database
            BusinessLogic.Number num =  new BusinessLogic.Number(number.IntNum);
            num.Id = number.Id;

            //return it
            return num;
        }

        public DataAccess.Number MapNumber(BusinessLogic.Number number)
        {
            //just place the value into a Number entity
            DataAccess.Number num = new DataAccess.Number();

            //return the entity
            return num;
        }

    }
}
