/**
 * Универсальное окно подтверждения в пастельно-голубом стиле
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