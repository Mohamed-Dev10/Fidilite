using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class Lesticketscaiss
{
    public string Ticket { get; set; }

    public DateOnly? DateTkt { get; set; }

    public string Heure { get; set; }

    public string CodeOpr { get; set; }

    public string CodeRes { get; set; }

    public string Modepaie1 { get; set; }

    public string Modepaie2 { get; set; }

    public decimal? MntHt { get; set; }

    public decimal? Taxe { get; set; }

    public decimal? MntTtc { get; set; }

    public decimal? Montant1 { get; set; }

    public decimal? Montant2 { get; set; }

    public decimal? Valremise { get; set; }

    public decimal? Points { get; set; }

    public string CodeClt { get; set; }

    public string CodeMag { get; set; }

    public string Annee { get; set; }

    public string CodeMaa { get; set; }

    public decimal? PointsM { get; set; }
}
