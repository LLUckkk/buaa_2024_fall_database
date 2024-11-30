using System.ComponentModel.DataAnnotations;
using CampusMarketApi.Core.Interfaces;

namespace CampusMarketApi.Core.Models.Entities;

public class User : IAggregateRoot
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public required string Username { get; set; }

    [Required]
    [StringLength(255)]
    public required string Password { get; set; }

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(64)]
    public required string Nickname { get; set; }

    public string? Bio { get; set; }

    [StringLength(255)]
    public string? Avatar { get; set; }

    [Required]
    public bool IsAdmin { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    [StringLength(64)]
    public string? StudentId { get; set; }

    [Required]
    public bool IsVerified { get; set; } = false;

    [Required]
    public float Rating { get; set; } = 0;

    [Required]
    public int TotalTransactions { get; set; } = 0;

    [Required]
    public int FollowersCount { get; set; } = 0;

    [Required]
    public int FollowingCount { get; set; } = 0;
}