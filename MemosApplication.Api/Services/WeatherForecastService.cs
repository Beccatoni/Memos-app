

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetForecasts();
}

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastRepository _repository;

    public WeatherForecastService(IWeatherForecastRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<WeatherForecast> GetForecasts()
    {
        return _repository.GetForecasts();
    }
}