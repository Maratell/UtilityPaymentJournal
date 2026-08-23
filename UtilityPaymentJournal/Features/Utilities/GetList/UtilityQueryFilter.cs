namespace UtilityPaymentJournal.Features.Utilities.GetList
{
    /// <summary>
    /// Объект параметров фильтрации, сортировки и поиска коммунальных услуг, прилетающий из пользовательского интерфейса (UI).
    /// Служит плоским контейнером запроса (Request/Query Object) для передачи параметров фильтрации через строку URL ([FromQuery]).
    /// </summary>
    /// <param name="IsActive">Фильтр по статусу активности услуги (null — извлечь все записи, true — только активные, false — только неактивные).</param>
    /// <param name="SearchTerm">Поисковый маркер для фильтрации по текстовому наименованию услуги (задел на будущее для реализации живого поиска).</param>
    /// <param name="OrderBy">Критерий и направление сортировки результирующего списка, например "name_desc" (задел на будущее для таблиц интерфейса).</param>
    public record UtilityQueryFilter(
        bool? IsActive = null,
        string? SearchTerm = null,
        string? OrderBy = null
    );
}
