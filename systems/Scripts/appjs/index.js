$(document).ready(function () {

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
});

$.fn.togglepanels = function () {
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
};
//$("#notaccordion").togglepanels();




$( "#sortable" ).sortable({
    placeholder: "ui-state-highlight"
});
$("#sortable").disableSelection();



