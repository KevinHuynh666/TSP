using System;
using System.IO;
using System.Collections.Generic;
namespace Assignment
{
    class Program
    {

        static void Main(string[] args)
        {
            int choice = 0;
            /*Programs need at least 3 arguments to run, so if arguments < 3 then it's missing*/
            if (args.Length < 3)
            {
                throw new NotEnoughArgsException(string.Format("Not enough arguments, please try again!"));
            }

            try
            {
                /*Check how many stations are there in the file*/
                string[] lines = File.ReadAllLines(args[0]);

                if (lines.Length > 12)
                {
                    /*Program will run level 2 if there are more than 12 stations*/ 
                    choice = 2;
                }
                else
                {
                    /*Program will run level 3 if there are less than 12 stations*/
                    choice = 3;
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Stations file not found, check the arguments again!");
                throw ex;
            }
                                 
            choice = 5;
            Solve Result = new Solve();
            List<Station> ResultTour = new List<Station>();
            
            if (choice == 4)
            {
                Result.Level4(args);
            }
            else if (choice == 2)
            {
                Result.Level2(args);
            }
            else if (choice == 3)
            {
                Result.Level3(args);
            }
            else if (choice == 1)
            {
                Result.Level1(args);
            }
            else if (choice == 5)
            {
                Result.Level5(args);
            }
            
           
            Console.ReadLine();

        }
    }
}
