using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTTE
{
    public class TipsManager
    {
        private readonly int choice;
        private readonly List<string> tips = new List<string> { 
         "You can type ' -e' after the file name when creating a file to instantly enter it",
         "You can delete multiple files by just separating their names with spaces",
         "You can use the same console options when editing text by pressing ctrl + 'option' i.e. ctrl+ Numpad8 will toggle theme"
        };
        public TipsManager()
        {
            Random random = new Random();
            choice = random.Next(0, tips.Count);
        }

        public void PrintTipOfTheDay()
        {
            Console.WriteLine(tips[choice]);
            Console.WriteLine();
        }
    }
}
