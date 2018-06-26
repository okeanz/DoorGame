﻿using System;
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



    public static class GameRoulette
    {
        public static double PlayMany(int count, Game.Strategies strategy)
        {
            long good = 0;
            for (int i = 0; i < count; i++)
            {
                var game = new Game(strategy);
                if (game.Play())
                    good++;
            }
            return ((double)good / count) * 100;
        }
    }

    //Функциональная реализация алгоритма
    public static class SimpleGame
    {
        //Стратегия без смены двери
        public static double DontChange(long count)
        {
            long good = 0;
            int car = 0;
            int choosen = 0;
            for (long i = 0; i < count; i++)
            {
                car = Helper.random.Next(0, 3); //Выбор двери за которой будет машина
                choosen = Helper.random.Next(0, 3); //Выбор двери игроком
                if (car == choosen)
                    good++;
            }
            return ((double)good / count) * 100;
        }

        //Стратегия со сменой двери
        public static double Change(long count)
        {
            long good = 0;
            int car = 0;
            int choosen = 0;
            int host = 0;
            var range = Enumerable.Range(0, 3);
            for (long i = 0; i < count; i++)
            {
                car = Helper.random.Next(0, 3); //Выбор двери за которой будет машина
                choosen = Helper.random.Next(0, 3); //Выбор двери игроком
                host = range.Where(x => x != car && x != choosen).GetRandom(); // Выбор двери ведущим
                choosen = range.Where(x => x != host && x != choosen).GetRandom(); // Смена выбора игроком
                if (car == choosen)
                    good++;
            }
            return ((double)good / count) * 100;
        }
    }

    //Класс, описывающий одну партию
    public class Game
    {
        Door[] _doors;
        Strategies _strategy;

        public Game(Strategies strategy)
        {
            _strategy = strategy;
            _doors = Door.MakeArray();
            _doors.GetRandom().ContainsCar = true;//Ставим машину за случайную дверь
        }

        //Сыграть партию
        public bool Play()
        {
            Player p = new Player();
            p.ChooseDoor(_doors); //Выбор игрока в первом раунде

            Host h = new Host(p);
            h.ChooseDoor(_doors); //Выбор двери ведущего

            switch (_strategy)
            {
                case Strategies.Change:
                    p.ChoosenDoor = _doors.Where(x => x != h.ChoosenDoor && x != p.ChoosenDoor).GetRandom(); //Смена двери игроком
                    if (p.ChoosenDoor.ContainsCar)
                        return true;
                    break;
                case Strategies.DontChange:
                    if (p.ChoosenDoor.ContainsCar)
                        return true;
                    break;
                default:
                    throw new NotImplementedException(); //На случай расширения пула стратегий
            }

            return false;
        }
        public enum Strategies { Change, DontChange }
    }

    public class Door
    {
        public bool ContainsCar;

        public static Door[] MakeArray(int count = 3)
        {
            var doors = new Door[count];
            for (int i = 0; i < count; i++)
            {
                doors[i] = new Door();
            }
            return doors;
        }
    }

    //Общий функционал для игрока и ведущего
    public abstract class Pawn
    {
        public Door ChoosenDoor;
        public Pawn()
        {
            ChoosenDoor = null;
        }
        public abstract void ChooseDoor(Door[] doors);
    }



    public class Player : Pawn
    {
        public Player() : base()
        {
        }
        public override void ChooseDoor(Door[] doors)
        {
            ChoosenDoor = doors.GetRandom();
        }
    }

    public class Host : Pawn
    {
        Player _player;
        public Host(Player player) : base()
        {
            _player = player;
        }
        public override void ChooseDoor(Door[] doors)
        {
            ChoosenDoor = doors.Where(x => !x.ContainsCar && x != _player.ChoosenDoor).GetRandom();
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
