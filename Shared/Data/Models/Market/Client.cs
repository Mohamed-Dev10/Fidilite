using System;
using System.Collections.Generic;

namespace BRICOMA.ECOMMERCE.Data.Models.Market;

public partial class Client
{
    public string CodeMag { get; set; }

    public string CodeClt { get; set; }

    public string Client1 { get; set; }

    public DateOnly? Datecreat { get; set; }

    public DateOnly? Datemodif { get; set; }

    public string CodeCar { get; set; }

    public DateOnly? Cartevalid { get; set; }

    public string CodePri { get; set; }

    public string Compte { get; set; }

    public string Tel { get; set; }

    public string Telmob { get; set; }

    public string Adresse { get; set; }

    public string CodeVil { get; set; }

    public string Email { get; set; }

    public string Siteweb { get; set; }

    public decimal? Statut { get; set; }

    public decimal? Echeance { get; set; }

    public string Modepaie { get; set; }

    public decimal? Bloquer { get; set; }

    public decimal? Plafond { get; set; }

    public decimal? Interne { get; set; }

    public string CodeCat { get; set; }

    public string Idfiscal { get; set; }

    public string Rc { get; set; }

    public string Patente { get; set; }

    public decimal? Solvable { get; set; }

    public string CodeVen { get; set; }

    public string CodeMet { get; set; }

    public decimal? BloqEdba { get; set; }

    public DateOnly? Datenaiss { get; set; }

    public string Ville { get; set; }

    public bool? FlagFid { get; set; }

    public string Cin { get; set; }

    public decimal? Nia { get; set; }

    public bool? Actif { get; set; }
}
