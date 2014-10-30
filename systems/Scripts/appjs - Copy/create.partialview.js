//jQueryUI method to create dialog box
$("#create-module").dialog({
    autoOpen: false,
    modal: true,
    width: 1000,
    height: 600,
    title: "Create Module"
});

$("input:radio[name=ModuleType]").click(function () {

    // This method is called when the radio buttons are clicked
    //event.stopPropagation();
    //event.preventDefault();

    // Get the selected module type
    //radioID = $(this).val();
    //alert(radioID);
    //var moduleType = $(this).data('moduletype');
    //alert(moduleType);

});

$(".create-module-choose").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    var profileID = $(this).data('profileid');
   // alert(profileID);
    // Call controller method for editing module
    $.ajax({
        url: '/FPIM/ChooseModuleTypePartial/',
        data: { profileID: profileID },
        type: 'Get',
        traditional: true,
        success: function (msg) {
            $("#create-module").dialog("open");
            $("#create-module").empty().append(msg);
        }
    });

});
$("#create-module-submit").click(function () {
    //alert("Create");
});

$("#choose-module-submit").click(function () {
    // On submit button click close dialog box
    $("#create-module").dialog("close");

    //Set inserted vlaues
    var modType = $("input:radio[name=ModuleType]").val();

    var profileID = $(this).data('profileid');
    //alert(profileID);

   // var profileID = $("#profileID").val();
    //alert(profileID + " Selected module type " + modType + ", creating....");

    if (modType == "HTML") {
        // Call HTML method
       // $.post('/FPIM/AddHTMLModule', { "profileID": modType },
           // function () {
                //alert("Module has been created successfully.");
               // var postForm = document.createElement("form");
               // postForm.setAttribute("method", "get");
              //  postForm.setAttribute("action", "/FPIM/AddHTMLModule");
              //  var postVal = document.createElement("input");
              //  postVal.setAttribute("type", "hidden");
             //   postVal.setAttribute("name", "profileID");
        // postVal.setAttribute("value", $("#HotelDDL option:selected").val());
              //  postVal.setAttribute("value", profileID);
              //  postForm.appendChild(postVal);
              //  document.body.appendChild(postForm);
             //   $(postForm).submit();

           // });
    }
    if (modType == "FileInclude") {
        // Call HTML method
        $.post('/FPIM/AddFileIncludeModule', { "modType": modType },
            function () {
                alert("Module has been created successfully.");
                window.location.reload(true);

            });
    }

});
