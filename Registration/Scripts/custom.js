$(document).ready(function () {
    $(":checkbox").click(function (event) {
        if ($(this).is(":checked"))
            $(".banquetMenu").show()
        else
            $(".banquetMenu").hide()
    });
});