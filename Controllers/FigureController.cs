using Microsoft.AspNetCore.Mvc;
using GeometryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GeometryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FiguresController : ControllerBase
{
    private readonly GeometryContext _context;
    private readonly IMemoryCache _memoryCache;
    private const string SQUARE_SUM_CACHE_KEY = "square-sum";

    public FiguresController(GeometryContext context, IMemoryCache cache)
    {
        _context = context;
        _memoryCache = cache;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFigure([FromBody] FigureCreateDto dto)
    {
        try
        {
            Figure figure = dto.Type switch // ToLower()?
            {
                "rectangle" => new Rectangle
                {
                    LeftUpper = dto.LeftUpper != null
                        ? new Coordinates { X = dto.LeftUpper.X, Y = dto.LeftUpper.Y }
                        : throw new ArgumentException("leftUpper обязателен для типа rectangle"),
                    RightBottom = dto.RightBottom != null
                        ? new Coordinates { X = dto.RightBottom.X, Y = dto.RightBottom.Y }
                        : throw new ArgumentException("rightBottom обязателен для типа rectangle"),
                },

                "circle" => new Circle
                {
                    Center = dto.Center != null
                        ? new Coordinates { X = dto.Center.X, Y = dto.Center.Y }
                        : throw new ArgumentException("center обязателен для типа circle"),
                    Diameter = dto.Diameter ?? throw new ArgumentException("diameter обязателен для типа Circle")
                },

                "triangle" => new Triangle
                {
                    LeftBottom = dto.LeftBottom != null
                        ? new Coordinates { X = dto.LeftBottom.X, Y = dto.LeftBottom.Y }
                        : throw new ArgumentException("leftBottom обязателен для типа Triangle"),
                    RightBottom = dto.RightBottom != null
                        ? new Coordinates { X = dto.RightBottom.X, Y = dto.RightBottom.Y }
                        : throw new ArgumentException("rightBottom is required for Triangle"),
                    Upper = dto.Upper != null
                        ? new Coordinates { X = dto.Upper.X, Y = dto.Upper.Y }
                        : throw new ArgumentException("upper is required for Triangle"),
                },

                _ => throw new ArgumentException($"неизвестный тип {dto.Type}")
            };

            figure.Square = figure.CalculateSquare();
            if (_memoryCache.TryGetValue(SQUARE_SUM_CACHE_KEY, out double sumSquares))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
                _memoryCache.Set(SQUARE_SUM_CACHE_KEY, sumSquares + figure.Square, cacheEntryOptions);
            }

            _context.Add(figure);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFigure), new { id = figure.Id }, figure);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { error = e.Message });
        }
    }

    [HttpGet("sum-squares")]
    public async Task<IActionResult> GetSumSquares()
    {   
        if (!_memoryCache.TryGetValue(SQUARE_SUM_CACHE_KEY, out double sumSquares))
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
            sumSquares = await _context.Figures.SumAsync(f => f.Square);
            _memoryCache.Set(SQUARE_SUM_CACHE_KEY, sumSquares, cacheEntryOptions);
            
        }
        return Ok(sumSquares);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetFigure(long id)
    {
        var figure = await _context.Set<Figure>().FindAsync(id);
        if (figure == null) return NotFound();
        return Ok(figure);
    }
}
