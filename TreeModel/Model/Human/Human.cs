using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Interfaces.Environments;
using TreeModel.Model.Environment;

namespace TreeModel.Model.Human;

public class Human: IHuman<ForestLayer>, IPositionable
{
    //private Position _goal;

    public void Init(ForestLayer layer)
    {
        ForestLayer = layer;
    }

    public void Tick()
    {
        //TODO
        //var listPosition = ForestLayer.ExploreTrees(this.Position,10);
        
        Move();
        // if()
        //CutDownTree(listPosition);
        Poop();
        //FruitHarvest(listPosition);
        //PlantingTree(listPosition);
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
            //WoodStorage -= ForestLayer.GatherWood(tree, WoodStorage);
            //TODO
            // HumanLayer.ForestLayer.HurtTree(tree,);
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
            //ForestLayer.GatherFruit(treePosition, 10);
            // reduce Fruit from tree 
            //var tree =ForestLayer.ExploreTrees(this.Position, 10);
            //ForestLayer.GatherFruit(tree.First(),10);
            //ForestLayer.HurtTree(tree.First(),10);
 

        }
    }

    public void PlantingTree(List<Position> positions)
    {
        foreach (var treePosition in positions)
        {
            // randomly select a tree type
            Random treeType = new Random();
            //ForestLayer.CreateTree(treeType.Next(1,4), treePosition);
        }

    }
    
    public ForestLayer ForestLayer { get; set; }
    
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
