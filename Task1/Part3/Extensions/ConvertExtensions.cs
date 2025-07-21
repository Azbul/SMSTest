namespace Part3.Extensions;

using Sms.Test;
using MenuItemEntity = Data.Models.MenuItem;

/// <summary>
/// Расширения для конвертации.
/// </summary>
internal static class ConvertExtensions
{
    /// <summary>
    /// Преобразует <see cref="MenuItem"/> в <see cref="MenuItemEntity"/>.
    /// </summary>
    /// <param name="grpcItem"></param>
    /// <returns></returns>
    public static MenuItemEntity ToDbEntity(this MenuItem grpcItem)
    {
        return new()
        {
            ExternalId = grpcItem.Id,
            Article = grpcItem.Article,
            Name = grpcItem.Name,
            Price = grpcItem.Price
        };
    }
}
