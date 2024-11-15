namespace libarySystem.ViewModels
{
    public class QuerryLendRecord
    {
        public int IDENTITY_FILED { get; set; }
        public int BOOK_ID { get; set; }

        public string KEEPER_ID { get; set; } = null!;
        public string USER_ID { get; set; } = null!;

        public DateTime LEND_DATE { get; set; }

        public string? USER_CNAME { get; set; }

        public string? USER_ENAME { get; set; }
    }
}
