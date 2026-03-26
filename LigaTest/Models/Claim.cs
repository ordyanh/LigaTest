using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LigaTest.Models;

public partial class Claim
{
    [Key]
    public int Id { get; set; }

    public int PolicyId { get; set; }

    public int UserId { get; set; }

    public DateTime IncidentDate { get; set; }

    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal RequestedAmount { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [ForeignKey("PolicyId")]
    [InverseProperty("Claims")]
    public virtual Policy Policy { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Claims")]
    public virtual User User { get; set; } = null!;
}
