﻿



$(".edit-module-button").click(function (event) {
   // event.stopPropagation();
    //event.preventDefault();

    // Get the selected module ID
    var htmlID = $(this).attr("id");
    var moduleID = $(this).data('moduleid');

    // Call controller method for editing module
   // $.ajax({
    //    url: '/FPIM/EditFacultyModulePartial/',
   //     data: { id: moduleID },
    //    type: 'Get',
    //    traditional: true,
    //    success: function (msg) {
    //        $("#edit-module").dialog("open");
    //        $("#edit-module").empty().append(msg);
     //   }
    //});

});

$("#edit-module-save").click(function () {
    $("#edit-module").dialog("close");

    var title = $("#edit-module-title").val();
    var data = $("#edit-module-data").val();

   // alert(title + "   " + data);

    // Call Edit action method
    //$.post('/Home/Edit', { "id": id, "name": name, "instructor": instructor, "startdate": startdatepicker, "enddate": enddatepicker, "time": timepicker, "duration": duration }, function () {
    //    alert("data is posted successfully");
     //   window.location.reload(true);
   // });
});

