setInterval(function () {
    var url = "/User/GuiGM";
    $.ajax({
        url: url,
        type: "POST",
        data: {},
        dataType: "json",
        timeout: 30 * 1000,
        error: function (jqXHR, textStatus, errorThrown) {
            //alert('Error - ' + errorThrown);
        }
    })
        .done(function (data) {
            //alert(data); 
        })
        .fail(function (jqXHR, status, error) {
            //console.log(jqXHR);
            //alert('fail');
        });
}, 5000);