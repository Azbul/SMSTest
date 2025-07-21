namespace Part3;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Part2.Services;
using Part3.Data.Repositories;
using Part3.Logging;
using Part3.Services;
using Sms.Test;

internal class Program
{
    // По хорошему нужно получить с di, но для тестового думаю не важно
    private static readonly ILogger _logger = Logger.CreateLogger<Program>();
    private static readonly ItemsIOService _itemsIOService = new(_logger);

    static async Task Main(string[] args)
    {
        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var cancellationToken = GetCancellationToken();

            var menuItemsRepository = GetMenuItemsRepository(config);
            var cashierService = GetCashierService(config);

            var menuItems = await cashierService.GetMenuItemsAsync(withPrice: true, cancellationToken);
            var menuItemsList = menuItems.ToList();

            await menuItemsRepository.AddNewMenuItemsAsync(menuItemsList);
            _itemsIOService.DisplayMenuItems(menuItemsList);

            var orderItems = _itemsIOService.ReadNewOrderItems();
            var order = new Order() { Id = Guid.NewGuid().ToString() };
            order.OrderItems.AddRange(orderItems);

            await cashierService.SendOrderAsync(order, cancellationToken);

            _logger.LogInformation("УСПЕХ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"При выполнении программы произошла ошибка: {ex.Message}");
        }

    }

    private static CashierService GetCashierService(IConfigurationRoot config)
    {
        var grpcUrl = config["Grpc:ServerUrl"];

        if (string.IsNullOrEmpty(grpcUrl))
            throw new ArgumentException("Не заданы Grpc.ServerUrl в appsettings.json");

        return new CashierService(grpcUrl);
    }

    private static MenuItemsRepository GetMenuItemsRepository(IConfigurationRoot config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("Не заданы параметры подключения к базе данных в appsettings.json");

        return new MenuItemsRepository(connectionString);
    }

    private static CancellationToken GetCancellationToken()
    {
        var cts = new CancellationTokenSource();

        // Ctrl + C
        Console.CancelKeyPress += (s, e) =>
        {
            _logger.LogInformation("Отмена...");
            cts.Cancel();
            e.Cancel = true;
        };

        return cts.Token;
    }
}