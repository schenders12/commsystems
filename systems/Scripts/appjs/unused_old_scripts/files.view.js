
$(document).ready(function () {
    var profileId = $('#profileId').attr('value');

    var url = ROOT + 'FPIM/GetFiles/';
    $.ajax({
        url: url,
        type: 'Get',
        data: { myId: profileId },
        success: function (data) {
            $.each(data, function (i, value) {
                var fileHTML = '<li id="' + value + '">';
              //  fileHTML = fileHTML + '<a href="" title = "View this file" class="ui-icon ui-icon-plus" value="' + value + '">' + value + '</a>'; // Preview TBD
                fileHTML = fileHTML + '<a href="" title = "Delete this file" class="ui-icon ui-icon-closethick" value="' + value + '">' + value + '</a>';
                fileHTML = fileHTML + "   " + value + '</li>'
                $('#fileList').append(fileHTML);
            }); // each

            // resolve the icons behavior with event delegation
            $(".ui-icon-closethick").click(function (event) {

                event.stopPropagation();
                event.preventDefault();

                var $item = $(this);
                var fileName = $item.attr('value');
                var $target = $(event.target);


                var url = ROOT + 'FPIM/DeleteFile/';
                $.ajax({
                    url: url,
                    type: 'Post',
                    data: { myId: profileId, fileName: fileName},
                    success: function (data) {
                        alert("File Deleted!!");
                        window.location.reload();
                    }, // success
                    error: function (data) {
                        alert("Delete failed");
                        window.location.reload();
                    } // error
               }); // ajax

                //return false;
            });

            // Preview icon
            $(".ui-icon-plus").click(function (event) {

                event.stopPropagation();
                event.preventDefault();

                var $item = $(this);
                var fileName = $item.attr('value');
                var $target = $(event.target);

                var url = ROOT + 'FPIM/DisplayFile/';
                $.ajax({
                    url: url,
                    type: 'Get',
                    data: { myId: profileId, fileName: fileName },
                    success: function (data) {
                        alert("Display file here!!");
                        //<iframe src="@Url.Action("GetPDF")" width="90%" height="90%">
//</iframe>
                    }, // success
                    error: function (data) {
                        alert("Preview failed");
                        window.location.reload();
                    } // error
                }); // ajax
                
                //return false;
            });

        } // success
    }); // ajax


});