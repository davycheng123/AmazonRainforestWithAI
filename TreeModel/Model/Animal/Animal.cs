using System;
using System.Linq;
using Mars.Interfaces.Environments;
using TreeModel.Model.Shared;
using System.Collections.Generic;
using Mars.Common.Core.Random;
using TreeModel.Model.Environment;

namespace TreeModel.Model.Animal;

public class Animal : IAnimal<ForestLayer>
{
    private List<Position> _adultTree;

    public void Init(ForestLayer layer)
    {
        //AnimalLayer = layer;
        //mAge = new Random().Next(0, 365 * 20);
        Alive = true;
    }

    public void Tick()
    {
        /*
        if (!Alive) return;

        // Die when too Old or out of Lifepoint
        if (LifePoints < 1 || Age > (365 * 20))
        {
            Alive = false;
            Die();
            return;
        }
        
        // Find Tree in The Radios
        _adultTree = AnimalLayer.TreeLayer.ExploreTrees(Position, 10)
            .FindAll(t => AnimalLayer.TreeLayer.GetState(t) == State.Adult);
        
        // Spawning Child when enough Energy and Old enough
        
        if (Energy > 140 && Age > 15 * 365)
        {
            var child = AnimalLayer.CreateAnimal(2, Position);
            Energy -= 20;
            LifePoints -= 20;
        }
        
        
        Move();
        
        // Eat if Energy too low
        if (Energy < 80)
        {
            Consume();
        }

        // Energy too low affect life point
        if (Energy <= 0)
        {
            Energy = 0;
            LifePoints -= 5;
        }

        Age++;
        */
    }
/*
    public void Move()
    {
        //if (_adultTree.Count > 0 && Energy < 30)
        if (_adultTree.Count > 0)    
        {
            AnimalLayer.Environment.MoveTo(this, _adultTree.First(), Movement);
        }
        else
        {
            var rnd = new Random();
            var x = AnimalLayer.Environment.DimensionX;
            x = rnd.Next(x);
            var y = AnimalLayer.Environment.DimensionY;
            y = rnd.Next(y);
            AnimalLayer.Environment.MoveTo(this, new Position(x, y), Movement);
        }

        // Lower the Energy when Moving
        Energy--;
    }

    public void Consume()
    {
        // Ask if the tree enough Fruits
        var fruitLeft = AnimalLayer.TreeLayer.FruitLeft(Position);

        if (fruitLeft > 0)
        {
            // Needed Fruit for full health
            var fruitNeed = (100 - Energy) / 20;
        
            // Gather Fruit from a tree, lower the Fruits count
            Energy += (AnimalLayer.TreeLayer.GatherFruit(Position, fruitNeed)) * 20 + 10;
            LifePoints += 10;
                
            cnt ++;
            // POop Spread Tree
            _seed = AnimalLayer.TreeLayer.GetSpecie(Position);
            Poop(_seed);
            
        }
        
    }
    private Specie _seed;
    private int cnt=0;
    
    public void Poop(Specie seed)
    {
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position, 10);
        var rnd = new Random();
        if (rnd.Next(100*PoopRate) % 69 == 0)
        {
            var x = RandomHelper.Random.Next(AnimalLayer.TreeLayer.Width);
            var y = RandomHelper.Random.Next(AnimalLayer.TreeLayer.Height);
            
            switch (seed)
            { 
                case Specie.NutmegTree:
                    //Console.Write("**Seed are 1**");
                    AnimalLayer.TreeLayer.CreatSeeding(1, Position.CreatePosition(Position.X + x, Position.Y + y));
                    break;
                case Specie.PalmTree:
                    //Console.Write("**Seed are 2**");
                    AnimalLayer.TreeLayer.CreatSeeding(2, Position.CreatePosition(Position.X + x, Position.Y + y));
                    break;
                case Specie.BrazilNutTree:
                    //Console.Write("**Seed are 3**");
                    AnimalLayer.TreeLayer.CreatSeeding(3, Position.CreatePosition(Position.X + x, Position.Y + y));
                    break;

            }
            
        }

        _seed = Specie.NotATree;


    }

    public void Die()
    {
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position, 500);
        AnimalLayer.RemoveAnimal(this);
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position,100);

    }

    public AnimalLayer AnimalLayer { get; set; }
*/
    public bool Alive { get; set; }

    public Position Position { get; set; }

    public int PoopRate { get; set; }

    public int Movement { get; set; } 

    public int LifePoints { get; set; } 

    public int Energy { get; set; } 

    public string Name { get; set; }
    public int AmountToSpawn { get; set; }
    public int MaxAge { get; set; }
    public int MatureAge { get; set; }
    public double MovementSpeed { get; set; }
    public double ConsumptionRate { get; set;}
    public bool Herbivore { get; set; }
    public bool Carnivore { get; set; }
    public int Age { get; set; }

    // identifies the agent
    public Guid ID { get; set; }
    public void Move()
    {
        throw new NotImplementedException();
    }

    public void Consume()
    {
        throw new NotImplementedException();
    }

    public void Poop(Specie seed)
    {
        throw new NotImplementedException();
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
}