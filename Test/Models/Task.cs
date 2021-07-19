using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Task
    {
        // ID задачи
        public int Id { get; set; }


        // Приоритет задачи
        [Required (ErrorMessage = "Выставите приоритет задаче")]        
        [Display(Name = "Приоритет задачи")]
        public int Task_Priority { get; set; }

        // Приоритет задачи
        [Required (ErrorMessage = "Введите наименование задачи")]
        [StringLength(40)]
        [Display(Name = "Наименование задачи")]
        public string Task_Name { get; set; }

        // Краткое описание задачи
        [Required(ErrorMessage = "Введите описание задачи")]
        [StringLength(200)]
        [Display(Name = "Описание задачи")]
        public string Task_Description { get; set; }

        // Дата начала задачи
        [Display(Name = "Начало выполнения задачи:")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Task_Beginning_Date { get; set; }

        // Дата окончания задачи
        [Display(Name = "Конец выполнения задачи:")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Task_Ending_Date { get; set; }
        //Список сотрудников для конкректной задачи
        public virtual ICollection<Employee> Employees { get; set; }

        public Task() { Employees = new List<Employee>(); }
        
        }
}
