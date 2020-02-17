using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");

            // use File.ReadAllLines(path) to grab all the lines from your csv file
            // Log and error if you get 0 lines and a warning if you get 1 line
            var lines = File.ReadAllLines(csvPath);

            if(lines.Length == 0)
            {
                logger.LogError("Zero lines", new NullReferenceException());
            }

            if(lines.Length == 1)
            {
                logger.LogWarning("You're only entering one location!");
            }

            logger.LogInfo($"Lines: {lines[0]}");

            // Create a new instance of your TacoParser class
            var parser = new TacoParser();

            // Grab an IEnumerable of locations using the Select command: var locations = lines.Select(parser.Parse);
            var locations = lines.Select(parser.Parse).ToArray();

            // Create two `ITrackable` variables with initial values of `null`. These will be used to store your two taco bells that are the furthest from each other.
            // Create a `double` variable to store the distance


            var finalA = new TacoBell();
            var finalB = new TacoBell();
            finalA = null;
            finalB = null;

            double distance = 0;

            // Include the Geolocation toolbox, so you can compare locations: `using GeoCoordinatePortable;`
            // Do a loop for your locations to grab each location as the origin (perhaps: `locA`)
            // Create a new corA Coordinate with your locA's lat and long
            logger.LogInfo("Finding the distances between any two stores, and comparing each to return the farthest distance between two.");
            for(int i = 0; i < locations.Length; i++)
            {
                var locA = locations[i];
                var corA = new GeoCoordinate(locA.Location.Longitude, locA.Location.Latitude);
                for(int j = 0; j < locations.Length; j++)
                {
                    // Now, do another loop on the locations with the scope of your first loop, so you can grab the "destination" location (perhaps: `locB`)
                    // Create a new Coordinate with your locB's lat and long
                    // Now, compare the two using `.GetDistanceTo()`, which returns a double
                    // If the distance is greater than the currently saved distance, update the distance and the two `ITrackable` variables you set above

                    // Once you've looped through everything, you've found the two Taco Bells furthest away from each other.

                    // TODO:  Find the two Taco Bells in Alabama that are the furthest from one another.
                    // HINT:  You'll need two nested forloops

                    var locB = locations[j];
                    var corB = new GeoCoordinate(locB.Location.Longitude, locB.Location.Latitude);
                    var testDist = corA.GetDistanceTo(corB);
                    if(testDist > distance)
                    {
                        distance = testDist;
                        finalA = (TacoBell)locA;
                        finalB = (TacoBell)locB;
                    }
                }
            }
            Console.WriteLine($"The two farthest Taco Bell locations in Alabama are {finalA.Name} and {finalB.Name} with a distance of {Math.Round(distance * 0.000621371, 1)} miles.");
        }
    }
}