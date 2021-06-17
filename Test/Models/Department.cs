using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Department
    {
        public int Id { get; set; }

        public int Department_Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public Department() { Employees = new List<Employee>(); }
    }
}
