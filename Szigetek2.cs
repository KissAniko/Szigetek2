using System;
using System.IO;

namespace TengerV3
{

    class Program
    {
        static void Main(string[] args)
        {
            const char TENGER_JEL = '*';
            const char SZIGET_JEL = 'O';

            char[,] tenger = new char[10, 20];
            Alaphelyzet(TENGER_JEL, tenger);

            

            bool futasVege = false;
            do
            {
                switch (ValasztMenubol())
                {
                    case 'g': //pálya generálása
                        Console.Write("\nHány sorból álljon ? :");
                        int sorSzam = Convert.ToInt32(Console.ReadLine());
                        Console.Write("\nHány oszlopból álljon ? :");
                        int oszlopSzam = Convert.ToInt32(Console.ReadLine());
                        tenger = General(sorSzam, oszlopSzam, TENGER_JEL);
                        break;

                    case 'u': //pálya ürítése
                        Alaphelyzet(TENGER_JEL, tenger);
                        break;

                    case 's':  //Szigetek elhelyezése
                        Console.Write("\nHány szigetet szeretne a tengerre? :");
                        int szigetSzam = Convert.ToInt32(Console.ReadLine());
                        SzigeteketRak(SZIGET_JEL, tenger, szigetSzam);
                        break;

                    case 'b': //pálya betöltése
                        Console.Write("\nKérem a tenger elérési útját és fájlnevét! :");
                        tenger = BetoltTerkepet(Console.ReadLine());
                        Console.WriteLine("Sikeres betöltés!");
                        break;

                    case 'm': //pálya mentése
                        Console.Write("\nKérem a tenger elérési útját és fájlnevét! :");
                        string terkepNev = Console.ReadLine();
                        MentTerkepet(tenger, terkepNev);
                        Console.WriteLine("Sikeres mentés!");
                        break;
                    case ' ':
                        futasVege = true;
                        break;
                }
                Megjelenit(tenger, true);
                InformaciokKiirasa(tenger, SZIGET_JEL);
            } while (!futasVege);

              
               //3. feladat
               //A megadott koordinátájú szigetnek van-e közvetlen szomszédja? /alul, felül, mellete/
               Console.Write($"Kérem a sor számát! [1..{tenger.GetLength(0)}] :");
               int adottSorSzama = Convert.ToInt32(Console.ReadLine());

               Console.Write($"Kérem az oszlop számát! [1..{tenger.GetLength(1)}] :");
               int adottOszlopSzama = Convert.ToInt32(Console.ReadLine());

               if (VanSzomszedja(tenger, SZIGET_JEL, adottSorSzama, adottOszlopSzama))
                   Console.WriteLine("Van szomszédja!");
               else
                   Console.WriteLine("Nincs szomszédja!");
           


        }

        static char[,] General(int sorSzam, int oszlopSzam, char tengerJel)
        {
            char[,] ujTerkep = new char[sorSzam, oszlopSzam];
            for (int sorIndex = 0; sorIndex < ujTerkep.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < ujTerkep.GetLength(1); oszlopIndexe++)
                {
                    ujTerkep[sorIndex, oszlopIndexe] = tengerJel;
                }
            }
            return ujTerkep;
        }

        static void InformaciokKiirasa(char[,] tenger, char szigetJel)
        {
            Console.WriteLine("\n\nInformációk:");
            Console.WriteLine($"Pálya mérete: {tenger.GetLength(0)} sor x {tenger.GetLength(1)} oszlop");
            //1.feladat
            //Hány sziget van a tengeren?
            Console.WriteLine($"A tengeren {SzigetekSzama(tenger, szigetJel)} db sziget van!");

            //2. feladat
            //Hány sziget van tenger szélén?
            Console.WriteLine($"A tenger szélén {TengerSzelenSzigetekSzama(tenger, szigetJel)} db sziget van!");

           
        }

        static char[,] BetoltTerkepet(string terkepNev)
        {
            string[] sorok = File.ReadAllLines(terkepNev);
            char[,] terkep = new char[sorok.Length, sorok[0].Length];
            for (int sorIndex = 0; sorIndex < terkep.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < terkep.GetLength(1); oszlopIndexe++)
                {
                    terkep[sorIndex, oszlopIndexe] = sorok[sorIndex][oszlopIndexe];
                }
            }
            return terkep;
        }
        static void MentTerkepet(char[,] terkep, string terkepNev)
        {
            string[] sorok = new string[terkep.GetLength(0)];
            string sor = "";
            for (int sorIndex = 0; sorIndex < terkep.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < terkep.GetLength(1); oszlopIndexe++)
                {
                    sor += terkep[sorIndex, oszlopIndexe];
                }
                sorok[sorIndex] = sor;
                sor = "";
            }
            File.WriteAllLines(terkepNev, sorok);
        }

        static char ValasztMenubol()
        {
            Console.WriteLine("\nMenü");
            Console.WriteLine("\t[g]enerál egy pályát");
            Console.WriteLine("\t[u]res pálya - A meglévő pálya alaphelyzetbe hozása");
            Console.WriteLine("\t[s]zigetek elhelyezése");
            Console.WriteLine("\t[b]etöltés fájlból");
            Console.WriteLine("\t[m]entés fájlba");
            Console.WriteLine("\t[p]álya szerkesztése");
            Console.WriteLine("\nKilépés az ESC billentyűvel, kérem válasszon!");
            char valasz;
            do
            {
                valasz = Console.ReadKey().KeyChar;
                if (valasz == 'b' || valasz == 'm' || valasz == 'p' || valasz == 's'
                    || valasz == 'u' || valasz == 'g')
                    return valasz;
            } while (valasz != (char)27);

            return ' ';
        }
        static bool VanSzomszedja(char[,] terkep, char jel, int sor, int oszlop)
        {
            sor--; //Az emberi számozás 1-től a gépi 0-tól indul, ezért a korrekció!
            oszlop--;
            if (sor - 1 >= 0 && terkep[sor - 1, oszlop] == jel)  //Felette van-e?
                return true;
            if (sor + 1 < terkep.GetLength(0) && terkep[sor + 1, oszlop] == jel)  //Alatta van-e?
                return true;
            if (oszlop - 1 >= 0 && terkep[sor, oszlop - 1] == jel)  //Balra van-e?
                return true;
            if (oszlop + 1 < terkep.GetLength(1) && terkep[sor, oszlop + 1] == jel)  //Jobbra van-e?
                return true;
            return false; //Ha egyetlen szomszédja sem sziget!

        }
        

        static int SzigetekSzama(char[,] terkep, char keresettJel)
        {
            int szigetSzam = 0;
            for (int sorIndex = 0; sorIndex < terkep.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < terkep.GetLength(1); oszlopIndexe++)
                {
                    if (terkep[sorIndex, oszlopIndexe] == keresettJel)
                    {
                        szigetSzam++;
                    }
                }
            }
            return szigetSzam;
        }

        static int TengerSzelenSzigetekSzama(char[,] terkep, char keresettJel)
        {
            int szigetSzam = 0;
            for (int sorIndex = 0; sorIndex < terkep.GetLength(0); sorIndex++)
            {
                if (terkep[sorIndex, 0] == keresettJel) //bal szélső oszlop
                    szigetSzam++;
                if (terkep[sorIndex, terkep.GetLength(1) - 1] == keresettJel) //jobb szélső oszlop
                    szigetSzam++;
            }
            for (int oszlopIndexe = 0; oszlopIndexe < terkep.GetLength(1); oszlopIndexe++)
            {
                if (terkep[0, oszlopIndexe] == keresettJel) //felső sor
                    szigetSzam++;
                if (terkep[terkep.GetLength(0) - 1, oszlopIndexe] == keresettJel) //alsó sor
                    szigetSzam++;
            }
            return szigetSzam;
        }

        static void SzigeteketRak(char SZIGET_JEL, char[,] terkep, int darab)
        {
            Random vel = new Random();
            for (int i = 0; i < darab; i++)
            {
                int sorIndex = vel.Next(terkep.GetLength(0));
                int oszlopIndexe = vel.Next(terkep.GetLength(1));
                terkep[sorIndex, oszlopIndexe] = SZIGET_JEL;
            }
        }

        static void Alaphelyzet(char TENGER_JEL, char[,] terkep)
        {
            for (int sorIndex = 0; sorIndex < terkep.GetLength(0); sorIndex++)
            {
                for (int oszlopIndexe = 0; oszlopIndexe < terkep.GetLength(1); oszlopIndexe++)
                {
                    terkep[sorIndex, oszlopIndexe] = TENGER_JEL;
                }
            }
        }

        static void Megjelenit(char[,] terkep, bool vanSzegely = false)
        {
            Console.Clear();
            if (vanSzegely)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(' ');
                for (int oszlopIndex = 1; oszlopIndex <= terkep.GetLength(1); oszlopIndex++)
                {
                    if (oszlopIndex % 10 == 0)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write(oszlopIndex % 10);
                    }
                }
            }
            Console.WriteLine();
            for (int sorIndex = 0; sorIndex < terkep.GetLength(0); sorIndex++)
            {
                if (vanSzegely)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if ((sorIndex + 1) % 10 == 0)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        Console.Write((sorIndex + 1) % 10);
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                for (int oszlopIndexe = 0; oszlopIndexe < terkep.GetLength(1); oszlopIndexe++)
                {
                    Console.Write(terkep[sorIndex, oszlopIndexe]);
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}