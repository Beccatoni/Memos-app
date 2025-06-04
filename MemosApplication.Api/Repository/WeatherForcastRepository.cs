
public interface IWeatherForecastRepository
{
    IEnumerable<WeatherForecast> GetForecasts();
}

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly AppDbContext _context;

    public WeatherForecastRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // private static readonly string[] Summaries = new[]
    // {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };

    public IEnumerable<WeatherForecast> GetForecasts()
    {
        return _context.WeatherForecasts.ToList();

    }
}