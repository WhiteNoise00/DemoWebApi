using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Position
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите наименование должности")]
        [Display(Name = "Наименование должности")]
        public string Position_Name { get; set; }

        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]// попробуем так
        public int DepartmentId { get; set; }
        public Department Department{ get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public Position() { Employees = new List<Employee>(); }

    }
}
