using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ToDoList.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TaskContext(serviceProvider.GetRequiredService<DbContextOptions<TaskContext>>()))
            {
                if(context.Task.Any())
                {
                    return;
                }

                context.Task.AddRange(
                    new Task
                    {
                        Title = "Task 1",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 2",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-4"),
                        DueDate = DateTime.Parse("2018-2-18"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 3",
                        Parent = 1,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 4",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 5",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-6"),
                        DueDate = DateTime.Parse("2018-2-16"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 6",
                        Parent = 5,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 7",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 8",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    },
                    new Task
                    {
                        Title = "Task 9",
                        Parent = 8,
                        CreationDate = DateTime.Parse("2018-2-7"),
                        DueDate = DateTime.Parse("2018-2-12"),
                        Completed = false
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
