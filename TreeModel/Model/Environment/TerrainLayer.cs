using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using TreeModel.Model.Spot;

namespace TreeModel.Model.Environment;


public class TerrainLayer: RasterLayer,ITerrainLayer
{
    /// <summary>
    ///     Responsible to create new agents and initialize them with required dependencies
    /// </summary>
    public IAgentManager AgentManager { get; private set; }

    /// <summary>
    ///     List from the Nutrient
    /// </summary>

    public List<Nutrient> _Nutrients;
    
    /// <summary>
    ///     List from the Water
    /// </summary>

    public List<Water> _Water;

    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
        UnregisterAgent unregisterAgentHandle)
    {
        var init =  base.InitLayer(layerInitData, registerAgentHandle, unregisterAgentHandle);
        
        AgentManager = layerInitData.Container.Resolve<IAgentManager>();
        _Nutrients= AgentManager.Spawn<Nutrient, TerrainLayer>().ToList();
        _Water = AgentManager.Spawn<Water, TerrainLayer>().ToList();
        
        return init;
    }
    

    public double GetWaterLevel(Position water)
    {
        var foundWater = _Water.Find(t => t.Position.Equals(water));
        if (foundWater !=null)
        {
            return foundWater.WaterValue;
        }
        return 0;
    }

    public double GetSoilNutrients(Position nutrien)
    {
        var foundNutrient = _Nutrients.Find(t => t.Position.Equals(nutrien));
        if (foundNutrient !=null)
        {
            return foundNutrient.NutrientValue;
        }

        return 0;
    }

    public double RemoveWater(Position water, int amount)
    {
        var foundWater = _Water.Find(t => t.Position.Equals(water));
        if (foundWater !=null)
        {
            foundWater.WaterValue -= amount;
            if (foundWater.WaterValue > 0)
            {
                return amount;
            }
            var amountOfWaterRemoved = amount - Math.Abs(foundWater.WaterValue);
            foundWater.WaterValue = 0;
            return amountOfWaterRemoved;
        }
        return 0;
    }

    public double RemoveSoilNutrients(Position nutrien, int amount)
    {
        var foundNutrient = _Nutrients.Find(t => t.Position.Equals(nutrien));
        if (foundNutrient !=null)
        {
            foundNutrient.NutrientValue -= amount;
            if (foundNutrient.NutrientValue > 0)
            {
                return amount;
            }
            var amountOfWaterRemoved = amount - Math.Abs(foundNutrient.NutrientValue);
            foundNutrient.NutrientValue = 0;
            return amountOfWaterRemoved;
        }
        return 0;
    }
}