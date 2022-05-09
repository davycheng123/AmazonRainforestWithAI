using Mars.Interfaces.Layers;

namespace TreeModel.Model.Environment
{
    public interface IWeatherLayer : IDataLayer
    {
        /// <summary>
        /// Calculates the weather and depending of what weather it is benefit or hurt trees/animals
        /// </summary>
        void WeatherForecast();
    }
}
