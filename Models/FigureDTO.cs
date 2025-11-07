namespace GeometryApi.Models;

public class FigureCreateDto
{
    public required string Type { get; set; }

    public CoordinatesDto? LeftUpper { get; set; }
    public CoordinatesDto? RightBottom { get; set; }
    public CoordinatesDto? Center { get; set; }
    public CoordinatesDto? LeftBottom { get; set; }
    public CoordinatesDto? Upper { get; set; }
    public double? Diameter { get; set; }
}

public class CoordinatesDto
{
    public required double X { get; set; }
    public required double Y { get; set; }
}
