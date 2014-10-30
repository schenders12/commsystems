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
  //  $("#new-page-droppable").droppable({
   //     accept: "#gallery > li",
     //   accept: "#new-page-droppable li",
     //   activeClass: "ui-state-highlight",
     //   drop: function (event, ui) {
      //      addModule(ui.draggable);
     //   }
   // }); // droppable

    // let the gallery be droppable as well, accepting items from the newPage
  //  $("#gallery").droppable({
   ////     accept: "#new-page-droppable li",
   //     accept: "#gallery > li",
   //     activeClass: "custom-state-active",
   //     drop: function (event, ui) {
    //        removeModule(ui.draggable);
    //    }
   // }); // droppable

    var url = ROOT + 'FPIM/GetModules/';
    $.ajax({
        url: url,
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
                var moduleHTML = '<li class="ui-widget-content ui-corner-tr" id="' + value.FacultyProfileModuleId + '">' +
                //var moduleHTML = '<li id="' + value.FacultyProfileModuleId + '">' +
                    '<h3>&nbsp' + value.ModuleTitle + '</h3>' + 
                    '<a href="" title = "View Module" class="ui-icon ui-icon-zoomin">View Module</a>' +
                    '<a href="" title = "Add to Page" class="ui-icon ui-icon-plus">Add to Page</a>' +
                    '</li>'
                $('#gallery').append(moduleHTML);
               // $('#' + value.FacultyProfileModuleId).draggable({
              //      containment: 'document',
               //     cursor: 'move',
                    //snap: '#new-page-droppable',
                //    helper: "clone",
               //     revert: 'invalid',
                   // connectToSortable: '#sortable-modules'
              //  }); // draggable

            }); // each

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
           // if ($item.hasClass('added')) {
            //if ($(child).length) {
                // Just fade the item
           //     $item.fadeIn(function () {
           //         $item.animate({ width: "300px" }).find("img").animate({ height: "36px" });
           //     });
           // }
            // Else create a new item
           // else  {
                // Remove the plus icon from the draggable item
                $item.find("a.ui-icon-plus").remove();
                // Add the remove icon from the draggable item
                $item.append(remove_icon);

                // Add a hidden element for MVC
                var draggId = $item.attr('id');
                var moduleHidden = '<input id="' + draggId + '" name="addMe" type ="hidden" value="' + draggId + '">';
                $item.append(moduleHidden);

                // Append the draggable to the list and fade it in, while resizing
             //   $item.appendTo($list).fadeIn(function () {
             //       $item.animate({ width: "340px" }).find("img").animate({ height: "36px" });
              //  });

                // Add a class to item
            //    $item.addClass('added');
           // }

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
            //  .appendTo($gallery)
              .fadeIn();
        });

        // Remove hidden MVC element
     //   var draggId = $item.attr('id');
      //  $newPage.remove(draggId);
    }

    // Module View function
    function viewLargerModule($item) {
        event.stopPropagation();
        event.preventDefault();

        var myID = $item.attr('id');

        var url = ROOT + 'FPIM/ViewModulePartial/';
        // Call controller method for editing module
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

    initSort($('.connectedSortable'));


    function initSort(element) {

        element.sortable({
            connectWith: '.connectedSortable',
            beforeStop: function (event, ui) {
                var parent = ui.helper.parent();
                if (parent.hasClass('new')) {
                    parent.removeClass('new');
                    var clone = $('.connectedSortable.new.hidden').clone();
                    clone.insertAfter(parent).removeClass('hidden');
                    initSort($('.connectedSortable'));
                }
                cleanUp();
            }
        }).disableSelection();

    }

    function cleanUp() {

        // Remove the ul
        $('.connectedSortable').not('.new, #gallery').each(function () {
            if ($(this).find('li').length == 0) {
                // Remove the ul from the new page Called when the last list
                // item is removed from the page
                $(this).remove();
            }
        });
    }

});



