using System.Text.Json;
using System.Text.Json.Serialization;

namespace UtilityPaymentJournal.Infrastructure.JsonConverters
{
    /// <summary>
    /// Глобальный конвертер дат для JSON-сериализатора (System.Text.Json) на границе API и Клиента (для Nullable DateTime?).
    /// 
    /// ПОЧЕМУ ЭТО НУЖНО:
    /// При заборе данных из PostgreSQL (колонки timestamptz), .NET Core может сбрасывать 
    /// флаг DateTimeKind.Utc в Unspecified. Из-за этого стандартный сериализатор стирает 
    /// символ 'Z' из JSON-строки, вынуждая фронтенд дважды вычитать часовой пояс (время скачет назад).
    /// 
    /// ЧТО ДЕЛАЕТ:
    /// Данный конвертер перехватывает типы DateTime? в момент формирования JSON-ответа, безопасно обрабатывает null
    /// и принудительно штампует символ 'Z' на конце заполненных дат, гарантируя сквозную передачу времени по стандарту ISO 8601 UTC.
    /// </summary>
    public class NullableUtcDateTimeJsonConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// Чтение данных: Срабатывает, когда фронтенд присылает JSON на бэкенд.
        /// </summary>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? dateStr = reader.GetString();

            // Если с фронтенда пришла пустая строка — возвращаем честный null для базы данных,
            // а не дефолтную дату "01.01.0001", чтобы не ломать логику пустых полей.
            if (string.IsNullOrEmpty(dateStr))
            {
                return null;
            }

            // Безопасно парсим строку, защищая сервер от падения при некорректном вводе
            if (DateTime.TryParse(dateStr, out var parsedDate))
            {
                return parsedDate.ToUniversalTime();
            }

            return null;
        }

        /// <summary>
        /// Запись данных: Срабатывает, когда бэкенд отправляет JSON на фронтенд.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            // Если дата в базе данных равна null, записываем в JSON обычный null
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            // Если дата есть, принудительно приводим к UTC и штампуем символ 'Z' на конце
            DateTime utcValue = value.Value.ToUniversalTime();
            writer.WriteStringValue(utcValue);
        }
    }
}
