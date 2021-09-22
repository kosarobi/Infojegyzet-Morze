using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Morze
{
    class Program
    {
        static Dictionary<string, string> abclista = new Dictionary<string, string>();
        static List<IdezetLista> idezetlista = new List<IdezetLista>();
        static List<DekodoltLista> dekodoltLista = new List<DekodoltLista>();

        static string BekerAdat;

        static void Main(string[] args)
        {
            abcbeolvas();
            idezetolvas();
            f03();
            f04();
            f07();
            f08();
            f09();
            f10();
            Console.ReadKey();
        }

        private static void f10()
        {
            StreamWriter sw = new StreamWriter("forditas.txt");
            foreach (var item in dekodoltLista)
            {
                sw.WriteLine($"{item.Szerzo} : {item.Idezet}");
            }
            sw.Close();
        }

        private static void f09()
        {
        
            var arisztoteles = dekodoltLista.Where(x => x.Szerzo.Contains("ARISZTOTELÉSZ"));
            Console.WriteLine("9. feladat: Arisztotelész idézetek");
            foreach (var item in arisztoteles)
            {
                Console.WriteLine($"\t{item.Idezet}");
            }
        }

        private static void f08()
        {

            foreach (var item in idezetlista)
            {
                //megcsinálom a dekódolt szöveget tartalmazó listát, amivel könnyebb lesz dolgozni a következő feladatokban
                dekodoltLista.Add(new DekodoltLista(Morze2Szoveg(item.Szerzo), Morze2Szoveg(item.Idezet)));
            }

            Console.WriteLine($"8. feladat");
            //a dekódolt idézet hossza szerint sorbarendezem és kiiratom.
            var leghosszabb = dekodoltLista.OrderByDescending(x => x.Idezet.Length).First();
            Console.WriteLine($"A leghosszabb idézet szerzője {leghosszabb.Szerzo}: {leghosszabb.Idezet}");
        }

        private static void f07()
        {
            Console.WriteLine($"7. feladat: {Morze2Szoveg(idezetlista.Select(x=>x.Szerzo).First())}");
        }

        private static string Morze2Szoveg(string szoveg)
        {
            string dekodolt = "";

            //elbontjuk az szöveget szavakra a delimiter mentén
            string[] szavak = szoveg.Split('|');
            
            //bejáruk a kapott szavakat tartalmazó tömböt
            foreach (var szo in szavak)
            {
                //felbontjuk a szót betűkre a delimiter mentén
                string[] betuk = szo.Split('#');

                //bejárjuk a kapott szó betűi tartalmazó tömböt
                for (int j = 0; j < betuk.Length; j++)
                {
                    //a betűket kicserélgetjük az abclista szótárban található kulcsnak megfelelően
                    //ahol a morze betű egyezik a tárolt morze kulcssal
                    dekodolt += abclista[betuk[j]];
                }
                //a szó végére kell egy szóköz
                dekodolt += " ";
            }

            return dekodolt;
        }

        private static void f04()
        {
            try
            {
                Console.Write("4. feladat: Kérek egy karaktert:");
                BekerAdat = Console.ReadLine();
                Console.WriteLine($"\t A bekért karakter morze kódja:{abclista.First(x=>x.Value==BekerAdat.ToUpper()).Key}");
            }
            catch (Exception)
            {
                Console.WriteLine("A bekér karakter nincs a karekterek közt!");
            }
        }


        private static void f03()
        {

            Console.WriteLine($"3. feladat:\n\tA morzeabx.txt állományban {abclista.Values.Count()} karakter található");
        }

        //idézet sorainak listába olvasása
        private static void idezetolvas()
        {
            StreamReader sr = new StreamReader("morze.txt");
            while (!sr.EndOfStream)
            {
                idezetlista.Add(new IdezetLista(sr.ReadLine()));
            }
            sr.Close();
        }

        //abc beolvasása szótárba
        private static void abcbeolvas()
        {
            StreamReader sr = new StreamReader("morzeabc.txt");
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split('\t');
                //itt a trükk hogy az abclista kulcsa lesz a morze kód az értéke pedig a karakter
                abclista.Add(sor[1], sor[0]);
            }
            sr.Close();
        }
    }



    //idézetek és szerzők beolvasása
    class IdezetLista
    {
        private string szerzo;
        private string idezet;

        public IdezetLista(string sor)
        {
            string[] adatok = sor.Split(';');
            szerzo = adatok[0];
            //a szavakat | választja el, a betűket # karakter
            szerzo = szerzo.Replace("       ", "|");
            szerzo = szerzo.Replace("   ", "#");
            szerzo = szerzo.Remove(szerzo.Length - 1); //le kell szedni az utolsó karaktert mert később be fog zavarni

            idezet = adatok[1];
            idezet = idezet.Replace("       ", "|");
            idezet = idezet.Replace("   ", "#");
        }

        public string Szerzo { get => szerzo; set => szerzo = value; }
        public string Idezet { get => idezet; set => idezet = value; }
    }

    //csak mert ezzel könnyebb dolgozni :D
    class DekodoltLista
    {
        private string szerzo;
        private string idezet;
        
        public DekodoltLista(string sz, string idez)
        {
            szerzo = sz;
            idezet = idez; 
        }

        public string Szerzo { get => szerzo; set => szerzo = value; }
        public string Idezet { get => idezet; set => idezet = value; }
    }
}
