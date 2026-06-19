using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class ItemsEcommerce
{
    public string CodeArt { get; set; }

    public string Article { get; set; }

    public string Rayon { get; set; }

    public decimal? PvTtc { get; set; }

    public DateOnly? Debut { get; set; }

    public DateOnly? Fin { get; set; }

    public decimal? Prixpromo { get; set; }

    public DateOnly? Datemodif { get; set; }

    public DateOnly? Datecreat { get; set; }

    public int? AchatBloq { get; set; }
}
