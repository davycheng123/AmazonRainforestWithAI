using System;
using System.Linq;
using Mars.Interfaces.Environments;
using TreeModel.Model.Shared;
using System.Collections.Generic;

namespace TreeModel.Model.Animal;

public class Animal : IAnimal<AnimalLayer>
{
    private List<Position> _adultTree;

    public void Init(AnimalLayer layer)
    {
        AnimalLayer = layer;
        Age = new Random().Next(0, 365 * 20);
        Alive = true;
    }

    public void Tick()
    {
        if (!Alive) return;

        if (LifePoints < 1 || Age > (365 * 20))
        {
            Alive = false;
            Die();
            return;
        }

        _adultTree = AnimalLayer.TreeLayer.ExploreTrees(Position, 10)
            .FindAll(t => AnimalLayer.TreeLayer.GetState(t) == State.Adult);

        if (Energy > 55 && Age > 5 * 365)
        {
            var child = AnimalLayer.CreateAnimal(1, Position);
            child.Age = 0;
            Energy -= 50;
            LifePoints -= 20;
        }

        Move();
        if (Energy < 30)
        {
            Consume();
        }

        if (Energy <= 0)
        {
            Energy = 0;
            LifePoints -= 5;
        }

        Age++;
    }

    public void Move()
    {
        if (_adultTree.Count > 0 && Energy < 30)
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
        if (fruitLeft <= 0)
        {
            return;
        }

        // Needed Fruit for full health
        var fruitNeed = (100 - Energy) / 20;

        // Gather Fruit from a tree, lower the Fruits count
        Energy += (AnimalLayer.TreeLayer.GatherFruit(Position, fruitNeed)) * 20;
        _seed = AnimalLayer.TreeLayer.GetSpecie(Position);
    }

    private Specie _seed;

    public void Poop()
    {
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position, 10);
        AnimalLayer.TreeLayer.CreatSeeding((double) _seed, Position);
    }

    public void Die()
    {
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position, 500);
        AnimalLayer.RemoveAnimal(this);
    }

    public AnimalLayer AnimalLayer { get; set; }

    public bool Alive { get; set; }

    public Position Position { get; set; }

    public int PoopRate { get; set; }

    public int Movement { get; set; } = 5;

    public int LifePoints { get; set; } = 100;

    public int Energy { get; set; } = 100;

    public int Age { get; set; }

    // identifies the agent
    public Guid ID { get; set; }
}