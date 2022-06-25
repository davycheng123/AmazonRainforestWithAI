using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Interfaces.Environments;
using ServiceStack;
using TreeModel.Model.Environment;

namespace TreeModel.Model.Human;

public class Human: IHuman<ForestLayer>, IPositionable
{
    //private Position _goal;
    
    public ForestLayer ForestLayer { get; set; }
    
    public Position Position { get; set; }
    
    public int DaysToCut { get; set; }
    
    public int initDay { get; set; }

    public double Movement { get; set; }
    
    public double PlantingRate { get; set; }
    
    public double HarvestRate { get; set; }
    
    private List<Position> _adultTree;



    // identifies the agent
    public Guid ID { get; set; }

    public void Init(ForestLayer layer)
    {
        ForestLayer = layer;
    }

    public void Tick()
    {
        _adultTree = ForestLayer.TreeEnvironment.Explore(Position, 10).ToList().Map(t => t.Position);
        
        if ((ForestLayer.GetCurrentTick()+initDay) % DaysToCut == 0)
        {
            KillTree();
        }
        
        // Spread the Tree
        FruitHarvest();
        PlantingTree();
        
        Move();
    }

    public void Move()
    {
        if (_adultTree.Count > 0)
        {
            ForestLayer.HumanEnvironment.MoveTo(this, _adultTree.First(),Movement);
        }
        else
        {
            var rnd = new Random();
            var x = ForestLayer.AnimalEnvironment.DimensionX;
            x = rnd.Next(x);
            var y = ForestLayer.AnimalEnvironment.DimensionY;
            y = rnd.Next(y);
            ForestLayer.HumanEnvironment.MoveTo(this, new Position(x, y), Movement);
        }

    }


    public void FruitHarvest()
    {
        Random rnd = new Random();
        var value = rnd.NextDouble();

        if (value < HarvestRate)
        {
            // Ask if the tree enough Fruits
            var fruitLeft = ForestLayer.FruitLeft(Position);
            if (fruitLeft > 0)
            {
                ForestLayer.GatherFruit(Position, 5);
            }
        }
    }

    public void PlantingTree()
    {
        Random rnd = new Random();
        var value = rnd.NextDouble();
        
        if (value < PlantingRate)
        {
            var tree = ForestLayer.ExploreTrees(Position,1);
            if (!tree.IsEmpty()) ForestLayer.Spread( ForestLayer.GetTree(tree.First()),Position);
        }
    }

    public void KillTree()
    {
        var tree = ForestLayer.ExploreTrees(Position,10);
        if (!tree.IsEmpty())
            ForestLayer.HurtTree(tree.First(), ForestLayer.GetCurrentTick() < 52 ? 65 : 95);
            // to prevent the lumberjack cutting all the trees at the beginning
    }

}
