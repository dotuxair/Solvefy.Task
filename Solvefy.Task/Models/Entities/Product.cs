using System.ComponentModel.DataAnnotations;

namespace Solvefy.Task.Models.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

   public string ProductName { get; set; } = string.Empty;

    [DataType(DataType.Currency)] public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}