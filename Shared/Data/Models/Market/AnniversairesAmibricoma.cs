using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class AnniversairesAmibricoma
{
    public string Magasin { get; set; }

    public string CodeClt { get; set; }

    public string Client { get; set; }

    public DateOnly? Datecreat { get; set; }

    public string Tel { get; set; }

    public string Telmob { get; set; }

    public string Email { get; set; }

    public string Metier { get; set; }

    public DateOnly? Datenaiss { get; set; }

    public string JourAnniv { get; set; }

    public string MoisAnniv { get; set; }

    public int? Annee { get; set; }

    public string TypeClient { get; set; }

    public decimal? CaNetHt { get; set; }

    public decimal? CaNetTtc { get; set; }

    public string Carte { get; set; }
}
