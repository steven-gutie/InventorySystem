using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Models.Specs
{
    public class PaginatedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize) // Math.Ceiling returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number.
            };
            AddRange(items); // AddRange is a method that adds the elements of the specified collection to the end of the List<T>.
        }

        public static PaginatedList<T> ToPaginate(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); // Skip is a method that bypasses a specified number of elements in a sequence and then returns the remaining elements.
                                                                                          // Take is a method that returns a specified number of contiguous elements from the start of a sequence.
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
