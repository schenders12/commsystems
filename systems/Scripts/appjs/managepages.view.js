// Choose type of page
$("#choose-page").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 400,
    title: "Create Page"
});

// Click event for adding a page
$(".create-page-choose").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    var profileID = $(this).data('profileid');

    // Call controller method for editing module
    var url = ROOT + 'FPIM/ChoosePageTypePartial/';
    $.ajax({
        url: url,
        data: { profileID: profileID },
        type: 'Get',
        traditional: true,
        success: function (msg) {
            $("#choose-page").dialog("open");
            $("#choose-page").empty().append(msg);
        }
    });

});

// Delete a page
$("#delete-page").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 550,
    title: "Are you sure you want to delete this page?"
});

$(".delete-page-button").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    // Get the selected module ID
    var htmlID = $(this).attr("id");
    var pageID = $(this).data('pageid');

    // Call controller method for deleting module
    var url = ROOT + 'FPIM/DeletePagePartial/';
    $.ajax({
        url: url,
        data: { pageId: pageID },
        type: 'Get',
        traditional: true,
        success: function (msg) {
            $("#delete-page").dialog("open");
            $("#delete-page").empty().append(msg);
        }
    });
});
