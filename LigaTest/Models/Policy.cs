using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LigaTest.Models;

[Index("PolicyNumber", Name = "UQ__Policies__46DA0157E4F6A1EF", IsUnique = true)]
public partial class Policy
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string PolicyNumber { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MaxCompensation { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PremiumAmount { get; set; }

    [InverseProperty("Policy")]
    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();

    [ForeignKey("UserId")]
    [InverseProperty("Policies")]
    public virtual User User { get; set; } = null!;
}
