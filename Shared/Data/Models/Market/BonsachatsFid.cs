using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class BonsachatsFid
{
    public string RefBon { get; set; }

    public DateOnly? DateBon { get; set; }

    public string Heure { get; set; }

    public decimal? Montant { get; set; }

    public string CodeClt { get; set; }

    public string CodeUti { get; set; }

    public string Ticket { get; set; }

    public DateOnly? DateTkt { get; set; }

    public string CodeMag { get; set; }

    public string Annee { get; set; }

    public string Agent { get; set; }

    public int? Flag { get; set; }
}
