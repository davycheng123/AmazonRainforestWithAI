using System;
using System.Linq;
using Mars.Interfaces.Data;
using Mars.Interfaces.Layers;
namespace TreeModel.Model.Environment;

public class WeatherLayer : IWeatherLayer, ISteppedActiveLayer
{
    private long _tick = 0;
    public TerrainLayer TerrainLayer { get; set; }
    public ForestLayer ForestLayer { get; set; }

    //chances
    public double RainfallChance { get; set; } = 0.7;
    
    public double StormChance { get; set; }
    public double FireChance { get; set; } = 0.001;

    // data
    public double Humidity { get; private set; }
    public double WindSpeed { get; private set; }
    public double Temperature { get; private set; }
    public double RainfallWater { get; private set; }

    // bools
    public bool Raining { get; private set; }
    public bool Storming { get; private set; }
    public bool Burning { get; private set; }

    public void WeatherForecast()
    {
        //TODO
        var rnd = new Random();
        Humidity = rnd.NextDouble() * 20 + 80;
        WindSpeed = rnd.NextDouble() * 150;
        Temperature = rnd.NextDouble() * 20 + 30;
        Burning = rnd.NextDouble() >= FireChance;
        Raining = rnd.NextDouble() >= RainfallChance;
        Storming = WindSpeed > 100;

        if (Raining)
        {
            Burning = false;
            RainfallWater = rnd.Next(10, 50);
            TerrainLayer.AddWaterEverywhere(RainfallWater);
        }

        if (Burning || Storming)
        {
            //TODO set trees on fire or have the trees check if burning is true and set them self on fire per chance
            foreach (var tree in ForestLayer.TreeEnvironment.Entities)
            {
                //if (rnd.NextDouble() > 0.8) ForestLayer.HurtTree(tree.Position, rnd.Next(0, 100));
            }
        }
    }

    public bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle = null,
        UnregisterAgent unregisterAgent = null)
    {
        Console.WriteLine("weather ok");
        return true;
    }

    public long GetCurrentTick()
    {
        return _tick;
    }

    public void SetCurrentTick(long currentStep)
    {
        _tick = currentStep;
    }

    public void Tick()
    {
        //Do nothing
    }

    public void PreTick()
    {
        WeatherForecast();
    }

    public void PostTick()
    { 
        const int daysPerPrint = 30;
        if(GetCurrentTick() % daysPerPrint != 0) return;
        
        var trees= ForestLayer.TreeEnvironment.Entities.Count();
        var animals = ForestLayer.AnimalEnvironment.Entities.Count();
        var humans = ForestLayer.HumanEnvironment.Entities.Count();
        Console.WriteLine(GetCurrentTick()/daysPerPrint +" month(s) passed, animals: "+animals+" trees: "+trees +" humans: " +humans);
    }
}