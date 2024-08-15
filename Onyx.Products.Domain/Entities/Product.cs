using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Onyx.Products.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ModelId { get; set; }
    public virtual ProductModel Model { get; set; } 
    public int PriceId { get; set; }
    public virtual ProductPrice Price { get; set; } 
    public int ColourId { get; set; }
    public virtual ProductColour Colour { get; set; } 
}
