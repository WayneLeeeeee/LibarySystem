using System;
using System.Collections.Generic;

namespace libarySystem.Models;

public partial class BOOK_CLASS
{
    public string BOOK_CLASS_ID { get; set; } = null!;

    public string BOOK_CLASS_NAME { get; set; } = null!;

    public DateTime? CREATE_DATE { get; set; }

    public string? CREATE_USER { get; set; }

    public DateTime? MODIFY_DATE { get; set; }

    public string? MODIFY_USER { get; set; }

    internal object EndsWith(string v)
    {
        throw new NotImplementedException();
    }
}
