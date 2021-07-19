using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Employee
    {
        // ID сотрудника
        public int Id { get; set; }

        // Личное имя
        [Required(ErrorMessage = "Введите имя сотрудника")]
        [StringLength(50)]
        [Display(Name = "Имя сотрудника:")]
        public string Employee_Name { get; set; }


        // Фамилия
        [Required(ErrorMessage = "Введите фамилию сотрудника")]
        [StringLength(100)]

        [Display(Name = "Фамилия сотрудника:")]
        public string Employee_Last_Name { get; set; }

        // Отчество
        [Required(ErrorMessage = "Введите отчество сотрудника")]
        [StringLength(100)]
        [Display(Name = "Отчество сотрудника:")]
        public string Employee_Middle_Name { get; set; }


        // Электронная почта сотрудника
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [Display(Name = "Адрес электронной почты:")]
        [StringLength(100)]
        public string Employee_Email { get; set; }


        // Телефонный номер сотрудника
        [Required(ErrorMessage = "Введите телефонный номер")]
        [Display(Name = "Телефонный номер для обратной связи:")]
        [StringLength(12)]
        public string Employee_Phone { get; set; } //а точно ли string?

        [Required(ErrorMessage = "Укажите должность")]
        [Display(Name = "Занимаемая должность:")]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        // Список прикрепелнных задач
        public virtual ICollection<Task> Tasks { get; set; }

        public Employee() { Tasks = new List<Task>(); }

    }
}
