using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Environment;

public class Weather : IWeatherLayer
{
    public bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle = null,
        UnregisterAgent unregisterAgent = null)
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public long GetCurrentTick()
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public void SetCurrentTick(long currentStep)
    {
        //TODO
        throw new System.NotImplementedException();
    }
    public void WeatherForecast()
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public double GetWaterLevel(Position position)
    {
        //TODO
        throw new System.NotImplementedException();
    }
    
}