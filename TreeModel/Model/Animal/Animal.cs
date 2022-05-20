using System;
using System.Linq;
using Mars.Common.Core.Random;
using Mars.Interfaces.Environments;
using TreeModel.Model.Shared;
using TreeModel.Model.Tree;

namespace TreeModel.Model.Animal;

public class Animal : IAnimal<AnimalLayer>
{
    // Set Taget for Animals
    private Position _goal;

    public void Init(AnimalLayer layer)
    {
        AnimalLayer = layer;
    }

    public void Tick()
    {
        //TODO
        var ListPosition = AnimalLayer.TreeLayer.ExploreTrees(this.Position,10);
        //Console.Write(ListPosition.Count);
        
        
        // TODO: 
        Move();
        if (this.TimetoEat > 10)
        {
            Consume();
        }

        if (this.LifePoints == 0)
        {
            Die();
        }
    }

    public void Move()
    {
        //Console.WriteLine("Animal Move");
        Position pos = this.Position;
        var Xpos = pos.X+ new Random().Next(-1,1);
        var Ypos = pos.Y+ new Random().Next(-1,1);
        this.Position = Position.CreatePosition(Xpos, Ypos);

        this.TimetoEat += 1;
        if (this.TimetoEat > 10)
        {
            this.LifePoints -= 5;
        }
    }

    public void Consume()
    {
        //Console.WriteLine("Animal Eat");
        var stateTree = AnimalLayer.TreeLayer.GetState(this.Position);
        // Ask if the tree old enough for Fruits
        if (stateTree== State.Nothing || stateTree== State.Seedling )
        {
            //TODO: We can say if it doesnt have any thing to eat then reduce the life point
            this.LifePoints -= 10;
            this.TimetoEat += 1;
        }
        else
        {
            this.LifePoints += 10;
            // Reset TimetoEat
            this.TimetoEat = 10;
            
            //reduce Fruit from tree 
            var tree = AnimalLayer.TreeLayer.getTree(this.Position, 10);
            tree.fruit -= 10;
            tree.LifePoints -= 10;
        }
    }

    public void Poop()
    {
        //TODO: ADD Benefit the Nutrient from the TerrainLayer.cs
        
        //TODO: ADD Seeding in the Position
        
    }

    public void Die()
    {
        
        //TODO: ADD Benefit the Nutrient from the TerrainLayer.cs
        AnimalLayer.Environment.Remove(this);
    }
    
    public AnimalLayer AnimalLayer { get; set; }
    
    public bool alive { get; set; }
    
    public Position Position { get; set; }
        
    public int PoopRate { get; set; }
    
    public int movement { get; set; }

    public int LifePoints { get; set; } = 100;

    public int TimetoEat { get; set; } = 10;


    // identifies the agent
    public Guid ID { get; set; }
}