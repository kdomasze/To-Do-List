using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Pages.Entries
{
    public class IndexModel : PageModel
    {
        private readonly ToDoList.Models.EntryContext _context;

        public IndexModel(ToDoList.Models.EntryContext context)
        {
            _context = context;
        }

        public IList<Entry> Entry { get;set; }

        public async Task OnGetAsync()
        {
            Entry = await _context.Entry.ToListAsync();
        }
    }
}
