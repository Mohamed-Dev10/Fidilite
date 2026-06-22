namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class DashboardStatsModel
    {
        public int TotalCartes { get; set; }
        public int CartesM3alem { get; set; }
        public int CartesArtisan { get; set; }
        public int CartesCeMois { get; set; }
        public int CartesActives { get; set; }
        public int CartesBloquees { get; set; }
        public List<MagasinStat> ParMagasin { get; set; } = new();

        // Contexte d'affichage : admin = vue globale (tous magasins) ;
        // utilisateur magasin = chiffres filtrés sur son magasin.
        public bool EstGlobal { get; set; } = true;
        public string? MagasinNom { get; set; }
        public bool SansMagasin { get; set; }
    }

    public class MagasinStat
    {
        public string Magasin { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
