using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            const int count = 1000000;
            Console.WriteLine($"Attempts: {count}");

            Console.WriteLine("Simple:");
            Console.WriteLine($"Dont change. Win chance: {SimpleGame.DontChange(count)}%");
            Console.WriteLine($"Change. Win chance: {SimpleGame.Change(count)}%");

            Console.WriteLine("OOP:");
            Console.WriteLine($"Dont change. Win chance: {GameRoulette.PlayMany(count, Game.Strategies.DontChange)}%");
            Console.WriteLine($"Change. Win chance: {GameRoulette.PlayMany(count, Game.Strategies.Change)}%");
        }
        
    }

    public static class Helper
    {
        public static Random random = new Random();

        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(random.Next(0, list.Count()));
        }
    }
}
