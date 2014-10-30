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

    // Create a droppable for the modules, accepting the module list items
    $("#new-page-droppable").droppable({
        accept: "#available-modules > li",
        activeClass: "ui-state-highlight",
        drop: function (event, ui) {
            addModule(ui.draggable);
        }
    }); // droppable

    // Make the available modules be droppable to 
    // accepting items from the new page if user drags them back
      $("#available-modules").droppable({
          accept: "#new-page-droppable li",
          activeClass: "custom-state-active",
          drop: function (event, ui) {
            removeModule(ui.draggable);
        }
    }); // droppable

    // Get the modules and add them to the available module list
    var url = ROOT + 'FPIM/GetModules/';
    $.ajax({
        url: url,
        type: 'Get',
        data: { myId: profileId },
        success: function (data) {
            $.each(data, function (i, value) {
                // Create a draggable item for each module and add to the list
                var moduleHTML = '<li class="ui-widget-content ui-corner-tr" id="' + value.FacultyProfileModuleId + '">' +
                    '<h3>&nbsp' + value.ModuleTitle + '</h3>' + 
                    '<a href="" title = "View Module" class="ui-icon ui-icon-zoomin">View Module</a>' +
                    '<a href="" title = "Add to Page" class="ui-icon ui-icon-plus">Add to Page</a>' +
                    '</li>'
                $('#available-modules').append(moduleHTML);

                $('#' + value.FacultyProfileModuleId).draggable({
                    containment: 'document',
                    cursor: 'move',
                    snap: '#new-page-droppable',
                    helper: "clone",
                    revert: 'invalid',
                   // connectToSortable: '#sortable-modules'
                }); // draggable

            }); // each

            // resolve the icons behavior with event delegation
             $("ul.gallery > li").click(function (event) {
                event.stopPropagation();
                event.preventDefault();

                var $item = $(this);
                var $target = $(event.target);

                if ($target.is("a.ui-icon-close")) {
                    // removeModule($item);
                    removeModule($(this));
                } else if ($target.is("a.ui-icon-zoomin")) {
                    //viewLargerModule($target);
                    // viewLargerModule($item);
                    viewLargerModule($(this));
                } else if ($target.is("a.ui-icon-plus")) {
                    // addModule($item);
                    addModule($(this));
                }

                return false;
            });
        } // success
    }); // ajax

    var $gallery = $("#draggables"),
    $newPage = $("#new-page-droppable");

    // Module Add function
    function addModule($item) {
        var remove_icon = "<a href='' title='Remove this module' class='ui-icon ui-icon-close'>Remove Module</a>";
        var $newPage = $("#new-page-droppable");

        $item.fadeOut(function () {
            // Get the ul:  Find if a ul has been created in the new page
            // If not, add it
            var $list = $("ul", $("#new-page-droppable")).length ?
              $("ul", $("#new-page-droppable")) :
              $("<ul class='gallery ui-helper-reset'/>").appendTo($("#new-page-droppable"));

            // If the item is already in the list, it is only being sorted and not added
           // var myId = $item.attr('id');
          //  var myFind = "input.#" + myId;
          //  var child = $list.children().attr(myId);
          //  var $child = $(myId);
          //  if ($($list > $child).length) {
           // if ($item.hasClass('added')) {
            //if ($(child).length) {
                // Just fade the item
           //     $item.fadeIn(function () {
           //         $item.animate({ width: "300px" }).find("img").animate({ height: "36px" });
           //     });
        //    }
            // Else create a new item
          //  else  {
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
                    $item.animate({ width: "340px" }).find("img").animate({ height: "36px" });
                });

           // }

        });
    }

    // Module Remove function
    function removeModule($item) {
        // var $gallery = $("#available-modules");
        // Create the html for the hidden icon
        var add_icon = "<a href='' title='Add this module' class='ui-icon ui-icon-plus'>Add Module</a>";
        $item.fadeOut(function () {
            $item.find("a.ui-icon-close")
                // Remove the close icon
                .remove()
              .end()
                // Remove the hidden MVC input item
               .find("input")
                .remove()
              .end()
                .css("width", "300px")
                // Add the plus icon
                .append(add_icon)
                .find("img")
                .css("height", "72px")
              .end()
                // Append to available modules
              .appendTo($("#available-modules"))
              .fadeIn();
        });

    }

    // Module View function
    function viewLargerModule($item) {
        event.stopPropagation();
        event.preventDefault();

        var myID = $item.attr('id');

        var url = ROOT + 'FPIM/ViewModulePartial/';
        // Call controller method for displaying module
        $.ajax({
            url: url,
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



