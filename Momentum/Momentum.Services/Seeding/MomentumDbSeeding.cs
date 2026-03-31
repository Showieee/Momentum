using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Momentum.Domain.Entities;
using Momentum.Domain.Enums;
using Momentum.Persistance;

namespace Momentum.Services.Seeding;

public class MomentumDbSeeding
{
    public static async Task Run(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<MomentumDbContext>();

        // Only seed if the database is empty
        if (await context.Companies.AnyAsync() || 
            await context.Products.AnyAsync() || 
            await context.Persons.AnyAsync() ||
            await context.Events.AnyAsync())
        {
            return;
        }

        await SeedAddresses(context);
        await SeedCompanies(context);
        await SeedProducts(context);
        await SeedPersons(context);
        await SeedEvents(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedAddresses(MomentumDbContext context)
    {
        var addresses = new List<AddressEntity>
        {
            new()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                StreetName = "Calea Victoriei",
                StreetNumber = "123",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "contact@elegancephoto.ro",
                PhoneNumber = "0721123456"
            },
            new()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                StreetName = "Strada Doamnei",
                StreetNumber = "45",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "info@sweettreats.ro",
                PhoneNumber = "0722234567"
            },
            new()
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                StreetName = "Piata Amzei",
                StreetNumber = "78",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "events@rhythmbeats.ro",
                PhoneNumber = "0723345678"
            },
            new()
            {
                Id = new Guid("44444444-4444-4444-4444-444444444444"),
                StreetName = "Bulevardul Unirii",
                StreetNumber = "200",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "booking@premium-dj.ro",
                PhoneNumber = "0724456789"
            },
            new()
            {
                Id = new Guid("55555555-5555-5555-5555-555555555555"),
                StreetName = "Strada Republicii",
                StreetNumber = "56",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "info@danceshowtroupe.ro",
                PhoneNumber = "0725567890"
            },
            new()
            {
                Id = new Guid("66666666-6666-6666-6666-666666666666"),
                StreetName = "Calea Floreasca",
                StreetNumber = "88",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "contact@grand-venue.ro",
                PhoneNumber = "0726678901"
            },
            new()
            {
                Id = new Guid("77777777-7777-7777-7777-777777777777"),
                StreetName = "Strada Matei Voievod",
                StreetNumber = "34",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "hello@gourmetcatering.ro",
                PhoneNumber = "0727789012"
            },
            new()
            {
                Id = new Guid("88888888-8888-8888-8888-888888888888"),
                StreetName = "Bulevardul Magheru",
                StreetNumber = "120",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "info@floraldesigns.ro",
                PhoneNumber = "0728890123"
            }
        };

        await context.Addresses.AddRangeAsync(addresses);
    }

    private static async Task SeedCompanies(MomentumDbContext context)
    {
        var companies = new List<CompanyEntity>
        {
            new()
            {
                Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name = "Elegance Photography Studio",
                CUI = "RO12345001",
                AddressId = new Guid("11111111-1111-1111-1111-111111111111")
            },
            new()
            {
                Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Name = "Sweet Treats Candy Bar",
                CUI = "RO12345002",
                AddressId = new Guid("22222222-2222-2222-2222-222222222222")
            },
            new()
            {
                Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Name = "Rhythm & Beats Entertainment",
                CUI = "RO12345003",
                AddressId = new Guid("33333333-3333-3333-3333-333333333333")
            },
            new()
            {
                Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Name = "Premium DJ Events",
                CUI = "RO12345004",
                AddressId = new Guid("44444444-4444-4444-4444-444444444444")
            },
            new()
            {
                Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Name = "Dance Show Troupe",
                CUI = "RO12345005",
                AddressId = new Guid("55555555-5555-5555-5555-555555555555")
            },
            new()
            {
                Id = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Name = "Grand Venue Locations",
                CUI = "RO12345006",
                AddressId = new Guid("66666666-6666-6666-6666-666666666666")
            },
            new()
            {
                Id = new Guid("10101010-1010-1010-1010-101010101010"),
                Name = "Gourmet Catering Co.",
                CUI = "RO12345007",
                AddressId = new Guid("77777777-7777-7777-7777-777777777777")
            },
            new()
            {
                Id = new Guid("20202020-2020-2020-2020-202020202020"),
                Name = "Floral Designs & Decorations",
                CUI = "RO12345008",
                AddressId = new Guid("88888888-8888-8888-8888-888888888888")
            }
        };

        await context.Companies.AddRangeAsync(companies);
    }

    private static async Task SeedProducts(MomentumDbContext context)
    {
        var products = new List<ProductEntity>
        {
            // Photography Products
            new()
            {
                Id = new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                Name = "Professional Photography Package - 8 Hours",
                Type = ProductType.Photography,
                Price = 2500,
                Description = "Full event coverage with 2 professional photographers, edited photos delivered within 2 weeks",
                CompanyId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Company = null!
            },
            new()
            {
                Id = new Guid("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2"),
                Name = "Premium Video Recording - 4K",
                Type = ProductType.Photography,
                Price = 3500,
                Description = "Professional 4K video coverage with drone footage, cinematic editing, and same-day highlight reel",
                CompanyId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Company = null!
            },
            new()
            {
                Id = new Guid("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3"),
                Name = "Photo Booth Rental - 4 Hours",
                Type = ProductType.Photography,
                Price = 800,
                Description = "Interactive photo booth with props, digital and printed copies for guests",
                CompanyId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Company = null!
            },

            // Food & Candy Bar Products
            new()
            {
                Id = new Guid("b1b1b1b1-b1b1-b1b1-b1b1-b1b1b1b1b1b1"),
                Name = "Sweet Candy Bar Station - 50 Guests",
                Type = ProductType.Food,
                Price = 1200,
                Description = "Unlimited candy, chocolate, and desserts with elegant displays and attendant",
                CompanyId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Company = null!
            },
            new()
            {
                Id = new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                Name = "Gourmet Donut Wall - 100 Pieces",
                Type = ProductType.Food,
                Price = 600,
                Description = "Custom decorated wall with assorted gourmet donuts and pastries",
                CompanyId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Company = null!
            },
            new()
            {
                Id = new Guid("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3"),
                Name = "Chocolate Fountain Station - 2 Hours",
                Type = ProductType.Food,
                Price = 500,
                Description = "Luxury chocolate fountain with fresh fruits, marshmallows, and pastries",
                CompanyId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Company = null!
            },

            // Music & DJ Products
            new()
            {
                Id = new Guid("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1"),
                Name = "Live Band - 4 Hours",
                Type = ProductType.Music,
                Price = 4000,
                Description = "Professional live band with vocalist, capable of playing multiple genres",
                CompanyId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Company = null!
            },
            new()
            {
                Id = new Guid("c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2"),
                Name = "Live Jazz Quartet",
                Type = ProductType.Music,
                Price = 2500,
                Description = "Sophisticated jazz quartet for cocktail hour and dinner",
                CompanyId = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Company = null!
            },
            new()
            {
                Id = new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                Name = "Premium DJ Service - 6 Hours",
                Type = ProductType.Music,
                Price = 2000,
                Description = "Professional DJ with premium sound system, lighting effects, and MC services",
                CompanyId = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Company = null!
            },
            new()
            {
                Id = new Guid("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4"),
                Name = "DJ with Wedding Ceremony Sound",
                Type = ProductType.Music,
                Price = 1500,
                Description = "DJ providing audio equipment and management for ceremony and reception",
                CompanyId = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Company = null!
            },

            // Videography/Entertainment Products
            new()
            {
                Id = new Guid("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1"),
                Name = "Professional Dance Show - 30 Minutes",
                Type = ProductType.Videography,
                Price = 2000,
                Description = "Energetic dance performance by professional troupe to entertain guests",
                CompanyId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Company = null!
            },
            new()
            {
                Id = new Guid("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2"),
                Name = "Surprise Dance Performance - Flash Mob Style",
                Type = ProductType.Videography,
                Price = 1500,
                Description = "Surprise flash mob dance performance featuring event attendees or professionals",
                CompanyId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Company = null!
            },

            // Location Products
            new()
            {
                Id = new Guid("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1"),
                Name = "Grand Ballroom - Full Day",
                Type = ProductType.Location,
                Price = 5000,
                Description = "Elegant ballroom with capacity for 500 guests, includes tables, chairs, and basic lighting",
                CompanyId = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Company = null!
            },
            new()
            {
                Id = new Guid("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2"),
                Name = "Garden Venue - Half Day",
                Type = ProductType.Location,
                Price = 3000,
                Description = "Beautiful outdoor garden venue with covered pavilion, capacity for 300 guests",
                CompanyId = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Company = null!
            },
            new()
            {
                Id = new Guid("e3e3e3e3-e3e3-e3e3-e3e3-e3e3e3e3e3e3"),
                Name = "Intimate Gallery Space - 6 Hours",
                Type = ProductType.Location,
                Price = 2000,
                Description = "Modern gallery space perfect for cocktail receptions and exhibitions, up to 200 guests",
                CompanyId = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Company = null!
            },

            // Catering Products
            new()
            {
                Id = new Guid("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"),
                Name = "Full Course Dinner - Per Person",
                Type = ProductType.Food,
                Price = 150,
                Description = "Multi-course gourmet dinner with appetizers, main course, and dessert",
                CompanyId = new Guid("10101010-1010-1010-1010-101010101010"),
                Company = null!
            },
            new()
            {
                Id = new Guid("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"),
                Name = "Cocktail Hour Appetizers - Per Person",
                Type = ProductType.Food,
                Price = 45,
                Description = "Selection of 8-10 hot and cold appetizers for pre-dinner reception",
                CompanyId = new Guid("10101010-1010-1010-1010-101010101010"),
                Company = null!
            },
            new()
            {
                Id = new Guid("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3"),
                Name = "Premium Bar Package - Per Person",
                Type = ProductType.Food,
                Price = 60,
                Description = "Open bar with premium spirits, wines, beers, and soft drinks",
                CompanyId = new Guid("10101010-1010-1010-1010-101010101010"),
                Company = null!
            },
            new()
            {
                Id = new Guid("f4f4f4f4-f4f4-f4f4-f4f4-f4f4f4f4f4f4"),
                Name = "Breakfast Buffet - Per Person",
                Type = ProductType.Food,
                Price = 50,
                Description = "Complete breakfast buffet including hot and cold items, beverages included",
                CompanyId = new Guid("10101010-1010-1010-1010-101010101010"),
                Company = null!
            },

            // Decoration Products
            new()
            {
                Id = new Guid("f5f5f5f5-f5f5-f5f5-f5f5-f5f5f5f5f5f5"),
                Name = "Full Event Floral Arrangements",
                Type = ProductType.Decoration,
                Price = 3500,
                Description = "Complete floral designs including bridal bouquet, centerpieces, arch arrangements, and entry decorations",
                CompanyId = new Guid("20202020-2020-2020-2020-202020202020"),
                Company = null!
            },
            new()
            {
                Id = new Guid("f6f6f6f6-f6f6-f6f6-f6f6-f6f6f6f6f6f6"),
                Name = "Table Centerpiece Package - 20 Tables",
                Type = ProductType.Decoration,
                Price = 1200,
                Description = "Elegant floral centerpieces for dining tables, includes flowers and elegant containers",
                CompanyId = new Guid("20202020-2020-2020-2020-202020202020"),
                Company = null!
            },
            new()
            {
                Id = new Guid("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7"),
                Name = "Bridal Bouquet & Boutonniere Set",
                Type = ProductType.Decoration,
                Price = 400,
                Description = "Custom designed bridal bouquet and matching boutonnieres for wedding party",
                CompanyId = new Guid("20202020-2020-2020-2020-202020202020"),
                Company = null!
            }
        };

        await context.Products.AddRangeAsync(products);
    }

    private static async Task SeedPersons(MomentumDbContext context)
    {
        var personAddresses = new List<AddressEntity>
        {
            new()
            {
                Id = new Guid("aa111111-1111-1111-1111-111111111111"),
                StreetName = "Strada Lapusneanu",
                StreetNumber = "15",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "maria.popescu@email.com",
                PhoneNumber = "0741111111"
            },
            new()
            {
                Id = new Guid("aa222222-2222-2222-2222-222222222222"),
                StreetName = "Bulevardul Dacia",
                StreetNumber = "32",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "ion.georgescu@email.com",
                PhoneNumber = "0742222222"
            },
            new()
            {
                Id = new Guid("aa333333-3333-3333-3333-333333333333"),
                StreetName = "Strada Stirbei Voda",
                StreetNumber = "44",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "alexandra.ivan@email.com",
                PhoneNumber = "0743333333"
            },
            new()
            {
                Id = new Guid("aa444444-4444-4444-4444-444444444444"),
                StreetName = "Calea Plevnei",
                StreetNumber = "88",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "mihai.andrei@email.com",
                PhoneNumber = "0744444444"
            },
            new()
            {
                Id = new Guid("aa555555-5555-5555-5555-555555555555"),
                StreetName = "Strada Vulturilor",
                StreetNumber = "22",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "laura.mihai@email.com",
                PhoneNumber = "0745555555"
            },
            new()
            {
                Id = new Guid("aa666666-6666-6666-6666-666666666666"),
                StreetName = "Bulevardul Bratianu",
                StreetNumber = "100",
                City = "Bucharest",
                State = "Bucharest",
                Country = "Romania",
                Email = "robert.stoian@email.com",
                PhoneNumber = "0746666666"
            }
        };

        await context.Addresses.AddRangeAsync(personAddresses);

        var persons = new List<PersonEntity>
        {
            new()
            {
                Id = new Guid("11111111-2222-3333-4444-555555555551"),
                FirstName = "Maria",
                LastName = "Popescu",
                CNP = "1900315123456",
                AddressId = new Guid("aa111111-1111-1111-1111-111111111111")
            },
            new()
            {
                Id = new Guid("11111111-2222-3333-4444-555555555552"),
                FirstName = "Ion",
                LastName = "Georgescu",
                CNP = "1920428654321",
                AddressId = new Guid("aa222222-2222-2222-2222-222222222222")
            },
            new()
            {
                Id = new Guid("11111111-2222-3333-4444-555555555553"),
                FirstName = "Alexandra",
                LastName = "Ivan",
                CNP = "2950512789012",
                AddressId = new Guid("aa333333-3333-3333-3333-333333333333")
            },
            new()
            {
                Id = new Guid("11111111-2222-3333-4444-555555555554"),
                FirstName = "Mihai",
                LastName = "Andrei",
                CNP = "1850723345678",
                AddressId = new Guid("aa444444-4444-4444-4444-444444444444")
            },
            new()
            {
                Id = new Guid("11111111-2222-3333-4444-555555555555"),
                FirstName = "Laura",
                LastName = "Mihai",
                CNP = "2910601901234",
                AddressId = new Guid("aa555555-5555-5555-5555-555555555555")
            },
            new()
            {
                Id = new Guid("11111111-2222-3333-4444-555555555556"),
                FirstName = "Robert",
                LastName = "Stoian",
                CNP = "1880415567890",
                AddressId = new Guid("aa666666-6666-6666-6666-666666666666")
            }
        };

        await context.Persons.AddRangeAsync(persons);
    }

    private static async Task SeedEvents(MomentumDbContext context)
    {
        var events = new List<EventEntity>
        {
            new()
            {
                Id = new Guid("ee111111-1111-1111-1111-111111111111"),
                Type = EventType.Wedding,
                Name = "Classic Spring Wedding",
                Date = "2024-06-15"
            },
            new()
            {
                Id = new Guid("ee222222-2222-2222-2222-222222222222"),
                Type = EventType.Wedding,
                Name = "Modern Elegant Wedding",
                Date = "2024-07-20"
            },
            new()
            {
                Id = new Guid("ee333333-3333-3333-3333-333333333333"),
                Type = EventType.Birthday,
                Name = "30th Birthday Celebration",
                Date = "2024-08-10"
            },
            new()
            {
                Id = new Guid("ee444444-4444-4444-4444-444444444444"),
                Type = EventType.Birthday,
                Name = "Sweet 18 Party",
                Date = "2024-09-05"
            },
            new()
            {
                Id = new Guid("ee555555-5555-5555-5555-555555555555"),
                Type = EventType.Teambuilding,
                Name = "Company Summer Teambuilding",
                Date = "2024-08-25"
            },
            new()
            {
                Id = new Guid("ee666666-6666-6666-6666-666666666666"),
                Type = EventType.Festival,
                Name = "Summer Music Festival",
                Date = "2024-07-01"
            }
        };

        await context.Events.AddRangeAsync(events);
    }
}
