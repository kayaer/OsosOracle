/*!
 * FileInput <_LANG_> Translations
 *
 * This file must be loaded after 'fileinput.js'. Patterns in braces '{}', or
 * any HTML markup tags in the messages must not be converted or translated.
 *
 * @see http://github.com/kartik-v/bootstrap-fileinput
 *
 * NOTE: this file must be saved in UTF-8 encoding.
 */
(function ($) {
    "use strict";

    $.fn.fileinput.locales.tr = {
        fileSingle: 'dosya',
        filePlural: 'dosya',
        browseLabel: 'Gözat &hellip;',
        removeLabel: 'Sil',
        removeTitle: 'Seçili dosyaları temizle',
        cancelLabel: 'Vazgeç',
        cancelTitle: 'Yüklemeyi iptal et',
        uploadLabel: 'Yükle',
        uploadTitle: 'Seçili dosyaları yükle',
        msgSizeTooLarge: 'Dosyanız "{name}" (<b>{size} KB</b>) izin verilen maksimum yükleme boyutunu <b>({maxSize} KB)</b> aşıyor!',
        msgFilesTooLess: 'Yükleme icin en az <b>{n}</b> {files} seçiniz!',
        msgFilesTooMany: 'Seçmiş olduğunuz <b>({n})</b> dosya izin verilen <b>({m})</b> dosya sayisini asiyor!',
        msgFileNotFound: 'Dosya "{name}" bulunamadı!',
        msgFileSecured: 'Güvenlik sebebi ile dosya "{name}" okunamıyor.',
        msgFileNotReadable: 'Dosya "{name}" okunabilir değil.',
        msgFilePreviewAborted: 'Dosya "{name}" görüntuleme iptal edildi.',
        msgFilePreviewError: 'Dosya "{name}" okunurken hata oluştu.',
        msgInvalidFileType: 'Geçersiz dosya "{name}" türü. Sadece "{types}" dosya türleri desteklenir.',
        msgInvalidFileExtension: 'Geçersiz dosya "{name}" uzantısı. Sadece "{extensions}" dosya uzantıları desteklenir.',
        msgValidationError: 'Dosya Yükleme Hatası',
        msgLoading: 'Dosya yükleniyor {index} / {files} &hellip;',
        msgProgress: 'Dosya yükleniyor {index} / {files} - {name} - {percent}% tamamlandı.',
        msgSelected: '{n} {files} seçildi',
        msgFoldersNotAllowed: 'Sadece dosya sürükle & bırak! Sürüklenen {n} klasör(ler) atlandı.',
        dropZoneTitle: 'Sürükle & Bırak &hellip;'
    };

    $.extend($.fn.fileinput.defaults, $.fn.fileinput.locales.tr);
})(window.jQuery);