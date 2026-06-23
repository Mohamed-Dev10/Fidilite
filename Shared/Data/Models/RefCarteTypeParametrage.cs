namespace BRICOMA.ECOMMERCE.Data.Models
{
    public partial class RefCarteTypeParametrage
    {
        public int Id { get; set; }
        public int RefCarteTypeId { get; set; }

        /// <summary>
        /// Message de réception (confirmation) envoyé au client après création de la carte.
        /// Texte simple. Si null/vide → message par défaut.
        /// </summary>
        public string MessageReception { get; set; }

        /// <summary>
        /// Chemin web (sous wwwroot) de l'image-modèle de la carte, ex: "media/types/4.png".
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Position horizontale du code-barres sur l'image, en pourcentage (0-100).
        /// </summary>
        public int BarcodeX { get; set; }

        /// <summary>
        /// Position verticale du code-barres sur l'image, en pourcentage (0-100).
        /// </summary>
        public int BarcodeY { get; set; }

        /// <summary>
        /// Taille du code-barres en pourcentage de sa taille de base (50-200, défaut 100).
        /// </summary>
        public int BarcodeScale { get; set; } = 100;

        public virtual RefCarteType RefCarteType { get; set; }
    }
}
