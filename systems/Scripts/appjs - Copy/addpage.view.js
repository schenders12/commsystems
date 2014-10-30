//jQueryUI method to create dialog box

$("#view-module").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 400,
    title: "View Module"
});

$(document).ready(function () {

    var profileId = $('#ProfileId').attr('value');
    var pageTitle = $('#PageTitle').attr('value');

    // Create a droppable for the modules, accepting the gallery list items
    $("#new-page-droppable").droppable({
        accept: "#gallery > li",
        accept: "#sortable-modules > li",
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            addModule(ui.draggable);
        }
    }); // droppable
    // Append a sortable ul element to the droppable
    $("<ul id='sortable-modules' class='gallery ui-helper-reset'/>").appendTo('#new-page-droppable');
    $("#sortable-modules").sortable({
        connectWith:  "#new-page-droppable"});

    $.ajax({
        url: "/FPIM/GetModules",
        type: 'Get',
        data: { myId: profileId },
        success: function (data) {
            $.each(data, function (i, value) {
                // *** old code, not using right now
                //$('#select-modules').append($('<option>').text(value.ModuleTitle).attr('value', value.ModuleTitle));
                //$('#select-modules').after('<input id="' + value.ModuleTitle + '" name="chosen-modules" type="checkbox" value="' + value.FacultyProfileModuleId + '">&nbsp' + value.ModuleTitle + '</p>');
                //$('#select-modules').append($('<option>').text(value).attr('value', value));

                //$('#select-draggable-modules').after('<div id="' + id + '" class="draggable-module ui-widget-content"><input id="' + value.ModuleTitle + '" name="chosen-modules" type="checkbox" value="' + value.FacultyProfileModuleId + '">&nbsp' + value.ModuleTitle + '</p></div>');
                //$('#select-draggable-modules').after('<div id="' + id + '" class="draggable-module ui-widget-content">&nbsp' + value.ModuleTitle + '</p>' + value.ModuleData + '</div>');
                // $('#select-draggable-modules').after('<div id="' + id + '" class="draggable-module ui-widget-content"><h3>&nbsp' + value.ModuleTitle + '</h3>' + value.ModuleData + '</div>');
                // end old code ***

                // Create a draggable item for each module and connect to the sortable ul item
                var id = 'draggable' + value.FacultyProfileModuleId;
                var moduleHTML = '<li class="ui-widget-content ui-corner-tr ui-sortable-handle" id="' + value.FacultyProfileModuleId + '">' +
                    '<h3>&nbsp' + value.ModuleTitle + '</h3>' + 
                    '<a href="" title = "View Module" class="ui-icon ui-icon-zoomin">View Module</a>' +
                    '<a href="" title = "Add to Page" class="ui-icon ui-icon-plus">Add to Page</a>' +
                    '</li>'
                $('#gallery').append(moduleHTML);
                $('#' + value.FacultyProfileModuleId).draggable({
                    containment: 'document',
                    cursor: 'move',
                    snap: '#droppable-page',
                    helper: "clone",
                    revert: 'invalid',
                    connectToSortable: '#sortable-modules'
                }); // draggable

            }); // each

            // let the gallery be droppable as well, accepting items from the newPage
            $("#gallery").droppable({
                accept: "#new-page-droppable li",
                activeClass: "custom-state-active",
                drop: function (event, ui) {
                    removeModule(ui.draggable);
                }
            }); // droppable

            // resolve the icons behavior with event delegation
            $("ul.gallery > li").click(function (event) {

                event.stopPropagation();
                event.preventDefault();

                var $item = $(this);
                var $target = $(event.target);

                if ($target.is("a.ui-icon-close")) {
                    removeModule($item);
                } else if ($target.is("a.ui-icon-zoomin")) {
                    //viewLargerModule($target);
                    viewLargerModule($item);
                } else if ($target.is("a.ui-icon-plus")) {
                    addModule($item);
                }

                return false;
            });
        } // success
    }); // ajax

    var $gallery = $("#gallery"),
    $newPage = $("#new-page-droppable");

    // Module Add function
    function addModule($item) {
        var remove_icon = "<a href='' title='Remove this module' class='ui-icon ui-icon-close'>Remove Module</a>";
        var $newPage = $("#new-page-droppable");

        $item.fadeOut(function () {
            // Get the ul:  Find if a ul has been created in the new page
            // If not, add it
            var $list = $("ul", $newPage).length ?
              $("ul", $newPage) :
              $("<ul id='sortable-modules' class='new-gallery ui-helper-reset'/>").appendTo($newPage);

            // If the item is already in the list, it is only being sorted and not added
            var myId = $item.attr('id');
            var myFind = "input.#" + myId;
            var child = $list.children().attr(myId);
            var $child = $(myId);
            //if ($($list > $child).length) {
            if ($item.hasClass('added')) {
            //if ($(child).length) {
                // Just fade the item
                $item.fadeIn(function () {
                    $item.animate({ width: "300px" }).find("img").animate({ height: "36px" });
                });
            }
            // Else create a new item
            else  {
                // Remove the plus icon from the draggable item
                $item.find("a.ui-icon-plus").remove();
                // Add the remove icon from the draggable item
                $item.append(remove_icon);

                // Add a hidden element for MVC
                var draggId = $item.attr('id');
                var moduleHidden = '<input id="' + draggId + '" name="addMe" type ="hidden" value="' + draggId + '">';
                $item.append(moduleHidden);

                // Append the draggable to the list and fade it in, while resizing
                $item.appendTo($list).fadeIn(function () {
                    $item.animate({ width: "300px" }).find("img").animate({ height: "36px" });
                });

                // Add a class to item
                $item.addClass('added');
            }

        });
    }

    // Module Remove function
    function removeModule($item) {
        var add_icon = "<a href='' title='Add this module' class='ui-icon ui-icon-plus'>Add Module</a>";
        $item.fadeOut(function () {
            $item.find("a.ui-icon-close")
                .remove()
              .end()
                .css("width", "300px")
                .append(add_icon)
                .find("img")
                .css("height", "72px")
              .end()
              .appendTo($gallery)
              .fadeIn();
        });

        // Remove add-me class
        $item.removeClass("add-this-module");
    }

    // Module View function
    function viewLargerModule($item) {
        event.stopPropagation();
        event.preventDefault();

        var myID = $item.attr('id');

        // Call controller method for editing module
        $.ajax({
            url: '/FPIM/ViewModulePartial/',
            data: { id: myID },
            type: 'Get',
            traditional: true,
            success: function (msg) {
                $("#view-module").dialog("open");
                $("#view-module").empty().append(msg);
            }
        });
    }
});



$(".add-page-submit").click(function () {

    //event.stopPropagation();
   // event.preventDefault();
    //alert("Add");
    // Get all droppables contained in draggable  ???
   // var myid = $("#new-page-droppable").find("li").attr("id");
   // alert(myid);
   // var myModules = [];
   // $("li").each(function (index) {
    //    console.log(index + ": " + $(this).text());
     //   if ($(this).hasClass('add-this-module'))
    //    {
     //       alert("Added " + $(this).attr('id'));
     //       myModules.push($(this).attr('id'));
     //   }
   // });
   // var profileId = $('#ProfileId').attr('value');
   // var pageTitle = $('#PageTitle').attr('value');
   // if ($('#PageTitle')[0].value.length == 0) {
   //     event.stopPropagation();
   //     event.preventDefault();
   //     return;
   // }
   // var sortedModules = $("#sortable-modules").sortable('toArray');
    // Call controller function
   // $.ajax({
    //    url: '/FPIM/AddFacultyPage/',
    //    data: { profileId: profileId, pageTitle: pageTitle, modules: sortedModules },
     //   type: 'post',
     //   traditional: true,
     //   success: function (msg) {
     //       window.location.href = '/FPIM/Index/';
      //  }
   // });
});


