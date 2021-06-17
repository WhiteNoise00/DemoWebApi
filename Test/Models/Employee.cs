using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Employee
    {
        // ID сотрудника
        public int Id { get; set; }
        // Личное имя
        public string Employee_Name { get; set; }
        // Фамилия
        public string Employee_Last_Name { get; set; }
        //Отчества в английском нет, возьмем как среднее имя
        public string Employee_Middle_Name { get; set; }
        //Электронная почта сотрудника
        public string Employee_Email { get; set; }

        //Отдел
        /*  public string Employee_Department { get; set; }
          // Занимаемая должность
          public string Employee_Position { get; set; }
          */

       /* public int PositionId { get; set; }
        public Position Position { get; set; }*/

        public virtual ICollection<Task> Tasks { get; set; }

        public Employee() { Tasks = new List<Task>(); }

    }
}
