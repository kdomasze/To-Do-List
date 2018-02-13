using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Entry
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        [Display(Name = "Completed")]
        public bool Completed { get; set; }
        public int Parent { get; set; }
    }
}
