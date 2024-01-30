using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Other
{
    public class Paged<T>
    {
        public int Page { get; set; }

        public int Pages { get; set; }

        public long TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public List<T> Items { get; set; }
    }
}
