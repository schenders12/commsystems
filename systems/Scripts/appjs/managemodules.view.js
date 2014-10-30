//Choose a module
$("#create-module").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 600,
    title: "Create Module"
});

$(".create-module-choose").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    var profileID = $(this).data('profileid');

    // Call controller method for editing module
    var url = ROOT + "FPIM/ChooseModuleTypePartial/";
    $.ajax({
        url: url,
        data: { profileID: profileID },
        type: 'Get',
        traditional: true,
        success: function (msg) {
            $("#create-module").dialog("open");
            $("#create-module").empty().append(msg);
        }
    });

});

// Delete a module
$("#delete-module").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 550,
    title: "Are you sure you want to delete this module?  It will be deleted from all your pages."
});

$(".delete-module-button").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    // Get the selected module ID
    var htmlID = $(this).attr("id");
    var moduleID = $(this).data('moduleid');
    //alert(moduleID);
    // Call controller method for deleting module
    var url = ROOT + 'FPIM/DeleteModulePartial/';
    $.ajax({
        url: url,
        data: { id: moduleID },
        type: 'Get',
        traditional: true,
        success: function (msg) {
            $("#delete-module").dialog("open");
            $("#delete-module").empty().append(msg);
        }
    });
});