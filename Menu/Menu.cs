using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2_Bildtjanster_I_Azure_AI.Handlers
{
    internal class Menu
    {
        private int SelectedIndex;
        private string[] Options;
        private string Prompt;
        private int Index = 0;

        //public Menu()
        //{
        //    //string prompt, string[] options, int selectedIndex = 0
        //    //Prompt = prompt;
        //    //Options = options;
        //    //SelectedIndex = selectedIndex;
        //}

        private void DisplayOptions()
        {
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          Image Analyzer                         ║");
            Console.WriteLine("╠═════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ {Prompt,-63} ║\n║                                                                 ║");

            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];

                if (i == SelectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{currentOption,-65} ║");
            }

            Console.ResetColor();
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
            Index = SelectedIndex;
        }

        public int Run()
        {
            //SelectedIndex = index;
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                // Update SelectedIndex based on arrow keys
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;

                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = Options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;

                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }

            } while (keyPressed != ConsoleKey.Enter);
            return SelectedIndex;
            GetIndex();
        }

        public void Indexo(int indexo)
        {
            SelectedIndex = indexo;
        }

        public void SetIndex(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
        }

        public int GetIndex()
        {
            return Index;
        }
    }
}
