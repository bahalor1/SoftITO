using System;
using System.Collections.Generic;

namespace SinemaUygulaması.Models;

public partial class Seanslar
{
    public int Id { get; set; }

    public int? FilmId { get; set; }

    public string SalonAdi { get; set; } = null!;

    public DateTime BaslangicSaati { get; set; }

    public virtual ICollection<Biletler> Biletlers { get; set; } = new List<Biletler>();

    public virtual Filmler? Film { get; set; }
}
