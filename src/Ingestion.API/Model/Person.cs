using System;
using System.Linq;

namespace Ingestion.API.Model
{
    public class Person
    {
        public Person(string eyeColour, string hairColour, string postcode, int height, string health)
        {
            EyeColour = eyeColour;
            HairColour = hairColour;
            Postcode = postcode;
            Height = height;
            Health = health;
        }

        public string EyeColour { get; set; }
        public string HairColour { get; set; }
        public string Postcode { get; set; }
        public int Height { get; set; }
        public string Health { get; set; }
    }

    public static class PersonFactory
    {
        public static Person GetGenericPerson()
        {
            return new Person(GetRandomEyeColour(),
                GetRandomHairColour(),
                GetRandomPostcode(),
                GetRandomHeight(),
                GetRandomHealth());
        }

        private static string GetRandomHealth()
        {
            var possibleHealth = new[] {"Health", "Unhealthy"};
            var random = new Random(DateTime.UtcNow.Millisecond);
            return possibleHealth[random.Next(0, possibleHealth.Length-1)];
        }

        private static int GetRandomHeight()
        {
            var random = new Random(DateTime.UtcNow.Millisecond);
            return random.Next(140, 200);
        }

        private static string GetRandomPostcode()
        {
            return RandomString(6);
        }

        private static string RandomString(int length)
        {
            var random = new Random(DateTime.UtcNow.Millisecond);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string GetRandomHairColour()
        {
            var possibleHairColour = new[] {"Blonde", "Brown", "Black", "Red", "Grey"};
            var random = new Random(DateTime.UtcNow.Millisecond);
            return possibleHairColour[random.Next(0, possibleHairColour.Length-1)];
        }

        private static string GetRandomEyeColour()
        {
            var PossibleEyeColour = new[] {"Blue", "Green", "Brown"};
            var random = new Random(DateTime.UtcNow.Millisecond);
            return PossibleEyeColour[random.Next(0, PossibleEyeColour.Length-1)];
        }
    } 
}