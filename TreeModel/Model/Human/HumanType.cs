using System;
using Mars.Numerics.Ranges;

namespace TreeModel.Model.Human;
using Mars.Interfaces.Agents;


public class HumanType: IEntity
{
    public string Name { get; set; }
    
    public bool Alive { get; set; }
    
    public double Movement { get; set; }
    
    public double WoodStorage { get; set; }
    
    public double WoodConsumption { get; set;}
    
    public double PlantingRate { get; set; } // Rate 
    
    public double HarvestRate { get; set; }

    public int AmountToSpawn { get; set; }
    
    public double Damage { get; set; }


    public Guid ID { get; set; }

}