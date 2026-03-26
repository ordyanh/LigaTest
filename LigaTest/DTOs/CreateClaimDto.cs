namespace LigaTest.DTOs
{
    public class CreateClaimDto
    {
        public int PolicyId { get; set; }
        public int UserId { get; set; } // В реальной системе возьмем из JWT токена
        public DateTime IncidentDate { get; set; }
        public string Description { get; set; } = null!;
        public decimal RequestedAmount { get; set; }
    }
}