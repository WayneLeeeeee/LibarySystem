using System;
using System.Collections.Generic;

namespace libarySystem.Models;

public partial class SPAN_TABLE
{
    public int IDENTITY_FILED { get; set; }

    public string? SPAN_YEAR { get; set; }

    public string SPAN_START { get; set; } = null!;

    public string SPAN_END { get; set; } = null!;

    public string? NOTE { get; set; }

    public DateTime? CRE_DATE { get; set; }

    public string? CRE_USR { get; set; }

    public DateTime? MOD_DATE { get; set; }

    public string? MOD_USR { get; set; }
}
