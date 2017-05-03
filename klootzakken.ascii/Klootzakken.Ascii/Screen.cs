using System;
using System.Collections.Generic;
using System.IO;

namespace Klootzakken.Ascii
{
    public class Screen : IScreen
    {
        public void Log(string message)
        {
            File.AppendAllText("Klootzakken.Ascii.log", $"{message}\r\n");
        }

        public void Display(string message)
        {
            Console.WriteLine(message);
            Log(message);
        }

        public string GetValue()
        {
            var line = Console.ReadLine();
            if (line?.Trim() == "exit")
            {
                Exit?.Invoke();
            }
            return line;
        }

        public event Func<bool> Exit;
        public void Clear()
        {
            Console.Clear();
        }

        public void Display(List<string> data)
        {
            data.ForEach(value =>
            {
                Console.Write(value);
                Console.Write(" ");
            });
            Console.WriteLine();
        }

        public void Validate()
        {
            if (Console.KeyAvailable)
            {
                GetValue();
            }
        }
    }
}
