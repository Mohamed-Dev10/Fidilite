namespace BRICOMA.ECOMMERCE.Data.Models
{
    public class AuditLog
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string? EntityCode { get; set; }
        public string? Detail { get; set; }
        public DateTime DateOperation { get; set; } = DateTime.Now;
    }
}
