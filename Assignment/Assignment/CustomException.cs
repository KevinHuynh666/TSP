using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment
{
    public class NotEnoughArgsException : Exception
    {
        public NotEnoughArgsException(string message)
            : base(message)
        {
           Console.WriteLine("Not enough arguments, please try again!");
        }
    }

    public class WrongFileException: Exception
    {
        public WrongFileException(string message)
            :base(message)
        {
            Console.WriteLine("Wrong file provided, please try again!");
        }
    }
}
