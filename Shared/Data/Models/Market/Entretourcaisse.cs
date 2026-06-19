using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class Entretourcaisse
{
    public string RefRet { get; set; }

    public DateOnly? DateRet { get; set; }

    public decimal? MntHt { get; set; }

    public decimal? MntTtc { get; set; }

    public string Ticket1 { get; set; }

    public DateOnly? DateTkt1 { get; set; }

    public string Ticket2 { get; set; }

    public DateOnly? DateTkt2 { get; set; }

    public string Agent { get; set; }

    public string Vendeur { get; set; }

    public string Remarque { get; set; }

    public string CodeMag { get; set; }

    public string Annee { get; set; }
}
