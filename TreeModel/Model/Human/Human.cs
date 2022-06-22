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
    
    public Position Position { get; set; }
    
    
    public double WoodConsumption { get; set; }     // constant: how many wood is consumed

    public double Movement { get; set; }
    
    public double WoodStorage { get; set; }         // variable: current no. of wood
    
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

        if (listPosition.Any())
        {
             CutDownTree(listPosition);
            
            // Spread the Tree
            Random rnd = new Random();
            //if (rnd.NextDouble() < HarvestRate )  FruitHarvest(listPosition);
            if (rnd.NextDouble() < PlantingRate) PlantingTree(listPosition);
        }
        
        Move();
    }

    private void Move()
    {
        // Only Move when the WoodStorage low 
        if (WoodStorage < 10 || WoodStorage < WoodConsumption)
        {
        var foundTree = ForestLayer.TreeEnvironment.Explore(Position, 5).ToList().Map(t => t.Position);
        if (foundTree.Count > 0)
        {
            ForestLayer.HumanEnvironment.MoveTo(this, foundTree.First(), Movement);
        }
        else
        {
            ForestLayer.HumanEnvironment.MoveTo(this, ForestLayer.NewRandomeLocation(), Movement);
            
        }
        } else
        {
            WoodStorage -= 1 * WoodConsumption; 
        }

    }


    private void CutDownTree(List<Position> positions)
    {
        foreach (var treePosition in from treePosition in positions let treeAtLoc = ForestLayer.GetTree(treePosition) where treeAtLoc != null where treeAtLoc.Wood != 0 select treePosition)
        {
            WoodStorage += ForestLayer.GatherWood(treePosition, WoodConsumption);
            ForestLayer.HurtTree(treePosition,Damage);
        }
    }

    private void FruitHarvest(List<Position> positions)
    {
        foreach (var treePosition in from treePosition in positions let treeAtLoc = ForestLayer.GetTree(treePosition) where treeAtLoc != null where treeAtLoc.State == State.Adult where treeAtLoc.Fruit != 0 select treePosition)
        {
            ForestLayer.GatherFruit(treePosition,10* HarvestRate);
            ForestLayer.HurtTree(treePosition, Damage - 5);
        }
    }

    private void PlantingTree(List<Position> positions)
    {
        var tree = ForestLayer.GetTree(positions.First()); 
        if(tree != null) ForestLayer.Spread(tree,Position);
    }

    private void killTree(List<Position> positions)
    {
        var tree = positions.FirstOrDefault();
        if (tree == null) return;
        ForestLayer.GetTree(tree).Die();
    }
}
