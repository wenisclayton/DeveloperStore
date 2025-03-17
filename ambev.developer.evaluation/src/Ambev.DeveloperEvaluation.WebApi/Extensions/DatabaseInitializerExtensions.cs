using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.SaleAggregate;

namespace Ambev.DeveloperEvaluation.WebApi.Extensions;

public static class DatabaseInitializerExtensions
{
    public static IApplicationBuilder MigrateAndSeedDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var runMigrations = configuration.GetValue<bool>("RunDatabaseMigration");

        if (runMigrations is false) return app;
        
        var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        context.Database.Migrate();
        SeedData(context, logger);

        return app;
    }

    private static void SeedData(DefaultContext context, ILogger<Program> logger)
    {
        if (context.Set<Branch>().Any() is false)
        {
            context.Set<Branch>().AddRange(new List<Branch>
                {
                    new(name: "Central"),
                    new(name: "Regional - Goiânia")
                });
        }

        // Exemplo de semeadura para Customers
        if (context.Set<Customer>().Any() is false)
        {
            context.Set<Customer>().AddRange(new List<Customer>
                {
                    new(name: "Uncle Bob"),
                    new(name: "Barbara Liskov")
                });
        }

        // Exemplo de semeadura para Products
        if (context.Set<Product>().Any() is false)
        {
            context.Set<Product>().AddRange(new List<Product>
                {
                    new(
                        name :"Clean Architecture ",
                        unitPrice : 65.80m
                    ),
                    new 
                    (
                        name : "Domain-Driven Design",
                        unitPrice : 95.90m
                    ),
                new
                    (
                        name : "O Programador Pragmático",
                        unitPrice : 149.63m
                    )
                });
        }

        
        if (context.Users.Any() is false)
        {
            var password = "$2a$11$lPlJ8hWmQ/bFbvf2p3TWwefO48EewpAkuzgUdBm2U8asIOYTSRIU2"; //"123@Senha"
            context.Users.AddRange(new List<User>
                {
                    new()
                    {
                        Username = "Admin",
                        Email = "admin@exemplo.com",
                        Phone = "123456789",
                        Password = password,
                        Role = UserRole.Admin,
                        Status = UserStatus.Active,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new()
                    {
                        Username = "Customer",
                        Email = "usuario@exemplo.com",
                        Phone = "987654321",
                        Password = password,
                        Role = UserRole.Customer,
                        Status = UserStatus.Active,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                });
        }

        // Salva as alterações
        context.SaveChanges();

        LogGenerationSavedData(context, logger);
    }

    private static void LogGenerationSavedData(DefaultContext context, ILogger<Program> logger)
    {
        logger.LogInformation("IDs das Branches:");
        foreach (var branch in context.Set<Branch>())
        {
            logger.LogInformation($"Nome: {branch.Name} - ID: {branch.Id}");
        }

        Console.WriteLine("IDs dos Customers:");
        foreach (var customer in context.Set<Customer>())
        {
            logger.LogInformation($"Nome: {customer.Name} - ID: {customer.Id}");
        }

        Console.WriteLine("IDs dos Products:");
        foreach (var product in context.Set<Product>())
        {
            logger.LogInformation($"Nome: {product.Name} - ID: {product.Id}");
        }

        Console.WriteLine("IDs dos Users:");
        foreach (var user in context.Users)
        {
            logger.LogInformation($"Username: {user.Username} - ID: {user.Id}");
        }
    }
}