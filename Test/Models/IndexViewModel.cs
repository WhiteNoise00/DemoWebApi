using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Position> Positions { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
