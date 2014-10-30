//View module dialog box
$("#view-module").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 400,
    title: "View Module"
});

//Add Page Submit button click
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

$(document).ready(function () {

    var profileId = $('#ProfileId').attr('value');
    var pageTitle = $('#PageTitle').attr('value');

    // Create sortables for the modules
    $(".gallery").sortable({
        placeholder: "ui-state-highlight",
        connectWith: '.gallery',
        revert:  150,
        receive: function(event, ui) {
           // ui.item.fadein();
        }
    }).disableSelection(); // sortable
  //  $("#available-modules").sortable({
   //     placeholder: "ui-state-highlight",
   //     connectWith: '.gallery'
   // }).disableSelection(); // sortable
   //  $("#sortable-modules").sortable({
    //     placeholder: "ui-state-highlight",
    //     connectWith:  '.gallery'
    // }).disableSelection(); // sortable

    // Get the modules and add them to the available module list
    var url = ROOT + 'FPIM/GetModules/';
    $.ajax({
        url: url,
        type: 'Get',
        data: { myId: profileId },
        success: function (data) {
            $.each(data, function (i, value) {
                // Create a draggable item for each module and add to the list
               // var moduleHTML = '<li class="ui-widget-content ui-corner-tr ui-sortable-handle available-module" id="' + value.FacultyProfileModuleId + '" data-modid="' + value.FacultyProfileModuleId + '" value="' + value.FacultyProfileModuleId + '">' +
                var moduleHTML = '<li class="ui-state-default ui-sortable-handle" id="' + value.FacultyProfileModuleId + '" data-modid="' + value.FacultyProfileModuleId + '" value="' + value.FacultyProfileModuleId + '">' +
                    '&nbsp' + value.ModuleTitle + '' + 
                   // '<a href="" title = "View Module" class="ui-icon ui-icon-zoomin">View Module</a>' +
                   // '<a href="" title = "Add to Page" class="ui-icon ui-icon-plus">Add to Page</a>' +
                    '</li>'
                $('#available-modules').append(moduleHTML);
            }); // each

        } // success
    }); // ajax
});



