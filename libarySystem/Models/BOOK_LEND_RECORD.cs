using System;
using System.Collections.Generic;

namespace libarySystem.Models;

public partial class BOOK_LEND_RECORD
{
    public int IDENTITY_FILED { get; set; }

    public int BOOK_ID { get; set; }

    public string KEEPER_ID { get; set; } = null!;

    public DateTime LEND_DATE { get; set; }

    public DateTime? CRE_DATE { get; set; }

    public string? CRE_USR { get; set; }

    public DateTime? MOD_DATE { get; set; }

    public string? MOD_USR { get; set; }
}
