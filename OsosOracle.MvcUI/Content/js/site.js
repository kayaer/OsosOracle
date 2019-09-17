$(function () {
    $('#changeLang').popover({
        container: 'body',
        placement: 'top',
        trigger: 'click',
        html: true,
        content: function () {
            return $("#popoverLangContent").html();
        }
    });
});

$(document)
    .ready(function () {
        pageInit("#bdy");
    });

$(document).ready(function () {


});



