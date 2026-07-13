using System.Text.Json;
using System.Text.Json.Serialization;

namespace UtilityPaymentJournal.Infrastructure.JsonConverters
{
    /// <summary>
    /// Глобальный конвертер дат для JSON-сериализатора (System.Text.Json) на границе API и Клиента.
    /// 
    /// ПОЧЕМУ ЭТО НУЖНО:
    /// При заборе данных из PostgreSQL (колонки timestamptz), .NET Core может сбрасывать 
    /// флаг DateTimeKind.Utc в Unspecified. Из-за этого стандартный сериализатор стирает 
    /// символ 'Z' из JSON-строки, вынуждая фронтенд дважды вычитать часовой пояс (время скачет назад).
    /// 
    /// ЧТО ДЕЛАЕТ:
    /// Данный конвертер принудительно штампует символ 'Z' на конце каждой даты, отправляемой клиенту,
    /// гарантируя сквозную передачу времени по международному стандарту ISO 8601 UTC.
    /// </summary>
    public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Чтение данных: Срабатывает, когда фронтенд присылает JSON с датой на бэкенд.
        /// </summary>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? dateStr = reader.GetString();

            // Проверяем на пустую строку или null (если дата не была передана)
            if (string.IsNullOrEmpty(dateStr))
            {
                return DateTime.MinValue; // Возвращаем дефолтную дату "01.01.0001"
            }

            // Пытаемся разобрать строку в объект DateTime
            if (DateTime.TryParse(dateStr, out var parsedDate))
            {
                // Если парсинг успешен, принудительно возвращаем время в формате UTC
                return parsedDate.ToUniversalTime();
            }

            // Если прислали не DateTime, логируем предупреждение и не роняем бэкенд.
            // В базу данных запишется дефолтное минимальное значение, либо сработает валидатор модели
            //Console.warn($"[JsonConverter]: Не удалось распарсить строку '{dateStr}' в формат DateTime.");
            return DateTime.MinValue;
        }

        /// <summary>
        /// Запись данных: Срабатывает, когда бэкенд отправляет JSON с датой на фронтенд.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // БРОНИРОВАННАЯ ЗАЩИТА: Если дата вдруг имеет тип Local или Unspecified, 
            // метод .ToUniversalTime() корректно переведет стрелки часов в UTC.
            // Если дата уже была в UTC, метод просто вернет её без изменений.
            DateTime utcValue = value.ToUniversalTime();

            //Console.WriteLine($"[DEBUG BACKEND] CreatedAt улетает в JSON как: {utcValue:yyyy-MM-ddTHH:mm:ss.fffZ}");

            // Записываем итоговую ISO-строку с гарантированным суффиксом 'Z' на конце
            writer.WriteStringValue(utcValue);
        }
    }
}
