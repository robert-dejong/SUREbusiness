using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SUREbusiness.FleetManagement.DAL.Entities;

namespace SUREbusiness.FleetManagement.DAL
{
    public static class DatabaseExtensions
    {
        private static readonly int VehiclesToGenerate = 100;

        private static readonly string[] Colors = { "Groen", "Rood", "Blauw" };
        private static readonly string[] Statuses = { "uitgeleend", "beschikbaar", "in reparatie", "verkocht", "in bestelling" };
        private static readonly string[] LoanedToNames = { "Robert", "Peter", "Jan" };
        private static readonly string[] LicensePlates = { "34TDK2", "39XXFZ", "36JJB5", "75PJF8", "92NXD2" };

        public static IHost ConfigureDefaultDataSet(this IHost host)
        {
            var scope = host.Services.CreateScope();
            var vehiclesDbContext = scope.ServiceProvider.GetRequiredService<VehiclesDbContext>();

            var faker = new Faker<VehicleEntity>()
                .RuleFor(entity => entity.LicensePlate, faker => faker.PickRandom(LicensePlates))
                .RuleFor(entity => entity.Color, faker => faker.PickRandom(Colors))
                .RuleFor(entity => entity.Remarks, faker => faker.Lorem.Sentence(1))
                .RuleFor(entity => entity.Status, faker => faker.PickRandom(Statuses))
                .RuleFor(entity => entity.LoanedTo, (faker, entity) => GetLoanedTo(faker, entity.Status))
                .RuleFor(entity => entity.YearOfManufacture, faker => faker.Random.Number(1990, 2021));

            var vehicles = faker.Generate(VehiclesToGenerate);

            vehicles.ForEach(entity => vehiclesDbContext.Add(entity));

            vehiclesDbContext.SaveChanges();

            return host;
        }

        private static string GetLoanedTo(Faker faker, string status)
        {
            return status == "uitgeleend" ? faker.PickRandom(LoanedToNames) : string.Empty;
        }
    }
}
