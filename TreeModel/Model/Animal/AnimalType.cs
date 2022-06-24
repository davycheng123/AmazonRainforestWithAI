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
    
    public int DaysToReproduce { get; set; }
    public double LifePoints { get; set; }
    public double Energy { get; set; }
    
    
    public  double Poop2Tree { get; set; }
    
    public double ReproduceRate { get; set; }
    
    public Guid ID { get; set;}
    
}