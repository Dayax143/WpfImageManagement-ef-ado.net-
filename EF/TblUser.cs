using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace WpfEFProfile.EF;

public partial class TblUser
{
    [Key]
	public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? PassWord { get; set; }

    public string? Usertype { get; set; }

    public string? RecoveryQuestion { get; set; }

    public string? RecoveryAnswer { get; set; }

    public string? UserStatus { get; set; }

	public string? PasswordHash { get; set; }
}
