using System;
using Mars.Interfaces.Agents;

namespace TreeModel.Model.Tree;

public class TreeType : IEntity
{
    public string Name { get; set; }
    public int AmountToSpawn { get; set; }
    public int MaxAge { get; set; }
    public int MatureAge { get; set; }
    public double ConsumptionRate { get; set;}
    public double GrowRate { get; set; }
    
    public double Fruits { get; set; }
    
    public double Woods { get; set; }
    public double ProductionRate { get; set; }
    public Guid ID { get; set; }
}