using UtilityPaymentJournal.Features.UtilityProviders.Commands;
using UtilityPaymentJournal.Features.UtilityProviders.Models;
using UtilityPaymentJournal.Features.UtilityProviders.Queries;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders
{
    /// <summary>
    /// Интерфейс маппера для преобразования моделей данных поставщика коммунальных услуг между слоями.
    /// </summary>
    public interface IUtilityProviderMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        CreateUtilityProviderDto ToDto(CreateUtilityProviderViewModel createViewModel);
        /// <summary>
        /// Преобразовать входящую модель редактирования во входной ДТО бизнес-логики.
        /// </summary>
        EditUtilityProviderDto ToDto(EditUtilityProviderViewModel editViewModel);
        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        UtilityProvider ToEntity(CreateUtilityProviderDto createDto);
        /// <summary>
        /// Обновить существующую доменную сущность на основе ДТО редактирования.
        /// </summary>
        void UpdateEntity(EditUtilityProviderDto editDto, UtilityProvider entity);
        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        UtilityProviderCommandResultDto ToCommandResultDto(UtilityProvider entity);
        /// <summary>
        /// Преобразовать сущность в ДТО результата запроса чтения.
        /// </summary>
        UtilityProviderQueryResultDto ToQueryResultDto(UtilityProvider entity);
        /// <summary>
        /// Преобразовать плоский ДТО записи в модель ответа API создания (для POST).
        /// </summary>
        UtilityProviderCreatedViewModel ToCreatedViewModel(UtilityProviderCommandResultDto dto);
        /// <summary>
        /// Преобразовать плоский ДТО записи в модель ответа API обновления (для PUT).
        /// </summary>
        UtilityProviderUpdatedViewModel ToUpdatedViewModel(UtilityProviderCommandResultDto dto);
        /// <summary>
        /// Преобразовать ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        UtilityProviderDetailsViewModel ToViewModel(UtilityProviderQueryResultDto dto);
    }
}
