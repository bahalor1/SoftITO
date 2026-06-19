using System;
using System.Collections.Generic;

namespace SinemaUygulaması.Models;

public partial class Biletler
{
    public int Id { get; set; }

    public int? SeansId { get; set; }

    public string MusteriAdSoyad { get; set; } = null!;

    public string KoltukNo { get; set; } = null!;

    public virtual Seanslar? Seans { get; set; }
}
