using System;
using System.Collections.Generic;

namespace libarySystem.Models;

public partial class MEMBER_M
{
    public string USER_ID { get; set; } = null!;

    public string? USER_CNAME { get; set; }

    public string? USER_ENAME { get; set; }

    public DateTime? CREATE_DATE { get; set; }

    public string? CREATE_USER { get; set; }

    public DateTime? MODIFY_DATE { get; set; }

    public string? MODIFY_USER { get; set; }
}
