using System;
using Mars.Interfaces.Environments;
using System.Collections.Generic;
using System.Linq;
using TreeModel.Model.Environment;
using ServiceStack;
namespace TreeModel.Model.Animal;

public class Animal : IAnimal<ForestLayer>
{
    private List<Position> _adultTree;
    private ForestLayer ForestLayer { get; set; }
    
    public TerrainLayer TerrainLayer { get; set; }
    
    private bool Alive { get; set; }

    public Position Position { get; set; }
    
    //100% LifePoints & Energy 
    public double LifePoints { get; set; }

    public double Energy { get; set; }

    // name = type, example: name = Monkey
    public string Name { get; set; }
    public int Age { get; set; }
    public int MaxAge { get; set; }
    public int MatureAge { get; set; }
    
    // rates
    public double MovementSpeed { get; set; }
    public double ConsumptionRate { get; set;}
    
    public double Poop2Tree { get; set; }
    
    public double ReproduceRate { get; set; }
    
    // herbivore = eats plants/fruits
    public bool Herbivore { get; set; }
    
    //carnivore = eats meat/other animals
    public bool Carnivore { get; set; }
    

    // identifies the agent
    public Guid ID { get; set; }
    public void Init(ForestLayer layer)
    {
        ForestLayer = layer;
        Alive = true;
    }

    public void Tick()

    {   
        // Every Tick the Animal will try to find tree 
        _adultTree = ForestLayer.TreeEnvironment.Explore(Position, 10).ToList().Map(t => t.Position);
        
        // check Alive
        if(!Alive) return;
        
        // Add Moving for the Animal
        Move();
        
        // Add Condition for animal to eat
        if(Energy < 50) Consume();
        
        // Chance to Poop
        Poop();
        
        // Age Increase
        Age++;


        Random rnd = new Random();
        
        // Reproduce
        const int daysToReproduce = 100;
        if(ForestLayer.GetCurrentTick() % daysToReproduce == 0 && ForestLayer.ExploreAnimals(Position,1).Count > 2 && rnd.NextDouble() < ReproduceRate ) 
            ForestLayer.Reproduce(this);


        // Lower the Life point if the Energy to low
        if (Energy < 1) LifePoints--;
        Energy -= 0.5 * ConsumptionRate;
        
        
        // Dying condition
        if (Age > MaxAge || LifePoints < 1) Die();
    }

    public void Move()
    {
        //The Animal will try to find tree to move to
        if (_adultTree.Count > 0)    
        {
            ForestLayer.AnimalEnvironment.MoveTo(this, _adultTree.First(), MovementSpeed);
        }
        else
        {
            var rnd = new Random();
            var x = ForestLayer.AnimalEnvironment.DimensionX;
            x = rnd.Next(x);
            var y = ForestLayer.AnimalEnvironment.DimensionY;
            y = rnd.Next(y);
            ForestLayer.AnimalEnvironment.MoveTo(this, new Position(x, y), MovementSpeed);
        }

        // Lower the Energy when Moving
        Energy--;
    }

    public void Consume()
    {
        
        if (Herbivore)
        {
            // Ask if the tree enough Fruits
            var fruitLeft = ForestLayer.FruitLeft(Position);
            if (fruitLeft > 0)
            {
                // Needed Fruit for full health
                var fruitNeed = (100 - Energy) / 20;
        
                // Gather Fruit from a tree, lower the Fruits count
                
                Energy += 10 * (ForestLayer.GatherFruit(Position, fruitNeed)) ;
                LifePoints += 1 * ConsumptionRate;

            }
        }

    }

    public void Poop()
    {

        ForestLayer.TerrainLayer.AddSoilNutrients(Position,10);
        Random rnd = new Random();
        var value = rnd.NextDouble();
        
        if (value < Poop2Tree)
        {
            var tree = ForestLayer.ExploreTrees(Position,1);
                    if (!tree.IsEmpty()) ForestLayer.Spread( ForestLayer.GetTree(tree.First()),Position);
        }
        
        ForestLayer.TerrainLayer.AddSoilNutrients(Position,10);

    }

    public void Die()
    {
        Alive = false;
        // Benefit the Nutrient and the Water 
        ForestLayer.TerrainLayer.AddSoilNutrients(Position,50);
        ForestLayer.AnimalEnvironment.Remove(this);
    }
    
    
}