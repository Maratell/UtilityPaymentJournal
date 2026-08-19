using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentJournal.Tests.Integration
{
    /// <summary>
    /// Класс-маркер (Test Collection), управляющий общим жизненным циклом и синхронизацией тестов.
    /// Этот класс физически не выполняет код, он служит исключительно конфигурационным контрактом для xUnit.
    /// </summary>
    [CollectionDefinition("Integration Tests Collection", DisableParallelization = true)]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
    {
        // Класс намеренно оставляется пустым.
        // 
        // ЗАЧЕМ ЭТО НУЖНО?
        //
        // 1. ОТКЛЮЧЕНИЕ ПАРАЛЛЕЛИЗМА (DisableParallelization = true):
        //    По умолчанию xUnit запускает разные тестовые классы одновременно в несколько потоков.
        //    В интеграционных тестах все они пишут в ОДНУ общую базу данных в Docker. 
        //    Если бы они работали параллельно, Тест А удалял бы данные, которые прямо сейчас пытается 
        //    проверить Тест Б. Это привело бы к хаосу и ложным падениям. Данный атрибут заставляет 
        //    тесты выполняться строго по очереди (последовательно).
        //
        // 2. ОДИН КОНТЕЙНЕР НА ВСЕ ТЕСТЫ (ICollectionFixture<>):
        //    Интерфейс сообщает фреймворку, что экземпляр нашей фабрики IntegrationTestWebAppFactory 
        //    должен быть создан ОДИН раз на всю сессию. xUnit поднимет Docker-контейнер перед самым первым 
        //    тестом и уничтожит его только тогда, когда завершится самый последний тест во всем проекте.
    }

    //[CollectionDefinition("Integration Tests Collection", DisableParallelization = true)] // ВАЖНО: Отключаем параллельный запуск внутри этой коллекции
    //public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
    //{
    //    // Этот класс не содержит кода.
    //    // Он нужен только для объединения тестов в одну группу (Collection).
    //}
}
