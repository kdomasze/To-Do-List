using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EntryContext(serviceProvider.GetRequiredService<DbContextOptions<EntryContext>>()))
            {
                if(context.Entry.Any())
                {
                    return;
                }

                context.Entry.AddRange(
                    new Entry
                    {
                        Title = "Entry 1",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = false
                    },
                    new Entry
                    {
                        Title = "Entry 2",
                        Parent = 0,
                        CreationDate = DateTime.Parse("2018-2-4"),
                        DueDate = DateTime.Parse("2018-2-18"),
                        Completed = false
                    },
                    new Entry
                    {
                        Title = "Entry 3",
                        Parent = 1,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = true
                    },
                    new Entry
                    {
                        Title = "Entry 4",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-9"),
                        DueDate = DateTime.Parse("2018-2-15"),
                        Completed = true
                    },
                    new Entry
                    {
                        Title = "Entry 5",
                        Parent = 2,
                        CreationDate = DateTime.Parse("2018-2-6"),
                        DueDate = DateTime.Parse("2018-2-16"),
                        Completed = false
                    },
                    new Entry
                    {
                        Title = "Entry 6",
                        Parent = 5,
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
