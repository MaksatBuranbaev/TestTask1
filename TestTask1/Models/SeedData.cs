using Microsoft.EntityFrameworkCore;
using TestTask1.Data;

namespace TestTask1.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new DoctorsPatientsDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<DoctorsPatientsDbContext>>()))
        {

            if (context.Districts.Any() && context.Offices.Any() && context.Specializations.Any())
            {
                return;
            }

            context.Districts.AddRange(
                new District
                {
                    Number = "1"
                },
                new District
                {
                    Number = "2"
                },
                new District
                {
                    Number = "3"
                }
            );

            context.Offices.AddRange(
                new Office
                {
                    Number = "1"
                },
                new Office
                {
                    Number = "2"
                },
                new Office
                {
                    Number = "3"
                }
            );

            context.Specializations.AddRange(
                new Specialization
                {
                    Name = "1"
                },
                new Specialization
                {
                    Name = "2"
                },
                new Specialization
                {
                    Name = "3"
                }
            );

            context.SaveChanges();
        }
    }
}