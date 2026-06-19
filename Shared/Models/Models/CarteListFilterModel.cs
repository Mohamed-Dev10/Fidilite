namespace BRICOMA.ECOMMERCE.Models.Models
{
    public class CarteListFilterModel
    {
        public int CarteTypeId { get; set; }
        public string? Search { get; set; }
        public int? StatutId { get; set; }
        public int? MagasinId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
