using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class ViewModel
    {
        public IEnumerable<Employee> Emploees { get; set; }
        public IEnumerable<Task> Tasks { get; set; }

        //или же можно указать конкретные свойства из классов
    }
}
