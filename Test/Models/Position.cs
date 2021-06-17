using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Position
    {
        public int Id { get; set; }
       
        public int Position_Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public Position() { Employees = new List<Employee>(); }
    }
}
