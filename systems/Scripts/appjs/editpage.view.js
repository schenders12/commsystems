//Edit Page Submit button click
$("#edit-page-submit").click(function () {
    // debugger;
    event.stopPropagation();
    event.preventDefault();

    //Set inserted vlaues
    var pageTitle = $("#PageTitle").val();
    var profileID = $("#ProfileId").val();
    var pageID = $("#FacultyPageId").val();

    var sortedModules = $("#sortable-modules").sortable('toArray', { attribute: 'data-modid' });

    if (pageTitle != "") {
        var url = ROOT + 'FPIM/EditFacultyPage/';
        $.ajax({
            url: url,
            data: { items: sortedModules, title: pageTitle, id: pageID },
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
    var pageId = $('#PageId').attr('value');

    // Create sortables for the modules
    $(".gallery").sortable({
        placeholder: "ui-state-highlight",
        connectWith: '.gallery',
        revert: 150,
        receive: function (event, ui) {
            // ui.item.fadein();
        }
    }).disableSelection(); 

    // Get the available modules and add them to the available module list
    var url = ROOT + 'FPIM/GetModules/';
    $.ajax({
        url: url,
        type: 'Get',
        data: { myId: profileId},
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

    // Get the modules for this page and add them to the sortable module list
   // var url = ROOT + 'FPIM/GetPageModules/';
   // $.ajax({
    //    url: url,
    //    type: 'Get',
     //   data: { myId: profileId, pageId: pageId },
     //   success: function (data) {
     //       $.each(data, function (i, value) {
                // Create a draggable item for each module and add to the list
                // var moduleHTML = '<li class="ui-widget-content ui-corner-tr ui-sortable-handle available-module" id="' + value.FacultyProfileModuleId + '" data-modid="' + value.FacultyProfileModuleId + '" value="' + value.FacultyProfileModuleId + '">' +
      //          var moduleHTML = '<li class="ui-state-default ui-sortable-handle" id="' + value.FacultyProfileModuleId + '" data-modid="' + value.FacultyProfileModuleId + '" value="' + value.FacultyProfileModuleId + '">' +
      //              '&nbsp' + value.ModuleTitle + '' +
                   // '<a href="" title = "View Module" class="ui-icon ui-icon-zoomin">View Module</a>' +
                   // '<a href="" title = "Add to Page" class="ui-icon ui-icon-plus">Add to Page</a>' +
       //             '</li>'
       //         $('#sortable-modules').append(moduleHTML);
       //     }); // each

       // } // success
   // }); // ajax

});