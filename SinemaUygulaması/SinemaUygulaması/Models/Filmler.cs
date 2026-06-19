using System;
using System.Collections.Generic;

namespace SinemaUygulaması.Models;

public partial class Filmler
{
    public int Id { get; set; }

    public string FilmAdi { get; set; } = null!;

    public string Tur { get; set; } = null!;

    public int SureDakika { get; set; }

    public virtual ICollection<Seanslar> Seanslars { get; set; } = new List<Seanslar>();
}
