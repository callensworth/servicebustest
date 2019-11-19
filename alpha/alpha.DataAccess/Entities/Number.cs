using System;
using System.ComponentModel.DataAnnotations;

namespace alpha.DataAccess
{
    public class Number
    {   
        
        [Key]
        public int Id {get; set; }

        public int IntNum { get; set; }
    }
}
