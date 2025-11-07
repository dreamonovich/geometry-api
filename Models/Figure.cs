using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GeometryApi.Models;

[Owned]
public class Coordinates
{   
    public required double X { get; set; }
    public required double Y { get; set; }
}

public abstract class Figure
{
    [Key]
    public long Id { get; set; }
    public double Square { get; set; }

    public virtual string Type { get; set; } = null!;

    public abstract double CalculateSquare();
}

public class Rectangle : Figure
{
    public override string Type { get; set; } = "Rectangle";

    public required Coordinates LeftUpper { get; set; }
    public required Coordinates RightBottom { get; set; }

    public override double CalculateSquare()
    {
        double length = Math.Abs(LeftUpper.X - RightBottom.X);
        double height = Math.Abs(LeftUpper.Y - RightBottom.Y);
        return length * height;
    }
}

public class Circle : Figure
{
    public override string Type { get; set; } = "Circle";

    public required Coordinates Center { get; set; }
    public required double Diameter { get; set; } // TODO: Валидация Diameter > 0

    public override double CalculateSquare()
    {
        double r = Diameter / 2.0;
        return Math.PI * r * r;
    }
}

public class Triangle : Figure
{
    public override string Type { get; set; } = "Triangle";

    public required Coordinates LeftBottom { get; set; }
    public required Coordinates RightBottom { get; set; }
    public required Coordinates Upper { get; set; }


    public override double CalculateSquare()
    {
        return 0.5 * Math.Abs(
            LeftBottom.X * (RightBottom.Y - Upper.Y) +
            RightBottom.X * (Upper.Y - LeftBottom.Y) +
            Upper.X * (LeftBottom.Y - RightBottom.Y));
    }
}
