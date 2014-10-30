$(document).ready(function () {
	$.ajax({
		url: "/FPIM/GetCourses",
		type: 'Get',
		data: { myId: 'Leopold', semester:  'Fall 2014' },
		success: function (data) {
			$.each(data, function (i, value) {
				var id = 'draggable' + value.FacultyProfileModuleId;
				var coursesHTML = '<li id="' + value.CourseID + '">' +
                    '<h3>&nbsp' + value.CourseID + ' ' + value.GeneralCourseTitle + '</h3>' +
                    '</li>'
				$('#courses').append(coursesHTML);
			}); // each
		} // success
	}); // ajax

});