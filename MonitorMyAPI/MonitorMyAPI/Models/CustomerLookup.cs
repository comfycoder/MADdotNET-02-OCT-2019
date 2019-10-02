using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MonitorMyAPI.Models
{
    public class CustomerLookup
    {
        [Key]
        public int CustomerId { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}
