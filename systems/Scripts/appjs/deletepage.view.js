// Dialog for deleting a page
$("#delete-page").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 400,
    title: "Delete Page"
});

$("#add-page-submit").click(function () {
    // debugger;
    event.stopPropagation();
    event.preventDefault();

    //Set inserted vlaues
    var pageTitle = $("#PageTitle").val();
    var profileID = $("#ProfileId").val();

    var sortedModules = $("#sortable-modules").sortable('toArray', { attribute: 'data-modid' });

    if (pageTitle != "") {
        var url = ROOT + 'FPIM/AddFacultyPage/';
        $.ajax({
            url: url,
            data: { items: sortedModules, title: pageTitle, profileID: profileID },
            type: 'post',
            traditional: true,
            success: function (response) {
                window.location.href = response.Url;
            }
        });
    }
    else {
        alert("Please enter a title for the page.");
    }

});