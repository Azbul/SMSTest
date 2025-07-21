using Part3.Extensions;
using Sms.Test;

namespace Part3.Data.Repositories;

/// <summary>
/// Репозиторий для блюд.
/// </summary>
internal sealed class MenuItemsRepository(string connectionString)
{
    /// <summary>
    /// Добавляет новые блюда в БД.
    /// </summary>
    /// <param name="menuItems">Список блюд из сервера.</param>
    public async Task AddNewMenuItemsAsync(List<MenuItem> menuItems)
    {
        using var dbContext = new ApplicationDbContext(connectionString);
        var externalIds = dbContext.MenuItems
            .Select(item => item.ExternalId)
            .ToHashSet();

        await dbContext.MenuItems.AddRangeAsync(
            menuItems
                .Where(item => !externalIds.Contains(item.Id))
                .Select(item => item.ToDbEntity()));

        await dbContext.SaveChangesAsync();
    }
}
