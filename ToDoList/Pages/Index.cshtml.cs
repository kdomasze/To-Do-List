using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Pages.Entries
{
    public class IndexModel : PageModel
    {
        private readonly EntryContext _context;

        public IndexModel(EntryContext context)
        {
            _context = context;
        }

        public IList<EntryItem> Entries { get; set; }

        public async Task OnGetAsync()
        {
            var entryList = await _context.Entry.ToListAsync();

            Entries = Entry.GetEntryItemList(entryList);
        }
    }
}
