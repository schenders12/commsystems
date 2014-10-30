$(document).ready(function () {

});

// JQuery UI methods to create dialogs for Choose Page, Delete page, Edit module, Delete Module
$("#choose-page").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 400,
    title: "Create Page"
});

$("#delete-page").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 400,
    title: "Delete Page"
});

$("#edit-module").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 750,
    title: "Edit Module"
});

$("#delete-module").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 550,
    title: "Are you sure you want to delete this module?"
});

// View Page button
$(".view-page-button").button().click(function () {
    // Get the selected page ID
    var pageid = $(this).data("pageid");
    var myid = $(this).attr("id");
    if (typeof console != "undefined") {
        console.log("view-page-button.button" + pageid);
        console.log("view-page-button.button" + myid);
    }

   // $.ajax({
   //     url: '/FPIM/ViewFacultyPage/',
   //     data: { id: pageid },
    //    type: 'Get',
    //    traditional: true,
    //    success: function (msg) {
            // SCH fyi msg returned is html from view class
    //        alert(msg);
     //       window.location.href = '/FPIM/ViewFacultyPage/'
     //   }
   // });
});

$(".view-page-button").click(function () {
    // Get the selected page ID
    var pageid = $(this).data('id');
    var myid = $(this).attr('id');
    if (typeof console != "undefined") {
        console.log("view-page-button.click" + pageid);
        console.log("view-page-button.button" + myid);
    }
});

// Edit Page Button
$(".edit-page-button").click(function () {
    // Get the selected page ID
    var pageid = $(this).data('id');
    if (typeof console != "undefined") {
        console.log("Edit-page-button.click" + pageid);
    }
});

$(".edit-page-button").button().click(function () {
    // Get the selected page ID
    var pageid = $(this).data('pageid');
    if (typeof console != "undefined") {
        console.log("edit-page-button.button" + pageid);
    }

});

// Delete Page Button
$(".delete-page-button").click(function () {
    // Get the selected page ID
    var pageid = $(this).data('id');
    if (typeof console != "undefined") {
        console.log("Delete-page-button.click" + pageid);
    }
});

$(".delete-page-button").button().click(function () {
    // Get the selected page ID
    var pageid = $(this).data('pageid');
    if (typeof console != "undefined") {
        console.log("delete-page-button.button" + pageid);
    }
});

$(".column").sortable({
    connectWith: ".column",
    handle: ".portlet-header",
    cancel: ".portlet-toggle",
    placeholder: "portlet-placeholder ui-corner-all"
});

$(".portlet")
  .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
  .find(".portlet-header")
    .addClass("ui-widget-header ui-corner-all")
    .prepend("<span class='ui-icon ui-icon-minusthick portlet-toggle'></span>");

$(".portlet-toggle").click(function () {
    var icon = $(this);
    icon.toggleClass("ui-icon-minusthick ui-icon-plusthick");
    icon.closest(".portlet").find(".portlet-content").toggle();
});
$("#accordion").accordion({
    collapsible: true, active: 'none'
});$.fn.togglepanels = function () {
    return this.each(function () {
        $(this).addClass("ui-accordion ui-widget ui-helper-reset")
      .find("h3")
        .addClass("ui-accordion-header ui-helper-reset ui-state-default ui-corner-all ui-accordion-icons notaccordion-height")
        .hover(function () { $(this).toggleClass("ui-state-hover"); })
        .prepend('<span class="ui-accordion-header-icon ui-icon ui-icon-triangle-1-e"></span>')
        .click(function () {
            $(this)
              .toggleClass("ui-accordion-header-active ui-state-active ui-state-default ui-corner-bottom")
              .find("> .ui-icon").toggleClass("ui-icon-triangle-1-e ui-icon-triangle-1-s").end()
              .next().slideToggle();
            return false;
        })
        .next()
          .addClass("ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom")
          .hide();
    });
};$("#notaccordion").togglepanels();

$(".delete-module-toolbar").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    alert('Are you sure you want to delete this module?');
    alert($(this).attr("href"));
    //window.location($(this).attr("href"));
    window.location.href = '/FPIM/Index/'

});

$(".delete-module-button").click(function (event) {
event.stopPropagation();
event.preventDefault();

// Get the selected module ID
var htmlID = $(this).attr("id");
var moduleID = $(this).data('moduleid');
//alert(moduleID);
// Call controller method for deleting module
$.ajax({
    url: '/FPIM/DeleteModulePartial/',
    data: { id: moduleID },
    type: 'Get',
    traditional: true,
    success: function (msg) {
        $("#delete-module").dialog("open");
        $("#delete-module").empty().append(msg);
    }
});
});

$(".delete-page-button").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    // Get the selected module ID
    var htmlID = $(this).attr("id");
    var pageID = $(this).data('pageid');
   // alert(pageID);
    // Call controller method for deleting module
    $.ajax({
        url: '/FPIM/DeletePagePartial/',
        data: { id: pageID },
        type: 'Get',
        traditional: true,
        success: function (msg) {
            $("#delete-page").dialog("open");
            $("#delete-page").empty().append(msg);
        }
    });
});


$( "#sortable" ).sortable({
    placeholder: "ui-state-highlight"
});
$("#sortable").disableSelection();

$("#edit-page-submit").button();

$("#edit-page-submit").click(function () {
   // debugger;
   // $.ajax({
    //    url: '/Admin/SortedLists/',
    //    data: { items: $("#sortable").sortable('toArray') },
    //    type: 'post',
    //    traditional: true
    //});
    //Set inserted vlaues
    var pageTitle = $("#PageTitle").val();
    var pageID = $("#FacultyPageId").val();

    var sortedModules = $("#sortable").sortable('toArray');
    //var sorted = $(".selector").sortable("serialize", { key: "sort" });
   // alert(sortedModules);
   // alert(pageTitle);
   // alert(pageID);
   // alert(sorted);
    $.ajax({
        url: '/FPIM/EditFacultyPage/',
        data: { items: sortedModules, title: pageTitle, id: pageID },
        type: 'post',
        traditional: true,
        success: function (msg) {
            alert("Success!!");
            window.location.href = '/FPIM/Index/';
        }
    });

});

// Clicik event for adding a page
$(".create-page-choose").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    var profileID = $(this).data('profileid');
    //alert(profileID);
    // Call controller method for editing module
    $.ajax({
        url: '/FPIM/ChoosePageTypePartial/',
        data: { profileID: profileID },
        type: 'Get',
        traditional: true,
        success: function (msg) {
            $("#choose-page").dialog("open");
            $("#choose-page").empty().append(msg);
        }
    });

});



