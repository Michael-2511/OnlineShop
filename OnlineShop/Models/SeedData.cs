using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using System.Collections.Generic;

namespace OnlineShop.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Verificam daca in baza de date exista cel putin un
                //rol
                // insemnand ca a fost rulat codul
                // De aceea facem return pentru a nu insera rolurile
                //inca o data
                // Acesta metoda trebuie sa se execute o singura data
                if (context.Roles.Any())
                {
                    return; // baza de date contine deja roluri
                }
                // CREAREA ROLURILOR IN BD
                // daca nu contine roluri, acestea se vor crea
                context.Roles.AddRange(
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "Administrator".ToUpper() },
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7211", Name = "Colaborator", NormalizedName = "Colaborator".ToUpper() },
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7212", Name = "Inregistrat", NormalizedName = "Inregistrat".ToUpper() }
                );
                // o noua instanta pe care o vom utiliza pentru
                //crearea parolelor utilizatorilor
                // parolele sunt de tip hash
                var hasher = new PasswordHasher<ApplicationUser>();
                // CREAREA USERILOR IN BD
                // Se creeaza cate un user pentru fiecare rol
                context.Users.AddRange(
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb0",
                    // primary key
                    UserName = "administrator@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "ADMINISTRATOR@TEST.COM",
                    Email = "administrator@test.com",
                    NormalizedUserName = "ADMINISTRATOR@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Administrator1!")
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb1",
                    // primary key
                    UserName = "colaborator@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "COLABORATOR@TEST.COM",
                    Email = "COLABORATOR@test.com",
                    NormalizedUserName = "COLABORATOR@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Colaborator1!")
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb2",
                    // primary key
                    UserName = "inregistrat@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "INREGISTRAT@TEST.COM",
                    Email = "inregistrat@test.com",
                    NormalizedUserName = "INREGISTRAT@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Inregistrat1!")
                },
                new ApplicationUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb3",
                    // primary key
                    UserName = "colaborator2@test.com",
                    EmailConfirmed = true,
                    NormalizedEmail = "COLABORATOR2@TEST.COM",
                    Email = "COLABORATOR2@test.com",
                    NormalizedUserName = "COLABORATOR2@TEST.COM",
                    PasswordHash = hasher.HashPassword(null, "Colaborator1!")
                }
                );
                // ASOCIEREA USER-ROLE
                context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {   // administrator
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb0"
                },
               new IdentityUserRole<string>
               {    // colaborator
                   RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                   UserId = "8e445865-a24d-4543-a6c6-9443d048cdb1"
               },
               new IdentityUserRole<string>
               {    // inregistrat
                   RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7212",
                   UserId = "8e445865-a24d-4543-a6c6-9443d048cdb2"
               },
               new IdentityUserRole<string>
               {    // colaborator
                   RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7211",
                   UserId = "8e445865-a24d-4543-a6c6-9443d048cdb3"
               }
                );
                context.SaveChanges();
            }

        }
    }
}