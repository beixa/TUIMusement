using System.Collections.Generic;

namespace TUIMusement.Weather
{
    public record WeatherForecast(Forecast Forecast);
    public record Forecast(IEnumerable<ForecastDay> ForecastDay);
    public record ForecastDay(Day Day);
    public record Day(Condition Condition);
    public record Condition(string Text);
}
