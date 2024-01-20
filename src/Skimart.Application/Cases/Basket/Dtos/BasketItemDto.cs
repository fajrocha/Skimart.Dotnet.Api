using System.ComponentModel.DataAnnotations;

namespace Skimart.Application.Cases.Basket.Dtos;

public record BasketItemDto(
    [Required]
    int Id, 
    [Required]
    string ProductName, 
    [Required]
    [Range(.1, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    decimal Price, 
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater or equal to 1.")]
    int Quantity,
    [Required]
    string PictureUrl,
    [Required]
    string Brand,
    [Required]
    string Type);