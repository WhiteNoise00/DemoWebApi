using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите наименование отдела")]
        [Display(Name = "Наименование отдела")]

        public string Department_Name { get; set; }
        [Required(ErrorMessage = "Введите адрес отдела")]
        [Display(Name = "Адрес отдела")]
        public string Department_Location { get; set; }

        public virtual ICollection<Position> Positions { get; set; }

        public Department() { Positions = new List<Position>(); }
    }
}
