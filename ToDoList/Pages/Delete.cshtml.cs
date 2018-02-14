using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Pages.Entries
{
    public class DeleteModel : PageModel
    {
        private readonly EntryContext _context;

        public DeleteModel(EntryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Entry Entry { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Entry = await _context.Entry.SingleOrDefaultAsync(m => m.ID == id);

            if (Entry == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            await DeleteChildrenEntriesAsync(id);

            return RedirectToPage("./Index");
        }

        private async Task DeleteChildrenEntriesAsync(int? parentID)
        {
            var entryList = await _context.Entry.ToListAsync();

            IList<EntryItem> Entries = Entry.GetEntryItemList(entryList);

            foreach (var entry in Entries)
            {
                await DeleteEntriesAsync(parentID, entry);
            }
        }

        private async Task DeleteEntriesAsync(int? parentID, EntryItem entryItem)
        {
            if (entryItem.Entry.Parent != parentID && entryItem.Entry.ID != parentID) return;

            // delete entry
            Entry = await _context.Entry.FindAsync(entryItem.Entry.ID);

            if (Entry != null)
            {
                _context.Entry.Remove(Entry);
                await _context.SaveChangesAsync();
            }

            foreach (EntryItem children in entryItem.Children)
            {
                await DeleteEntriesAsync(entryItem.Entry.ID, children);
            }
        }
    }
}
