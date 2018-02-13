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
            var Entry = await _context.Entry.ToListAsync();

            Entries = new List<EntryItem>();

            foreach (var entry in Entry)
            {
                if (entry.Parent == 0)
                {
                    Entries.Add(new EntryItem(entry));
                }
                else
                {
                    foreach (var entryItem in Entries)
                    {
                        var entryList = FindParentForEntry(entry, entryItem);
                        if (entryList.Entry == null) continue;

                        entryList.Children.Add(new EntryItem(entry));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Recursively finds the parent for each <c>entry</c>. If no parent is found in the specified <c>entryItem</c>, a nullified version of <c>EntryItem</c> will be returned.
        /// </summary>
        /// <param name="entry">The child <c>Entry</c>.</param>
        /// <param name="entryItem">The <c>EntryItem</c> to start the search from.</param>
        /// <returns>The EntryItem that represents the parent of entry or a new <c>EntryItem(null)</c>.</returns>
        public EntryItem FindParentForEntry(Entry entry, EntryItem entryItem)
        {
            EntryItem output = new EntryItem(null);

            if (entryItem.Entry.ID != entry.Parent)
            {
                foreach (EntryItem children in entryItem.Children)
                {
                    output = FindParentForEntry(entry, children);
                }
            }
            else
            {
                output = entryItem;
            }

            return output;
        }

        public struct EntryItem
        {
            public Entry Entry;
            public IList<EntryItem> Children;

            public EntryItem(Entry entry)
            {
                Entry = entry;
                Children = new List<EntryItem>();
            }
        }
    }
}
