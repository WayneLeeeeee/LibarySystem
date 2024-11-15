using libarySystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace libarySystem.ViewModels
{
    public class LibaryQuerryData
    {
        public int BOOK_ID { get; set; }

        public string BOOK_NAME { get; set; } = null!;

        public string BOOK_CLASS_NAME { get; set; } = null!;

        public DateTime? BOOK_BOUGHT_DATE { get; set; }

        public string? USER_CNAME { get; set; }

        public string? BOOK_STATUS_NAME { get; set; }
        

        //public List<BOOK_CLASS> BOOK_CLASSs { get; set; }

        //public List<BOOK_DATA> BOOK_DATAs { get; set; }

        //public List<BOOK_LEND_RECORD> BOOK_LEND_RECORDs { get; set; }

        //public List<MEMBER_M> MEMBER_Ms { get; set; }


    }
    //public SearchModel searchModel { get; set; }
    //public class SearchModel
    //{
    //    public SelectListItem selectedBookClass { get; set; }
    //    public List<SelectListItem> bookClassSelectList { get; set; }
    //    public string bookName { get; set; }

}
