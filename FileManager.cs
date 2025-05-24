using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CTTE
{
    internal class FileManager
    {
        private Editor Editor { get; set; }

        public FileManager(Editor editor)
        {
            Editor = editor;
        }
        public void DeleteFile(string path, string extension)
        {
            Console.Write("files to delete:  ");
            string? filesToDelete = Console.ReadLine();
            if (filesToDelete == "") return;
            string[] files = filesToDelete.Split(' ');
            foreach (string file in files)
            {
                if (File.Exists(Path.Combine(path, file) + extension)) File.Delete(Path.Combine(path, file) + extension);
                else Console.WriteLine($"file {file} doesnt exist");
            }               
        }

        //TODO  -e parameter to enter file after it has been created so not to press 1 two times
        public void CreateFile(string path, string extension)
        {
            Console.Write("filename:  ");
            string? fileName = Console.ReadLine();
            if (fileName == "") return;


            // test -e
            if (fileName.Substring(fileName.Length - 2) == "-e" && fileName.IndexOf(' ') != -1 && !File.Exists(Path.Combine(path, fileName.Substring(0, fileName.IndexOf(' '))) + extension))
            {
                using (FileStream fs = File.Create(Path.Combine(path, fileName.Substring(0, fileName.IndexOf(' '))) + extension))
                {
                    Editor.OpenEditor(fs);
                }
            }

            // can't create files with whitespaces now

            // refactor this to FileMode.OpenOrCreate   
            if (fileName.Substring(fileName.Length - 2) != "-e" && fileName.IndexOf(' ') == -1 && !File.Exists(Path.Combine(path, fileName) + extension))
            {

                // use File.CreateText()
                using (FileStream fs = File.Create(Path.Combine(path, fileName) + extension)) { }
            }
            else if (File.Exists(Path.Combine(path, fileName) + extension))
            {   
                FileStream fs = File.Open(Path.Combine(path, fileName) + extension, FileMode.Open, FileAccess.ReadWrite);
                Editor.OpenEditor(fs); // not case-sensitive
                fs.Close();
            }
            else
            {
                Console.WriteLine("file cannot be created or opened");
            }
        }
    }
}
