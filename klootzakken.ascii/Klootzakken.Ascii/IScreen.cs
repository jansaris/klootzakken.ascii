using System;
using System.Collections.Generic;

namespace Klootzakken.Ascii
{
    public interface IScreen
    {
        void Log(string message);
        void Display(string message);
        string GetValue();

        event Func<bool> Exit;
        void Clear();
        void Display(List<string> data);
        void Validate();
    }
}