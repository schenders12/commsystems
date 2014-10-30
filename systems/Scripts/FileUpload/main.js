/*
 * jQuery File Upload Plugin JS Example 6.5.1
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/*jslint nomen: true, unparam: true, regexp: true */
/*global $, window, document */

$(function () {
    'use strict';
    var siteRoot;
    function SetSiteRoot(path){
       // $.ajax({
       //     type: "POST",
       //     url: ajaxPostUrl
        // });
        siteRoot = path;
    }

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload();

    $('#fileupload').fileupload('option', {
            maxFileSize: 500000000,
            resizeMaxWidth: 1920,
            resizeMaxHeight: 1200
    });

    // Load existing files:
    var profileId = $('#profileId').attr('value');
    var url = '@Url.Content("~/Upload/UploadHandler.ashx")';
    var userApiPath = '@Url.Content("~/Upload/UploadHandler.ashx")';
    if (ROOT == null) {
       // var ROOT = '@Url.Content("~/")';
    }
    var url = ROOT + 'Upload/UploadHandler.ashx';
   // var url = ROOT + 'Upload/UploadHandler.ashx"';
    $.ajax({
        url: url,
        type:  'GET',
        data: { profileId: profileId },
        dataType: 'json',
        context: $('#fileupload')[0]
    }).done(function (result) {
        $(this).fileupload('option', 'done').call(this, null, { result: result });
    });
});
