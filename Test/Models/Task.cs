using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Task
    {
        // ID задачи
        public int Id { get; set; }
        // Приоритет задачи
        public int Task_Priority { get; set; }
        // Приоритет задачи
        public string Task_Name { get; set; }
        // Отдел-заказчик
        public string Task_Customer_Department { get; set; }
        // Отдел-исполнитель
        public string Task_Executing_Department { get; set; }
        // Данные об исполнителях задачи
        public string Task_Executor_Data { get; set; }//это можно убрать
        // Дата начала задачи
        public DateTime Task_Beginning_Date { get; set; }
        // Дата окончания задачи
        public DateTime Task_Ending_Date { get; set; }
        //Список сотрудников для конкректной задачи
        public virtual ICollection<Employee> Employees { get; set; }

        public Task() { Employees = new List<Employee>(); } // Зачем мы создаем конструктор для данного класса? Реализация коллекции?

     
    }
}
