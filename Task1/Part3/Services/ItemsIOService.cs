namespace Part3.Services;

using Microsoft.Extensions.Logging;
using Sms.Test;
using System;
using System.Collections.Generic;
using System.Globalization;

/// <summary>
/// Сервис для ввода и вывода данных элементов через консоль.
/// </summary>
/// <param name="logger"></param>
internal class ItemsIOService(ILogger logger)
{
    /// <summary>
    /// Получает список блюд в формате Код1:Кол-во1;Код2:Кол-во2...
    /// </summary>
    public List<OrderItem> ReadNewOrderItems()
    {
        var orderItems = new List<OrderItem>();
        while (true)
        {
            logger.LogInformation("Введите заказ одной строкой в формате Код1:Кол-во1;Код2:Кол-во2...");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                logger.LogInformation("Ввод не может быть пустым. Повторите ввод.");
                continue;
            }

            try
            {
                var dishes = input.Split(';');
                foreach (var dish in dishes)
                {
                    var splited = dish.Split(':');

                    var code = int.Parse(splited[0].Trim());
                    var quantity = double.Parse(splited[1].Trim(), CultureInfo.InvariantCulture);

                    if (code <= 0)
                        throw new Exception("Код не может быть меньше нулем или отрицательным числом.");

                    if (quantity <= 0)
                        throw new Exception($"Количество должно быть больше 0 для кода: {code}");


                    orderItems.Add(new OrderItem { Id = code.ToString(), Quantity = quantity });
                }
                break;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Ошибка формата ввода: {msg}.\r\nПовторите ввод.", ex.Message);
            }
        }

        return orderItems;
    }

    /// <summary>
    /// Выводит список блюд на консоль.
    /// </summary>
    public void DisplayMenuItems(List<MenuItem> menuItemsList)
    {
        logger.LogInformation("Меню:");
        foreach (var item in menuItemsList)
        {
            logger.LogInformation($"{item.Name} – {item.Article} – {item.Price} руб.");
        }
    }
}
