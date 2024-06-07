using Backend.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class SeedModel
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Create a new database context using the provided service provider
            using (var context = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                // Check if the database context or any required tables are null, or if the Country table already has data
                if (context == null || context.Country == null || context.Offer == null || context.Country.Any())
                {
                    return; // If any condition is true, return early to avoid reseeding
                }

                // Create Country objects
                var Spain = new Country("Spain");
                var France = new Country("France");
                var Italy = new Country("Italy");
                var Portugal = new Country("Portugal");
                var Poland = new Country("Poland");
                var Germany = new Country("Germany");
                var Austria = new Country("Austria");
                var Belgium = new Country("Belgium");
                var Turkey = new Country("Turkey");
                var England = new Country("England");
                var Ireland = new Country("Ireland");

                var Countries = new List<Country>()
                {
                    Spain, France, Italy, Portugal, Germany, Poland, Austria, Belgium, Turkey,  England, Ireland
                };

                // Create City objects and associate them with countries
                var Cities = new List<City>()
                {
                    new City("Barcelona", Spain),
                    new City("Valencia", Spain),
                    new City("Madryt", Spain),
                    new City("Cracow", Poland),
                    new City("Warsaw", Poland),
                    new City("Sopot", Poland),
                    new City("Milan", Italy ),
                    new City("Rome", Italy ),
                    new City("Venice", Italy),
                    new City("Aveiro", Portugal),
                    new City("Porto", Portugal),
                    new City("Lisboa", Portugal),
                    new City("Paris", France),
                    new City("Lyon", France),
                    new City("Nicea", France),
                    new City("Berlin", Germany),
                    new City("Munich", Germany),
                    new City("Bonn", Germany),
                    new City("Vienna", Austria),
                    new City("Graz", Austria),
                    new City("Salzburg", Austria),
                    new City("Antalya", Turkey),
                    new City("Istanbul", Turkey),
                    new City("Ankara", Turkey),
                    new City("London", England),
                    new City("Nottingham", England),
                    new City("Southampton", England),
                    new City("Dublin", Ireland),
                    new City("Galway", Ireland),
                    new City("Cork", Ireland),
                    new City("Brussels", Belgium),
                    new City("Charleroi", Belgium),
                    new City("Antwerp", Belgium),
                };

                var Hotels = new List<Hotel>();

                // Read hotel names from file and create Hotel objects
                using (StreamReader sr = File.OpenText("Data/hotel_names.txt"))
                {
                    string? s = sr.ReadLine();
                    while (s != null)
                    {
                        Random random = new Random();
                        var index = random.Next(Cities.Count);
                        var hotel = new Hotel(s, random.Next(1, 6), Cities[index],
                            random.Next(0, 2) == 1 ? true : false,
                            random.Next(0, 2) == 1 ? true : false);
                        Hotels.Add(hotel);
                        s = sr.ReadLine();
                    }
                }

                // Create Offer objects
                for (int i = 0; i < 500; i++)
                {
                    Random random = new Random();
                    // Select a random hotel and departure city
                    var indexHotel = random.Next(Hotels.Count);
                    var indexCity = random.Next(Cities.Count);

                    var offer = new Offer(Hotels[indexHotel], random.Next(1000, 3000), DateTime.Now.AddDays(10), DateTime.Now.AddDays(17),
                                                Cities[indexCity], true);
                    context.Offer.Add(offer);
                }

                // Save all changes to the database
                context.SaveChanges();
            }
        }
    }
}

