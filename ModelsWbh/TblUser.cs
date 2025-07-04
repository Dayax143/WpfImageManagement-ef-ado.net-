using System;
using System.Collections.Generic;

namespace WpfEFProfile.ModelsWbh;

public partial class TblUser
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? PassWord { get; set; }

    public string? Usertype { get; set; }

    public string? RecoveryQuestion { get; set; }

    public string? RecoveryAnswer { get; set; }

    public string? UserStatus { get; set; }
}
