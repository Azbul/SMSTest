using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Part3.Data.Models;

/// <summary>
/// Сущность блюда в БД.
/// </summary>
[Table("MenuItems")]
public class MenuItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string ExternalId { get; set; } = string.Empty;

    [StringLength(250)]
    public string Article { get; set; } = string.Empty;

    [StringLength(250)]
    public string Name { get; set; } = string.Empty;

    public double Price { get; set; }
}