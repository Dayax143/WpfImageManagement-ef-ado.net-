using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfEFProfile.EF;

public partial class TblLora
{
    [Key]
    public int LoraId { get; set; }

    public string? CorSupply { get; set; }

    public string? LoraSerial { get; set; }

    public string? ReceiptRv { get; set; }

    public string? Status { get; set; }

    public string? Refference { get; set; }

    public DateTime? Date { get; set; }
}
