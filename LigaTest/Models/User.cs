using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LigaTest.Models;

[Index("Email", Name = "UQ__Users__A9D1053481103558", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    [StringLength(200)]
    public string FullName { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();

    [InverseProperty("User")]
    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
}
