using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Interfaces.Environments;
/*
namespace TreeModel.Model.Human;

public class Human: IHuman<HumanLayer>, IPositionable
{
    //private Position _goal;

    public void Init(HumanLayer layer)
    {
        HumanLayer = layer;
    }

    public void Tick()
    {
        //TODO
        var listPosition = HumanLayer.TreeLayer.ExploreTrees(this.Position,10);
        
        Move();
        // if()
        CutDownTree(listPosition);
        Poop();
        FruitHarvest(listPosition);
        PlantingTree(listPosition);
    }

    public void Move()
    {        
        //Console.WriteLine("Human Move");
        Position pos = this.Position;
        var Xpos = pos.X+ new Random().Next(-1,1);
        var Ypos = pos.Y+ new Random().Next(-1,1);
        this.Position = Position.CreatePosition(Xpos, Ypos);

    }

    public void CutDownTree(List<Position> positions)
    {
        foreach (var tree in positions)
        {
            WoodStorage -= HumanLayer.TreeLayer.GatherWood(tree, WoodStorage);
            //TODO
            // HumanLayer.TreeLayer.HurtTree(tree,);
        }
    }

    public void Poop()
    {
        //TODO: ADD Benefit the Nutrient from the TerrainLayer.cs
        
        //TODO: ADD Seeding in the Position
        
    }

    public void FruitHarvest(List<Position> positions)
    {
        foreach (var treePosition in positions)
        {
            HumanLayer.TreeLayer.GatherFruit(treePosition, 10);
            // reduce Fruit from tree 
            var tree = HumanLayer.TreeLayer.ExploreTrees(this.Position, 10);
            HumanLayer.TreeLayer.GatherFruit(tree.First(),10);
            HumanLayer.TreeLayer.HurtTree(tree.First(),10);
 

        }
    }

    public void PlantingTree(List<Position> positions)
    {
        foreach (var treePosition in positions)
        {
            // randomly select a tree type
            Random treeType = new Random();
            HumanLayer.TreeLayer.CreateTree(treeType.Next(1,4), treePosition);
        }

    }
    
    public HumanLayer HumanLayer { get; set; }
    
    public bool alive { get; set; }
    
    public Position Position { get; set; }
    
    public Position StartPosition { get; set; }
    
    public int PoopRate { get; set; }
    
    public int WoodConsumption { get; set; }

    public int Movement { get; set; }
    
    public int WoodStorage { get; set; }


    // identifies the agent
    public Guid ID { get; set; }
}
*/