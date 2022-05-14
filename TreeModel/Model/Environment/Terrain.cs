using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Environment;

public class Terrain : ITerrainLayer
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

    public double GetWaterLevel(Position position)
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public double GetSoilNutrients(Position position)
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public double RemoveWater(Position position, double amount)
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public double RemoveSoilNutrients(Position position, double amount)
    {
        //TODO
        throw new System.NotImplementedException();
    }
}