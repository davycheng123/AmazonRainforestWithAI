using System;
using System.Linq;
using Mars.Common.Core.Random;
using Mars.Interfaces.Environments;
using TreeModel.Model.Shared;
using TreeModel.Model.Tree;
using System.Collections.Generic;

namespace TreeModel.Model.Animal;

public class Animal : IAnimal<AnimalLayer>
{
    // Set Taget for Animals
    private Position _goal;

    private List<Position> TreeNearby;

    public void Init(AnimalLayer layer)
    {
        AnimalLayer = layer;
    }

    public void Tick()
    {
        //TODO
        TreeNearby = AnimalLayer.TreeLayer.ExploreTrees(this.Position,10);
        //Console.Write(ListPosition.Count);
        
        
        // TODO: 
        Move();
        if (this.Energy > 10)
        {
            Consume();
        }

        if (this.LifePoints == 0)
        {
            Die();
        }

        if (Energy <= 0)
        {
            Energy = 0;
            LifePoints -= 5;
        }
    }

    public void Move()
    {
        //TODO: Improve Moving
        var AdultsTree = TreeNearby.FindAll(t => AnimalLayer.TreeLayer.GetState(t) == State.Adult);
        
        if (AdultsTree.Count > 0 && Energy < 30)
        {
            AnimalLayer.Environment.MoveTo(this, AdultsTree.First(), movement);
        }
        else{
            var rnd = new Random();
            var x = AnimalLayer.Environment.DimensionX;
            x = rnd.Next(x);
            var y = AnimalLayer.Environment.DimensionY;
            y = rnd.Next(y);
            AnimalLayer.Environment.MoveTo(this, new Position(x, y), movement);
        }
        // Lower the Energy when Moving
        Energy --;
    }

    public void Consume()
    {
        //Console.WriteLine("Animal Eat");
        var stateTree = AnimalLayer.TreeLayer.GetState(this.Position);
        // Ask if the tree enough Fruits
        var fruitLeft = AnimalLayer.TreeLayer.FruitLeft(Position);
        if (fruitLeft <= 0 )
        {
            return;
        }
        
        // Needed Fruit for full health
        var fruitNeed = (int)((100 - Energy)/10);

        // Gather Fruit from a tree, lower the Fruits count
        Energy = (AnimalLayer.TreeLayer.GatherFruit(Position, fruitNeed)) * 10;
        
    }

    public void Poop()
    {
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position,10);
        

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

    public int Energy { get; set; } = 10;


    // identifies the agent
    public Guid ID { get; set; }
}