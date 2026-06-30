namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class DashboardStatsModel
    {
        public int TotalCartes { get; set; }
        public int CartesCeMois { get; set; }
        public int CartesActives { get; set; }
        public int CartesBloquees { get; set; }
        public int CartesAujourdhui { get; set; }
        public int CartesCetteSemaine { get; set; }
        public int CartesSemainePrecedente { get; set; }
        public double TauxActivation { get; set; }
        public List<MagasinStat> ParMagasin { get; set; } = new();

        // Un KPI par type de carte géré (M3alem, Artisan, et tout type paramétrable ajouté).
        // Généré dynamiquement : un nouveau type apparaît automatiquement dans le dashboard.
        public List<CarteTypeStat> ParType { get; set; } = new();

        // Tendance des créations : une entrée par jour sur les 30 derniers jours (séries complètes, jours vides à 0).
        public List<DailyStat> Tendance { get; set; } = new();

        // Contexte d'affichage : admin = vue globale (tous magasins) ;
        // utilisateur magasin = chiffres filtrés sur son magasin.
        public bool EstGlobal { get; set; } = true;
        public string? MagasinNom { get; set; }
        public bool SansMagasin { get; set; }
    }

    public class MagasinStat
    {
        public string Magasin { get; set; } = string.Empty;

        // Comparaison année en cours vs année précédente (demande responsable : pas par ville,
        // par magasin, et limité à 2 ans pour ne pas surcharger le graphe). "Année en cours"
        // = du 1er janvier à aujourd'hui ; "Année précédente" = même période l'an dernier
        // (comparaison équitable jour-pour-jour, pas année complète vs année partielle).
        public int CountCurrentYear { get; set; }
        public int CountPreviousYear { get; set; }

        // Répartition par type de carte pour chaque année (barres empilées colorées par type).
        public List<MagasinTypeStat> ParType { get; set; } = new();
    }

    public class MagasinTypeStat
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public int CountCurrentYear { get; set; }
        public int CountPreviousYear { get; set; }
    }

    public class CarteTypeStat
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class DailyStat
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
