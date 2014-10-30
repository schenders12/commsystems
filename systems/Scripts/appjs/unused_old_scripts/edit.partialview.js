$(".edit-module-button").click(function (event) {
   // event.stopPropagation();
    //event.preventDefault();

    // Get the selected module ID
    var htmlID = $(this).attr("id");
    var moduleID = $(this).data('moduleid');


});

$("#edit-module-save").click(function () {
    $("#edit-module").dialog("close");

    var title = $("#edit-module-title").val();
    var data = $("#edit-module-data").val();


});

