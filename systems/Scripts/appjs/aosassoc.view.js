// JavaScript source code for AOS and Assoc page

$(document).ready(function () {
    $("#primary-association").addItems();
});

// Adds AOS items that are  in database
$.fn.addItems = function () {

    var url = ROOT + 'FPIM/GetAOSCodes/';
    $.ajax({
        //Call GetInstructorList action method
        url: url,
        type: 'Get',
        success: function (data) {
            $.each(data, function (i, value) {
                // Transpose value.Short into uppercase
                value.Short = value.Short.toUpperCase();
                // Split code into DEPT prefix and AOS code
                var n = value.Short.indexOf("-");
                var dept = value.Short.slice(0, n);
                var aos = value.Short.slice(n+1);

                // Search for an already existing control
                //  If the control exists, skip it
                var deptElement = document.getElementById(value.Short);

                if (deptElement != null) {
                    console.log("Found " + value.Short + ", adding Areas of Interest control...");
                    // Add textarea for participating areas
                    $(deptElement).after('<div id=\"' + value.Short + '_PA\" style=\"display:none\"><p><strong>' + value.AreaOfStudy + ' Areas of Interest</strong></p>' +
                                     '<p><textarea name=\"' + value.Short + '_PA\" id=\"ParticipatingAreas\" class=\"twelve\" rows=\"5\" cols=\"80\" maxlength=\"600\">' + '</textarea></div>');
                    // Add the participating areas to the animatedcollapse functions
                    animatedcollapse.addDiv(value.Short + '_PA');
                }
                else {
                    // Find the department element and append checkbox (add after the participating areas text box)
                    var myDeptElementStr = "DEPT-" + dept + "_PA";
                    console.log("Searching for " + myDeptElementStr);
                    var myDeptElement = document.getElementById(myDeptElementStr);
                    if (myDeptElement != null) {
                        console.log("Found " + myDeptElementStr + ", adding CBox and PAs");
                        // Add the AOS Checkbox and participating area textbox
                        var paElement = value.Short + '_PA';
                        $(myDeptElement).after('<div id=' + value.Short + '><p><input id="' + value.Short + '" name="assoc" type="checkbox" value="' + value.Short + '" onClick="javascript: animatedcollapse.toggle(\'' + paElement + '\')">&nbsp' + value.AreaOfStudy + '</p></div>');

                        // Add the participating areas text box after the checkbox
                        var myAOSElement = document.getElementById(value.Short);
                        if (myAOSElement != null) {
                            $(myAOSElement).after('<div id=\"' + paElement + '\" style=\"display:none\"><p>' + value.AreaOfStudy + ' Areas of Interest (Optional)</p>' +
                                             '<p><textarea name=\"' + paElement + '\" id=\"ParticipatingAreas\" class=\"twelve\" rows=\"5\" cols=\"80\" maxlength=\"600\">' + '</textarea></div>');
                            // Add the participating areas to the animatedcollapse functions
                            animatedcollapse.addDiv(paElement);
                        }
                        else {
                            console.log("Could not find " + value.Short);
                        }
                    }
                    else {
                        // Add the checkbox (Certifications, Minors, etc....)
                        var myElementStr = "DEPT-" + dept;
                        console.log("Searching for " + myElementStr);
                        var myElement = document.getElementById(myElementStr);
                        if (myElement != null) {
                            console.log("Found " + myElementStr+ " adding Cbox " + value.Short);
                             $(myElement).after('<p><input id=\"' + value.Short + '\" name=\"assoc\" type=\"checkbox\" value=\"' + value.Short + '\">&nbsp' + value.AreaOfStudy + '</p>');
                        }
                    }
                }
            });

            animatedcollapse.ontoggle = function ($, divobj, state) { //fires each time a DIV is expanded/contracted
                //$: Access to jQuery
                //divobj: DOM reference to DIV being expanded/ collapsed. Use "divobj.id" to get its ID
                //state: "block" or "none", depending on state
            }
            animatedcollapse.init()
            $("#primary-association").checkItems();
        }
    });
};

// Checks items that are set in database
$.fn.checkItems = function () {
    // Get all hidden types with name 'checkMe'
    var checkMe = document.getElementsByName("checkMe");

    // Get the corresponding checkbox element and check it
    $.each(checkMe, function (index, value) {
        console.log("HIDDEN ELEMENT:  INDEX: " + index + " VALUE: " + this.value);
        var element = document.getElementById(this.value);
        if (element != null) {
            console.log("CHECKING ELEMENT INDEX: " + index + " VALUE: " + element.value);
            //if ($(element).attr('type') == 'checkbox') {
            if ($(element).is(':checkbox')) {
                //$(element).prop('checked', $(this).prop('checked'));
                $(element).prop('checked', true);
                animatedcollapse.show(this.value + '_PA');
                //$(element).after('<p><strong>Optional Participating Faculty list Areas of Interest</strong></p>' +
                //              '<p><textarea name=\"ParticipatingAreas\" id=\"ParticipatingAreas\" class=\"twelve\" rows=\"5\" cols=\"80\" maxlength=\"600\">' + '</textarea>');
            }
            else {
                var ckbox = $(element).find(':checkbox');
                $(ckbox).prop('checked', true);
                animatedcollapse.show(this.value + '_PA');
                // $(ckbox).after('<p><strong>Optional Participating Faculty list Areas of Interest</strong></p>' +
                //   '<p><textarea name=\"ParticipatingAreas\" id=\"ParticipatingAreas\" class=\"twelve\" rows=\"5\" cols=\"80\" maxlength=\"600\">' + '</textarea>');
            }
        }
    });
};

$("input:checkbox[name=assoc]").click(function (e) {

    var value = $(this).val();
    $(this).prop('checked', $(this).prop('checked'));
    //animatedcollapse.toggle(value + '_PA')
    //document.getElementById(value).checked = true;
    //$('input[name=assoc]').prop('checked', $(this).prop('checked'));
  //  $(this).prop('checked', $(this).prop('checked'));
    //if ($(this).prop('checked')) {
    //    alert("Create PE Control....");
    //    $(this).after('<p><input id=\"' + value.Short + '\" name=\"assoc\" type=\"checkbox\" value=\"' + value.Short + '\"/>&nbsp;' + value.AreaOfStudy + '</p>');
    //    $(this).after('<p><strong>Participating Faculty list Areas of Interest</strong></p>' + 
    //                  '<p><textarea name=\"ParticipatingAreas\" id=\"ParticipatingAreas\" class=\"twelve\" rows=\"5\" cols=\"80\" maxlength=\"600\"><' + '></textarea>');
    //}

});

