using System;

namespace TreeModel.Model.Human;
using Mars.Interfaces.Agents;


public class HumanType: IEntity
{
    public string Name { get; private set; }
    
    public double Movement { get; set; }
    
    public int DaysToCut { get; set;}
    
    public double PlantingRate { get; set; } // Rate 
    
    public double HarvestRate { get; set; }

    public int AmountToSpawn { get; set; }
    

    public Guid ID { get; set; }

}