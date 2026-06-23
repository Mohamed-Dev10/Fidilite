namespace BRICOMA.ECOMMERCE.Models.Helpers
{
    /// <summary>
    /// Paramétrage du modèle de carte transmis à la génération d'image :
    /// image de fond + position du code-barres en pourcentage. Quand il est null
    /// (aucun paramétrage), le service retombe sur le modèle codé en dur du type.
    /// </summary>
    public class CardTemplateParametrage
    {
        /// <summary>Chemin web (sous wwwroot) de l'image-modèle, ex: "media/types/4.png".</summary>
        public string ImagePath { get; set; }

        /// <summary>Position horizontale du code-barres en pourcentage (0-100).</summary>
        public int BarcodeXPercent { get; set; }

        /// <summary>Position verticale du code-barres en pourcentage (0-100).</summary>
        public int BarcodeYPercent { get; set; }
    }
}
