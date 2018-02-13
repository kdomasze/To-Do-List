using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Recursively finds the parent for each <c>entry</c>. If no parent is found in the specified <c>entryItem</c>, a nullified version of <c>EntryItem</c> will be returned.
        /// </summary>
        /// <param name="entry">The child <c>Entry</c>.</param>
        /// <param name="entryItem">The <c>EntryItem</c> to start the search from.</param>
        /// <returns>The EntryItem that represents the parent of entry or a new <c>EntryItem(null)</c>.</returns>
        public static EntryItem FindParentForEntry(Entry entry, EntryItem entryItem)
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
