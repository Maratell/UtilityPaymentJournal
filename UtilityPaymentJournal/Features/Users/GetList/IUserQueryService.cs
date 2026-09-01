namespace UtilityPaymentJournal.Features.Users.GetList
{
    /// <summary>
    /// Специализированный сервис чтения (Read-Model / Query Service) для получения писка пользователей.
    /// 
    /// ЗАЧЕМ ОН НУЖЕН:
    /// 1. Позволяет писать кастомные производительные LINQ-запросы с LEFT JOIN и GroupBy на стороне СУБД.
    /// 2. Защищает интерфейс 'IApplicationDbContext' от загрязнения системными таблицами Identity (Users, Roles).
    /// 3. Полностью изолирует тяжелую инфраструктуру базы данных, позволяя писать быстрые и легковесные 
    ///    Unit-тесты для хэндлеров без использования Docker и реального подключения к PostgreSQL.
    /// </summary>
    public interface IUserQueryService
    {
        /// <summary>
        /// Высокоэффективное получение списка всех пользователей системы вместе с их ролями.
        /// Выполняет один объединенный SQL-запрос с группировкой на стороне СУБД, полностью 
        /// исключая проблему N+1 и декартово произведение строк.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции со стороны клиента.</param>
        /// <returns>Коллекция легковесных моделей отображения пользователей для списка.</returns>
        Task<IReadOnlyCollection<GetUsersListResponse.Item>> GetUsersListWithRolesAsync(CancellationToken cancellationToken);
    }
}
