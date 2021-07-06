using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Assignment
{
    class Solve
    {
        /*Calculate the tour length*/
        static double totalLength(List<Station> listStations, int n)
        {
            double total = 0;
            for (int i = 0; i <= n - 1; i++)
            {
                if (i == n - 1)
                {
                    /*Calculating the distance from last station to PO*/
                    total += listStations[i].calculateDistance(listStations[0]);
                }
                else
                {
                    /*Calculating the distance from station[i] to station[i+1]*/
                    total += listStations[i].calculateDistance(listStations[i + 1]);

                }
            }
            return total;
        }

        static Station firstStation = new Station();
        static int n = 0;

        /*Read Stations*/
        static List<Station> ReadStations(string fileName)
        {
            try
            {
                string[] lines = File.ReadAllLines(fileName);

                List<Station> listOfStation = new List<Station>();

                int i = 0;
                foreach (string line in lines)
                {
                    string[] split = line.Split(' ');
                    if (split.Length != 3)
                    {
                        /*Throw error for providing plane-spec file instead of stations file*/
                        throw new WrongFileException(string.Format("Wrong file provided, please check the argument again!"));
                    }

                    Station testStat = new Station();

                    try
                    {
                        testStat.NamingStation(split[0], split[1], split[2]);
                    }
                    catch (FormatException ex)
                    {
                        /*Throw error for incorrect format of the station */
                        Console.WriteLine("Error in input file, check your input again!");
                        throw ex;
                    }

                    listOfStation.Add(testStat);

                    i++;
                }
                n = listOfStation.Count;
                firstStation = listOfStation[0];
                return listOfStation;
            }
            catch (FileNotFoundException ex)
            {
                /*Throw error for can not find stations file*/
                Console.WriteLine("Stations file not found, check the arguments again!");
                throw ex;
            }
        }
        /*Read Stations*/



        /*Read Plane Spec*/
        static Plane ReadPlane(string fileName)
        {

            Plane plane = new Plane();

            try
            {
                string[] planeInput = File.ReadAllLines(fileName);
                string[] planeSplit = planeInput[0].Split(' ');
                if (planeSplit.Length != 5)
                {
                    /*Throw error for providing stations file instead of plane-spec file*/
                    throw new WrongFileException(string.Format("Wrong file provided, please check the argument again!"));
                }
                try
                {
                    plane.addPlane(planeSplit[0], planeSplit[1], planeSplit[2], planeSplit[3], planeSplit[4]);
                }
                catch (FormatException ex)
                {
                    /*Throw error for incorrect format of the plane specs */
                    Console.WriteLine("Error in input file, check your input again!");
                    throw ex;
                }


            }
            catch (FileNotFoundException ex)
            {
                /*Throw error for can not find plane-spec file*/
                Console.WriteLine("Plan Spec file not found, check the arguments again!");
                throw ex;
            }

            return plane;
        }

        /*Read Plane Spec*/

        /*Doing level 1 */
        static List<Station> findLevel1(string[] args)
        {
            /* Getting stations information*/
            List<Station> listOfStation = ReadStations(args[0]);

            /*Calculate distance*/

            double t1, t2, tAdd, tMin;
            /*Init the result list of level 1*/
            List<Station> TourList1 = new List<Station>();

            /*Add the first 2 stations to the list*/
            TourList1.Add(listOfStation[0]);
            TourList1.Add(listOfStation[1]);

            int m = 2;
            int pos;
            double currLen = 0;


            for (int i = 2; i < n; i++)
            {
                /*Create a station to calculate*/
                Station tempStation = listOfStation[i];

                /*Calculate current tour length*/
                currLen = totalLength(TourList1, m);

                /*Calculate tour length when insert temp into last position*/
                tMin = (currLen - TourList1[0].calculateDistance(TourList1[m - 1]))
                    + TourList1[m - 1].calculateDistance(tempStation)
                    + TourList1[0].calculateDistance(tempStation);

                pos = i;

                for (int j = 0; j < m - 1; j++)
                {
                    /*Caclulate distance from temp to the previous station*/
                    t1 = TourList1[j].calculateDistance(tempStation);

                    /*Caclulate distance from temp to the next station*/
                    t2 = TourList1[j + 1].calculateDistance(tempStation);

                    /*Calculate the tour length after insert temp*/
                    tAdd = currLen + t1 + t2 - TourList1[j].calculateDistance(TourList1[j + 1]);

                    /*Compare new length with min length*/
                    if (tAdd <= tMin)
                    {
                        tMin = tAdd;
                        pos = j + 1;
                    }

                }
                /*Insert temp to the best position*/
                TourList1.Insert(pos, tempStation);
                m++;
            }
            Station temp = TourList1[0];
            TourList1.Reverse();
            TourList1.Remove(temp);
            TourList1.Insert(0, temp);

            return TourList1;
        }
        /*Doing level 1 */

        /*Level 1*/
        public void Level1(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();


            List<Station> TourList1 = findLevel1(args);

            Plane plane = new Plane();
            /* Getting plane information*/
            try
            {
                plane = ReadPlane(args[1]);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Arguments are not fully provided.");
                throw ex;
            }
            /* Getting plane information*/

            /*Init the tour variable to store the result and time*/
            Tour Lvl1 = new Tour();

            /*Get the result tour*/
            Lvl1.getTour(TourList1);

            /*Get the start time*/
            try
            {
                Lvl1.startTime = TimeSpan.Parse(args[2]);
                Lvl1.sTime = TimeSpan.Parse(args[2]);
            }
            catch (Exception ex)
            {
                /*Throw error for can not find the time*/
                Console.WriteLine("Start time not provided");
                throw ex;
            }
            Lvl1.checkTime(plane);

            /*Print out the result*/


            double totalDistance = totalLength(TourList1, TourList1.Count);

            TimeSpan totalTime = TimeSpan.FromMinutes(Lvl1.totalTime());
            Console.WriteLine("Optimising tour length: Level 1...");
            var elapsed = watch.Elapsed;
            Console.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
            Console.WriteLine("Tour time: {0} ", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
            Console.WriteLine("Total length: " + Math.Round(totalDistance, 4));
            Lvl1.printTime();
            /*Print out the result*/

            /*Write out the result*/
            if (args.Length > 3)
            {
                Console.WriteLine("Saving itinerary to itinerary.txt");


                string outputFile = args[4];
                StreamWriter fo = new StreamWriter(outputFile);
                fo.WriteLine("Optimising tour length: Level 1...");
                fo.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
                fo.WriteLine("Tour time: {0} minutes", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
                fo.WriteLine("Total length: " + Math.Round(totalDistance, 4));
                Lvl1.writeTime(fo);
                fo.Close();
            }
            /*Write out the result*/

        }
        /*Level 1*/

        /*Level 2*/
        public void Level2(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            /*Get level 1 tour*/
            List<Station> resultTour = findLevel1(args);
            /*Get level 1 tour*/

            double resultDist = 0;

            /*Minimum length is the length of lvl 1 tour*/
            double minLen = totalLength(resultTour, resultTour.Count);
            resultDist = minLen;

            int i = 1;

            do
            {
                List<Station> TourList2 = new List<Station>(resultTour.ToArray());
                /*Create a test station to calculate*/
                Station temp = new Station();
                temp = TourList2[i];

                double currImpact = double.MaxValue;
                int pos = 1;
                TourList2.Remove(temp);
                for (int j = 1; j <= TourList2.Count; j++)
                {
                    double newImpact = 0;
                    /*Calculate the impact when insert a station into a gap*/
                    if (j == TourList2.Count)
                    {
                        /*If the station is the last station*/
                        newImpact = temp.calculateDistance(TourList2[0])
                                  + temp.calculateDistance(TourList2[j - 1])
                                  - TourList2[j - 1].calculateDistance(TourList2[0]);
                    }

                    else
                    {
                        /*If the station is not the last station*/
                        newImpact = temp.calculateDistance(TourList2[j])
                                  + temp.calculateDistance(TourList2[j - 1])
                                  - TourList2[j - 1].calculateDistance(TourList2[j]);
                    }

                    if (newImpact < currImpact)
                    {
                        pos = j;
                        currImpact = newImpact;
                    }
                }



                /*Insert the station at pos and calculate the tour length after insert*/
                TourList2.Insert(pos, temp);
                double currLen = totalLength(TourList2, TourList2.Count);

                /*Compare to minimum tour length and break through the loop if smaller*/
                if (currLen < minLen)
                {
                    resultTour = new List<Station>(TourList2.ToArray());
                    minLen = currLen;
                    i = 1;
                }
                else
                {
                    i++;
                }

            }
            while (i < resultTour.Count);

            resultDist = minLen;
            /*Init the tour variable to store the result and time*/
            Tour Tour2 = new Tour();

            /* Get the result tour*/
            Tour2.getTour(resultTour);

            /*Get the start time*/
            try
            {
                Tour2.startTime = TimeSpan.Parse(args[2]);
                Tour2.sTime = TimeSpan.Parse(args[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start time not provided");
                throw ex;
            }


            /* Getting plane information*/
            Plane plane = ReadPlane(args[1]);
            /* Getting plane information*/

            /*Print out the result*/
            var elapsed = watch.Elapsed;
            Tour2.checkTime(plane);
            TimeSpan totalTime = TimeSpan.FromMinutes(Tour2.totalTime());
            Console.WriteLine("Optimising tour length: Level 2...");
            Console.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
            Console.WriteLine("Tour time: {0} ", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
            Console.WriteLine("Total length: " + Math.Round(resultDist, 4));
            Tour2.printTime();
            /*Print out the result*/

            /*Check if -o and itinerary file is provided*/
            if (args.Length > 3)
            {
                Console.WriteLine("Saving itinerary to itinerary.txt");
                /*Write out the result*/
                string outputFile = args[4];
                StreamWriter fo = new StreamWriter(outputFile);
                fo.WriteLine("Optimising tour length: Level 2...");
                fo.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
                fo.WriteLine("Tour time: {0} minutes", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
                fo.WriteLine("Total length: " + Math.Round(resultDist, 4));
                Tour2.writeTime(fo);
                fo.Close();
                /*Write out the result*/
            }
        }
        /*Level 2*/


        /*Level 3*/

        static List<Station> TourLvl3;
        static double MinDistance = 0;
        static List<Station> Lvl3 = new List<Station>();

        static void Generate(string[] args)

        {
            /*Get stations info*/
            TourLvl3 = ReadStations(args[0]);

            /*Set minimum length to default tour length*/
            MinDistance = totalLength(TourLvl3, TourLvl3.Count);

            /*Start permuation*/
            generate(TourLvl3.Count, TourLvl3);

        }

        static void generate(int k, List<Station> array)
        {
            if (k == 1)
            {
                /*Check if the current permutation starts with PO*/
                if (array[0] == firstStation)
                {
                    double currLen = totalLength(array, array.Count);

                    /*Compare new tour length with minimum length*/
                    if (currLen < MinDistance)
                    {
                        Lvl3.Clear();
                        for (int i = 0; i < array.Count; i++)
                        {
                            Station testStat = new Station();
                            testStat.NamingStation(array[i].Name, array[i].x.ToString(), array[i].y.ToString());
                            Lvl3.Add(testStat);
                        }
                        MinDistance = currLen;
                    }
                }
            }
            else
            {
                /*Permutation*/
                generate(k - 1, array);
                for (int i = 0; i < k - 1; i++)
                {
                    if (k % 2 == 0)
                    {
                        Station num = new Station();
                        num = array[i];
                        array[i] = array[k - 1];
                        array[k - 1] = num;
                    }
                    else
                    {
                        Station num = new Station();
                        num = array[0];
                        array[0] = array[k - 1];
                        array[k - 1] = num;
                    }
                    generate(k - 1, array);

                }

            }

        }
        public void Level3(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            /* Getting plane information*/
            Plane plane = ReadPlane(args[1]);
            /* Getting plane information*/

            /*Do level 3*/
            Generate(args);

            var elapsed = watch.Elapsed;


            Tour Tour3 = new Tour();

            List<Station> TourList3 = Lvl3;

            Tour3.getTour(TourList3);
            try
            {
                Tour3.startTime = TimeSpan.Parse(args[2]);
                Tour3.sTime = TimeSpan.Parse(args[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start time not provided");
                throw ex;
            }


            Tour3.checkTime(plane);
            TimeSpan totalTime = TimeSpan.FromMinutes(Tour3.totalTime());
            /*Print out the result*/
            Console.WriteLine("Optimising tour length: Level 3...");
            Console.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
            Console.WriteLine("Tour time: {0} ", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
            Console.WriteLine("Total length: " + Math.Round(MinDistance, 4));
            Tour3.printTime();
            /*Print out the result*/



            if (args.Length > 3)
            {
                Console.WriteLine("Saving itinerary to itinerary.txt");
                /*Write out the result*/
                string outputFile = args[4];
                StreamWriter fo = new StreamWriter(outputFile);
                fo.WriteLine("Optimising tour length: Level 3...");
                fo.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
                fo.WriteLine("Tour time: {0} minutes", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
                fo.WriteLine("Total length: " + Math.Round(MinDistance, 4));
                Tour3.writeTime(fo);
                fo.Close();
                /*Write out the result*/
            }
        }
        /*Level 3*/

        /*Level 4*/
        private static List<Station> TwoOptSwap(List<Station> TourList, int i, int k)
        {
            List<Station> newTour = new List<Station>();
            int n = TourList.Count;
            for (int j = 0; j < i; j++)
            {
                newTour.Add(TourList[j]);
            }

            for (int j = k; j >= i; j--)
            {
                newTour.Add(TourList[j]);
            }
            for (int j = k + 1; j < n; j++)
            {
                newTour.Add(TourList[j]);
            }
            return newTour;
        }


        public void Level4(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<Station> TourList4 = ReadStations(args[0]);

            List<Station> resultList = new List<Station>();
            int n = TourList4.Count;
            bool check = true;

            while (check)
            {
                check = false;
                double minLen = totalLength(TourList4, TourList4.Count);

                for (int i = 1; i < n - 1; i++)
                {
                    for (int k = i + 1; k < n; k++)
                    {
                        List<Station> newTour = new List<Station>();
                        newTour = TwoOptSwap(TourList4, i, k);
                        double currLen = totalLength(newTour, newTour.Count);
                        if (currLen < minLen)
                        {
                            check = true;
                            TourList4 = new List<Station>(newTour.ToArray());
                            minLen = currLen;
                            break;
                        }
                    }
                    if (check)
                    { break; }
                }
            }

            var elapsed = watch.Elapsed;
            resultList = TourList4;
            resultList.RemoveAt(resultList.Count - 1);

            /* Getting plane information*/
            Plane plane = ReadPlane(args[1]);
            /* Getting plane information*/
            Tour Tour4 = new Tour();


            Tour4.getTour(resultList);
            try
            {
                Tour4.startTime = TimeSpan.Parse(args[2]);
                Tour4.sTime = TimeSpan.Parse(args[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start time not provided");
                throw ex;
            }

            Tour4.checkTime(plane);
            TimeSpan totalTime = TimeSpan.FromMinutes(Tour4.totalTime());

            double MinDistance = totalLength(resultList, resultList.Count);

            Console.WriteLine("Optimising tour length: Level 4...");
            Console.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
            Console.WriteLine("Tour time: {0} ", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
            Console.WriteLine("Total length: " + Math.Round(MinDistance, 4));
            Tour4.printTime();

        }
        /*Level 4*/

        /*Bonus Level*/
        public void Level5(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            /*Get level 1 tour*/
            List<Station> resultTour = findLevel1(args);
            /*Get level 1 tour*/

            /* Getting plane information*/
            Plane plane = ReadPlane(args[1]);
            /* Getting plane information*/

            double resultDist = 0;

            /*Minimum length is the length of lvl 1 tour*/
            double minLen = totalLength(resultTour, resultTour.Count);
            resultDist = minLen;

            int i = 1;
            double limit = plane.getRange() * plane.getSpeed();
            do
            {
                List<Station> TourList2 = new List<Station>(resultTour.ToArray());
                /*Create a test station to calculate*/
                Station temp = new Station();
                temp = TourList2[i];

                double currImpact = double.MaxValue;
                int pos = 1;
                TourList2.Remove(temp);
                for (int j = 1; j <= TourList2.Count; j++)
                {
                    double newImpact = 0;
                    /*Calculate the impact when insert a station into a gap*/
                    if (j == TourList2.Count)
                    {
                        /*If the station is the last station*/
                        newImpact = temp.calculateDistance(TourList2[0])
                                  + temp.calculateDistance(TourList2[j - 1])
                                  - TourList2[j - 1].calculateDistance(TourList2[0]);
                    }

                    else
                    {
                        /*If the station is not the last station*/
                        newImpact = temp.calculateDistance(TourList2[j])
                                  + temp.calculateDistance(TourList2[j - 1])
                                  - TourList2[j - 1].calculateDistance(TourList2[j]);
                    }

                    if (newImpact < currImpact)
                        if (newImpact <= limit)
                        {
                            pos = j;
                            currImpact = newImpact;
                        }
                }



                /*Insert the station at pos and calculate the tour length after insert*/
                TourList2.Insert(pos, temp);
                double currLen = totalLength(TourList2, TourList2.Count);

                /*Compare to minimum tour length and break through the loop if smaller*/
                if (currLen < minLen)
                {
                    resultTour = new List<Station>(TourList2.ToArray());
                    minLen = currLen;
                    i = 1;
                }
                else
                {
                    i++;
                }

            }
            while (i < resultTour.Count);

            resultDist = minLen;
            /*Init the tour variable to store the result and time*/
            Tour Tour2 = new Tour();

            /* Get the result tour*/
            Tour2.getTour(resultTour);

            /*Get the start time*/
            try
            {
                Tour2.startTime = TimeSpan.Parse(args[2]);
                Tour2.sTime = TimeSpan.Parse(args[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Start time not provided");
                throw ex;
            }




            /*Print out the result*/
            var elapsed = watch.Elapsed;
            Tour2.checkTime(plane);
            TimeSpan totalTime = TimeSpan.FromMinutes(Tour2.totalTime());
            Console.WriteLine("Optimising tour length: Level 2...");
            Console.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
            Console.WriteLine("Tour time: {0} ", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
            Console.WriteLine("Total length: " + Math.Round(resultDist, 4));
            Tour2.printTime();
            /*Print out the result*/

            /*Check if -o and itinerary file is provided*/
            if (args.Length > 3)
            {
                Console.WriteLine("Saving itinerary to itinerary.txt");
                /*Write out the result*/
                string outputFile = args[4];
                StreamWriter fo = new StreamWriter(outputFile);
                fo.WriteLine("Optimising tour length: Level 2...");
                fo.WriteLine("Elapsed time: {0}", elapsed.ToString("s'.'fff"));
                fo.WriteLine("Tour time: {0} minutes", totalTime.ToString("d' day(s) 'h' hour(s) 'mm' minute(s)'"));
                fo.WriteLine("Total length: " + Math.Round(resultDist, 4));
                Tour2.writeTime(fo);
                fo.Close();
                /*Write out the result*/
            }
        }
        /*Bonus Level*/
    }


}
