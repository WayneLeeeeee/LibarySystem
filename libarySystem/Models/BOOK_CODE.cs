using System;
using System.Collections.Generic;

namespace libarySystem.Models;

public partial class BOOK_CODE
{
    public string CODE_TYPE { get; set; } = null!;

    public string CODE_ID { get; set; } = null!;

    public string? CODE_TYPE_DESC { get; set; }

    public string? CODE_NAME { get; set; }

    public DateTime? CREATE_DATE { get; set; }

    public string? CREATE_USER { get; set; }

    public DateTime? MODIFY_DATE { get; set; }

    public string? MODIFY_USER { get; set; }
}
