/**
 * Универсальное окно подтверждения
 * @param {Object} options Параметры конфигурации окна
 * @param {string} options.title Заголовок окна в шапке
 * @param {string} options.message Текст основного вопроса по центру
 * @param {string} [options.targetText] Текст выделенного объекта на пунктирной плашке (например, адрес)
 * @param {string} [options.btnText] Текст на кнопке действия (по умолчанию "Подтвердить")
 * @param {string} [options.btnClass] Тип стиля кнопки ('notebook-delete-btn-confirm' для удаления или пустая строка)
 * @param {string} [options.iconClass] Иконка Bootstrap в шапке (например, 'bi-trash3-fill')
 * @param {function} onConfirm Callback-функция, которая выполнится только при согласии пользователя
 */
function showConfirm(options, onConfirm) {
    const modalEl = document.getElementById('universalConfirmModal');
    if (!modalEl) return;

    // 1. Установка текстового контента
    $('#confirmModalLabel').text(options.title || 'Подтверждение');
    $('#confirmModalMessage').text(options.message || 'Вы уверены, что хотите выполнить это действие?');

    // 2. Настройка центрированной плашки для выделенного объекта (адреса)
    if (options.targetText) {
        $('#confirmModalTarget').text(options.targetText).show();
    } else {
        $('#confirmModalTarget').hide().text('');
    }

    // 3. Динамическая настройка кнопки подтверждения (ПРАВКИ ИЗ ШАГА 3)
    const confirmBtn = $('#btnConfirmAction');
    confirmBtn.text(options.btnText || 'Подтвердить');

    // Сбрасываем старые классы и вешаем новые пастельные стили с границами
    confirmBtn.removeClass();
    if (options.btnClass === 'notebook-delete-btn-confirm') {
        // Вешаем класс одинаковой длины и коралловую рамку для удаления
        confirmBtn.addClass('btn notebook-equal-btn notebook-confirm-btn-danger-style');
    } else {
        // Вешаем класс одинаковой длины и стандартную серо-голубую рамку
        confirmBtn.addClass('btn notebook-equal-btn notebook-confirm-btn-action');
    }

    // 4. Динамическая настройка иконки на одном уровне с текстом слева в шапке
    const iconEl = $('#confirmModalIcon');
    iconEl.removeClass();
    if (options.iconClass) {
        iconEl.addClass('bi me-2 ' + options.iconClass);
    } else {
        iconEl.addClass('bi bi-question-circle-fill notebook-confirm-icon me-2');
    }

    // 5. Безопасная привязка события клика (off() предотвращает дублирование)
    confirmBtn.off('click').on('click', function () {
        if (typeof onConfirm === 'function') {
            onConfirm();
        }
        bootstrap.Modal.getOrCreateInstance(modalEl).hide();
    });

    // 6. Инициализация и показ модального окна Bootstrap 5
    const modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);

    // Автоматический фокус на кнопку "Отмена" для защиты от случайного стирания данных
    $(modalEl).one('shown.bs.modal', function () {
        document.getElementById('btnConfirmCancel').focus();
    });

    modalInstance.show();
}

/**
 * Функция для быстрого показа предупреждения через Bootstrap Modal
 * @param {string} message - Текст ошибки или предупреждения
 */
function showAlert(message) {
    var $modalEl = $('#alertModal');

    // Проверка безопасности: если модалки нет в DOM, выводим обычный alert
    if ($modalEl.length === 0) {
        alert(message);
        return;
    }

    $('#alertModalMessage').text(message);

    // Находим или создаем инстанс модального окна Bootstrap
    const alertModal = bootstrap.Modal.getOrCreateInstance($modalEl[0]);
    alertModal.show();
}

/**
 * Пытается извлечь и показать ошибку из ответа сервера (ProblemDetails или Validation Errors).
 * @param {Object} xhr - Объект XHR от jQuery $.ajax
 * @returns {boolean} - true, если ошибка была распознана и выведена; false, если структура неизвестна
 */
function tryShowServerError(xhr) {
    if (!xhr || !xhr.responseJSON) {
        return false;
    }

    // 1. ПЕРВЫМ ДЕЛОМ проверяем специфичные ошибки валидации (объект errors)
    if (xhr.status === 400 && xhr.responseJSON.errors) {
        const errors = xhr.responseJSON.errors;
        const keys = Object.keys(errors);

        if (keys.length > 0) {
            const firstField = keys[0];
            // Проверяем, массив это или одиночная строка
            const firstError = Array.isArray(errors[firstField]) ? errors[firstField][0] : errors[firstField];

            if (firstError) {
                showAlert(firstError);
                return true;
            }
        }
    }

    // 2. ЕСЛИ ошибок валидации нет, выводим общую кастомную ошибку (объект detail)
    if (xhr.responseJSON.detail) {
        showAlert(xhr.responseJSON.detail);
        return true;
    }

    return false;
}

/* ==========================================================================
   РЕГИОН: РАБОТА С ДАТАМИ И ВРЕМЕНЕМ (МУЛЬТИРЕГИОНАЛЬНЫЙ ПОДХОД)
   --------------------------------------------------------------------------
   АРХИТЕКТУРА: Сквозной UTC на бэкенде и динамическая локализация дат на клиенте.
   БД (PostgreSQL) и сервер (ASP.NET Core) оперируют строго нулевым часовым поясом (UTC).
   Браузер (клиент) отвечает за перевод UTC в локальное время пользователя и обратно.
   ========================================================================== */

/**
 * 1. ИЗ ИНПУТА В UTC (Для отправки на бэкенд / POST, PUT запросы)
 * 
 * Назначение: Берет «сырое» локальное значение из <input type="datetime-local">,
 * интерпретирует его по часовому поясу устройства пользователя и конвертирует
 * в международный формат ISO 8601 (строку Гринвича с символом 'Z' на конце).
 * 
 * @param {string|null|undefined} inputVal - Значение свойства .val() из datetime-local инпута.
 * @returns {string|null} Строка даты в UTC для API-запроса, либо null, если дата не выбрана.
 */
function toUtcIsoString(inputVal) {
    if (!inputVal || String(inputVal).trim() === '') {
        return null;
    }

    // Нормализуем строку (заменяем пробелы на разделитель T, если необходимо)
    const normalizedInput = String(inputVal).replace(' ', 'T');
    const localDate = new Date(normalizedInput);

    // Защита от RangeError: если пользователь ввел некорректные данные, возвращаем null
    if (isNaN(localDate.getTime())) {
        console.warn(`[Dates Region]: Не удалось распознать локальную дату из значения: "${inputVal}"`);
        return null;
    }

    return localDate.toISOString(); // Результат: "YYYY-MM-DDTHH:mm:ss.sssZ"
}

/**
 * 2. ИЗ UTC В ИНПУТ (Для заполнения форм / Редактирование в модальных окнах)
 * 
 * Назначение: Принимает строку UTC от API, сдвигает её на часовой пояс текущего
 * пользователя и форматирует строго в технический вид "YYYY-MM-DDTHH:mm".
 * Без этой функции HTML-инпут <input type="datetime-local"> останется пустым.
 * 
 * @param {string|null|undefined} utcString - Строка даты в UTC от бэкенда (например, "2026-10-15T11:00:00Z")
 * @returns {string} Строка для установки в .val() инпута, либо пустая строка.
 */
function formatForDateTimeLocal(utcString) {
    if (!utcString || String(utcString).trim() === '') {
        return '';
    }

    let normalizedStr = String(utcString).trim();

    // Гарантируем, что строка парсится строго как UTC, даже если сервер стёр 'Z'
    if (!normalizedStr.endsWith('Z') && !normalizedStr.includes('+') && !normalizedStr.match(/-\d{2}:\d{2}$/)) {
        normalizedStr += 'Z';
    }

    const date = new Date(normalizedStr);

    if (isNaN(date.getTime())) {
        return '';
    }

    // Сдвигаем UTC-время на смещение часового пояса пользователя (в миллисекундах)
    const clientOffsetMs = date.getTimezoneOffset() * 60000;
    const localDate = new Date(date.getTime() - clientOffsetMs);

    // Обрезаем строку до секунд, оставляя формат "YYYY-MM-DDTHH:mm"
    return localDate.toISOString().slice(0, 16);
}

/**
 * 3. ИЗ UTC В ТЕКСТ (Для чтения человеком / Рендеринг карточек и таблиц)
 * 
 * Назначение: Принимает строку UTC от бэкенда (включая авто-даты вроде CreatedAt),
 * автоматически определяет язык браузера, текущую временную зону пользователя
 * и выводит красивый, привычный для человека текст.
 * 
 * @param {string|null|undefined} utcString - Строка даты в UTC (например, "2026-10-15T11:00:00Z")
 * @returns {string} Локализованная строка даты и времени, либо прочерк.
 */
function formatToReadableText(utcString) {
    if (!utcString || String(utcString).trim() === '') {
        return '—';
    }

    let normalizedStr = String(utcString).trim();

    // Если сервер забыл прислать 'Z' или знак смещения, дописываем 'Z' (UTC)
    if (!normalizedStr.endsWith('Z') && !normalizedStr.includes('+') && !normalizedStr.match(/-\d{2}:\d{2}$/)) {
        normalizedStr += 'Z';
    }

    const date = new Date(normalizedStr);
    if (isNaN(date.getTime())) return '—';

    return date.toLocaleString([], {
        day: '2-digit', month: '2-digit', year: 'numeric',
        hour: '2-digit', minute: '2-digit'
    });
}

/* ==========================================================================
   КОНЕЦ РЕГИОНА РАБОТЫ С ДАТАМИ
   ========================================================================== */