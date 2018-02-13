using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using static ToDoList.Pages.Entries.IndexModel;

namespace ToDoList.Pages.Entries
{
    public class DoneModel : PageModel
    {
        private readonly EntryContext _context;

        public DoneModel(EntryContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            // marks the task and all children tasks as completed
            await MarkChildrenEntriesDoneAsync(Entry.ID);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryExists(Entry.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EntryExists(int id)
        {
            return _context.Entry.Any(e => e.ID == id);
        }

        /// <summary>
        /// Marks all entries who's ID or parent ID matches <c>parentID</c>, and all children of those entries as completed.
        /// </summary>
        /// <param name="parentID">The ID marked as completed</param>
        private async Task MarkChildrenEntriesDoneAsync(int parentID)
        {
            var entryList = await _context.Entry.ToListAsync();

            IList<EntryItem> Entries = Entry.GetEntryItemList(entryList);

            foreach (var entry in Entries)
            {
                MarkEntriesCompleted(parentID, entry);
            }
        }

        /// <summary>
        /// Recursively marks entries as completed if the parentID matches the entry's ID or Parent ID field
        /// </summary>
        /// <param name="parentID">The ID marked as completed</param>
        /// <param name="entryItem">An entry Item being marked for completion</param>
        private void MarkEntriesCompleted(int parentID, EntryItem entryItem)
        {
            if (entryItem.Entry.Parent != parentID && entryItem.Entry.ID != parentID) return;

            // marks entry as complete and needed to be updated
            entryItem.Entry.Completed = true;
            _context.Attach(entryItem.Entry).State = EntityState.Modified;

            foreach (EntryItem children in entryItem.Children)
            {
                MarkEntriesCompleted(entryItem.Entry.ID, children);
            }
        }
    }
}