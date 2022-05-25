using System;
using Mars.Interfaces.Agents;

namespace TreeModel.Model.Animal;

public class AnimalType : IEntity
{
    public string Name { get; set; }
    public int AmountToSpawn { get; set; }
    public int MaxAge { get; set; }
    public int MatureAge { get; set; }
    public double MovementSpeed { get; set; }
    public double ConsumptionRate { get; set;}
    public bool Herbivore { get; set; }
    public bool Carnivore { get; set; }
    public Guid ID { get; set;}
    
}