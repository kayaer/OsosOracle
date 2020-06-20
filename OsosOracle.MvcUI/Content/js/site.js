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

        setTimeout(function () { pageInit("#bdy"); },1000);
       
    });

$(document).ready(function () {
    //GetSessionInformation();
    
});



