using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Interfaces.Environments;
using ServiceStack;
using TreeModel.Model.Environment;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Human;

public class Human: IHuman<ForestLayer>, IPositionable
{
    //private Position _goal;
    
    public ForestLayer ForestLayer { get; set; }
    
    public bool Alive { get; set; }
    
    public Position Position { get; set; }
    
    
    public double WoodConsumption { get; set; }

    public double Movement { get; set; }
    
    public double WoodStorage { get; set; }
    
    public double PlantingRate { get; set; }
    
    public double HarvestRate { get; set; }
    
    public double Damage { get; set; }


    // identifies the agent
    public Guid ID { get; set; }

    public void Init(ForestLayer layer)
    {
        ForestLayer = layer;
    }

    public void Tick()
    {
        var listPosition = ForestLayer.ExploreTrees(this.Position,10);
        
        Move();
        if(WoodStorage < 10) CutDownTree(listPosition);
        
        // Spread the Tree
        Random rnd = new Random();
        if(rnd.Next(100) % HarvestRate  == 0)  FruitHarvest(listPosition);
        if (rnd.Next(100) % PlantingRate == 0) PlantingTree(listPosition);
        
        
    }

    public void Move()
    {
        // Only Move when the WoodStorage low 
        if (WoodStorage < 10)
        {
            var foundTree = ForestLayer.TreeEnvironment.Explore(Position, 10).ToList().Map(t => t.Position);
            if (foundTree.Count > 0)
            {
                ForestLayer.HumanEnvironment.MoveTo(this, foundTree.First(), Movement);
            }
            else
            {
                ForestLayer.HumanEnvironment.MoveTo(this, ForestLayer.NewRandomeLocation(), Movement);
            }
        }else
        {
            WoodStorage -= 1 * WoodConsumption; 
        }

    }

    public void CutDownTree(List<Position> positions)
    {
        foreach (var treePosition in positions)
        {
            Tree.Tree treeAtLoc = ForestLayer.GetTree(treePosition);
            if(treeAtLoc.State == State.Adult){
                if (treeAtLoc.Wood != 0)
                {
                    ForestLayer.GatherWood(treePosition, WoodConsumption);
                    ForestLayer.HurtTree(treePosition,Damage);
                }
            }
         
           
        }
    }

    public void FruitHarvest(List<Position> positions)
    {
        foreach (var treePosition in positions)
        {
            Tree.Tree treeAtLoc = ForestLayer.GetTree(treePosition);
            if (treeAtLoc.State == State.Adult)
            {
                if (treeAtLoc.Fruit != 0)
                {
                    ForestLayer.GatherFruit(treePosition, HarvestRate);
                    ForestLayer.HurtTree(treePosition, Damage - 5);
                }
            }
        }
    }

    public void PlantingTree(List<Position> positions)
    {
        var tree = ForestLayer.GetTree(positions.First()); 
        ForestLayer.Spread(tree,Position);
    }
}
