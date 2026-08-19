using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentJournal.Tests.Integration
{
    [CollectionDefinition("Integration Tests Collection", DisableParallelization = true)] // ВАЖНО: Отключаем параллельный запуск внутри этой коллекции
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
    {
        // Этот класс не содержит кода.
        // Он нужен только для объединения тестов в одну группу (Collection).
    }
}
