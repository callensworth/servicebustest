using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using beta.BusinessLogic;
using beta.DataAccess;

namespace beta.Controllers
{

    //defaults to the get
    [Route("")]
    [ApiController]
    public class NumberController : ControllerBase
    {

        //stores the database reference
        private readonly IRepository _repository;

        //injection defined in constructor
        public NumberController(IRepository repository)
        {
            //here
            _repository = repository;
        }

        // GET: api/Number //works
        [HttpGet]
        public async Task<ActionResult<BusinessLogic.Number>> Get()
        {
            try
            {
                //just return the first number
                BusinessLogic.Number num = await _repository.GetNumberAsync(1);


                return Ok(num);

            }
            catch (Exception e)
            {
                string danger = "Danger Will Robinson: Server Meltdown Eminant: " + e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, danger);
            }
        }

        
        // GET: /number/<index in table> //works
        [HttpGet("get/{id}", Name = "getnumber")]
        public async Task<ActionResult<BusinessLogic.Number>> GetNum(int id)
        {
            try
            {
                BusinessLogic.Number num = await _repository.GetNumberAsync(id);
                return num;
            }
            catch (Exception e)
            {
                string danger = $"Warning: Gremlins in server, number at {id} missing:" + e.Message;
                return StatusCode( StatusCodes.Status500InternalServerError, danger );
            }
        }
    
        // POST: api/Number / create a new number //works
        [ HttpPost("new/{num}") ]
        public async Task< ActionResult<object> > Post(int num)
        {
            try 
            { 
                //put it in the db.
                await _repository.PlaceNumberAsync( new BusinessLogic.Number(num) );
                return Ok();
            }
            
            catch(Exception e)
            {
                string danger = "The database was unable to post for... reasons. : " + e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, danger);
            }
        }

        // PUT: update/<index>/<value>
        [HttpPut("update/{id}/{num}")]
        public async Task<ActionResult<object> > Put( int id, int num ) 
        {
            try
            { 
                BusinessLogic.Number number = new BusinessLogic.Number(num);
                number.Id = id;
                await _repository.UpdateNumberAsync(number);
                return Ok();
            }
            catch(Exception e)
            {
                string danger = "The number is on vacation in Palm Springs and is Not Found: " + e.Message;
                return NotFound(danger);
            }
            
        }

        // DELETE: delete/<index>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<DataAccess.Number>> Delete(int id)
        {
            try
            {
                //does the number exist?
                BusinessLogic.Number num = await _repository.GetNumberAsync(id);

                //if the answer is "Nope!"
                if (num == null)
                    return NotFound();

                //otherwise give it the boot
                await _repository.RemoveNumberAtIndexAsync(num);

                //report a sucessful booting
                return NoContent();
            }
            catch(Exception e)
            {
                string danger = "While the server isn't on fire... for now... your number wasn't found: "+ e.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, danger);
            }
        }
    }
}
