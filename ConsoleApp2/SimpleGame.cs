using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
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
}
