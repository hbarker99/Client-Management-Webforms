using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Management_Database.DTOs
{
    public class SearchResult<T>
    {
        public List<T> Results { get; set; }
        public int Count { get; set; }
    }
}
