using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace book_tracker.Models
{
    public class BooksViewModel
    {
        public int BookID { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public int NumberOfPages { get; set; }
        public Uri ImageURL { get; set; }
        public Uri BookDepoURL { get; set; }

        public List<SelectListItem> Authors { get; set; }
        public int AuthorID {get;set;}
        public String Review { get; set; }
    }
}
