using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
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
}
