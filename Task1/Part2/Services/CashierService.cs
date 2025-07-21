namespace Part2.Services;

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Sms.Test;
using System.Collections.Generic;

/// <summary>
/// Сервис для работы с кассой.
/// </summary>
public class CashierService
{
    private readonly SmsTestService.SmsTestServiceClient _client;

    public CashierService(string serverUrl)
    {
        var channel = GrpcChannel.ForAddress(serverUrl);
        _client = new SmsTestService.SmsTestServiceClient(channel);
    }

    /// <summary>
    /// Возвращает список блюд.
    /// </summary>
    public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync(
        bool withPrice, 
        CancellationToken cancellationToken)
    {
        var request = new BoolValue { Value = withPrice };
        var response = await _client
            .GetMenuAsync(request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!response.Success)
        {
            throw new RpcException(
                new Status(StatusCode.Unknown, response.ErrorMessage));
        }

        return response.MenuItems;
    }

    /// <summary>
    /// Отправляет заказ на сервер.
    /// </summary>
    public async Task SendOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var response = await _client
            .SendOrderAsync(order, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!response.Success)
        {
            throw new RpcException(
                new Status(StatusCode.Unknown, response.ErrorMessage));
        }
    }
}