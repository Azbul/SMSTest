namespace Part1.Services;

using Part1.Models;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

/// <summary>
/// Сервис для работы с кассой.
/// </summary>
public class CashierService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    public CashierService(string baseUrl, string username, string password)
    {
        _httpClient = new() { BaseAddress = new(baseUrl) };
        SetBasicAuthHeader(username, password);
    }

    /// <summary>
    /// Возвращает список блюд.
    /// </summary>
    /// <param name="withPrice">Признак включения цен в результат.</param>
    public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync(
        bool withPrice,
        CancellationToken cancellationToken)
    {
        var request = new
        {
            Command = "GetMenu",
            CommandParameters = new { WithPrice = withPrice }
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient
            .PostAsync(string.Empty, content, cancellationToken)
            .ConfigureAwait(false);
        
        var responseBody = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        var menuItemsResult = JsonSerializer.Deserialize<MenuItemsResult>(responseBody, _jsonOptions) 
            ?? throw new Exception("Не удалось прочитать ответ от сервера");
        
        if (!menuItemsResult.Success)
            throw new Exception($"Ошибка получения списка блюд: {menuItemsResult.ErrorMessage}");

        return menuItemsResult.Data.MenuItems;
    }

    /// <summary>
    /// Отправляет заказ на сервер.
    /// </summary>
    public async Task SendOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var request = new
        {
            Command = "SendOrder",
            CommandParameters = new { order.OrderId, order.MenuItems }
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient
            .PostAsync(string.Empty, content, cancellationToken)
            .ConfigureAwait(false);

        var responseBody = await response.Content
            .ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);

        var serverResult = JsonSerializer.Deserialize<ServerResult>(responseBody, _jsonOptions)
            ?? throw new Exception("Не удалось прочитать ответ от сервера");

        if (!serverResult.Success)
            throw new Exception($"Ошибка отправки заказа: {serverResult.ErrorMessage}");
    }

    private void SetBasicAuthHeader(string username, string password)
    {
        var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
    }
}