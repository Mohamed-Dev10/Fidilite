using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class Magasin
{
    public string CodeMag { get; set; }

    public string Magasin1 { get; set; }

    public string StoreId { get; set; }

    public string Directeur { get; set; }

    public int? NbrSal { get; set; }

    public decimal? Rang { get; set; }

    public string CodeReg { get; set; }

    public DateOnly? Dateremdata { get; set; }

    public DateOnly? Datecalstock { get; set; }

    public int? IntCmd { get; set; }

    public DateOnly? DateInt { get; set; }

    public int? SQuic { get; set; }

    public int? SOut { get; set; }

    public int? SSani { get; set; }

    public int? SJard { get; set; }

    public int? SElec { get; set; }

    public int? SDrog { get; set; }

    public int? OrdrOuv { get; set; }

    public int? AnneeOuv { get; set; }
}
