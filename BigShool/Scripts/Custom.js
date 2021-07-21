window.onload = function () {
    $('button.js-tongle-attend').click(function (Event) {
        let ThisButton = $(Event.target);
        $.post('/api/attendances', { Id: ThisButton.attr('data-course-id') })
            .done(function (Result) {
                console.log(Result == 'Removed');
                if (Result == 'Removed') {
                    alert("Remove Success.!");
                    ThisButton.removeClass('btn-info').addClass('btn-primary').removeClass('js-tongle-attend').text("Going");
                }
                else {
                    alert("Success.!");
                    ThisButton.removeClass('btn-primary').addClass('btn-info').removeClass('js-tongle-attend').text("On Going");
                }
            })
            .fail(function () { alert("Something Wrong.!"); })
    });

    $('button.js-tongle-flow').click(function (Event) {
        let ThisButton = $(Event.target);
        $.post('/api/followings', { FolloweeId: ThisButton.attr('data-course-id') })
            .done(function (Result) {
                console.log(Result == 'Removed');
                if (Result == 'Removed') {
                    alert("Unfollow Success.!");
                    ThisButton.removeClass('btn-info').addClass('btn-primary').removeClass('js-tongle-attend').text("Follow");
                }
                else {
                    alert("Success.!");
                    ThisButton.removeClass('btn-primary').addClass('btn-info').removeClass('js-tongle-attend').text("On Following");
                }
            })
            .fail(function () { alert("Something Wrong.!"); })
    });

    $('#DatePick').daterangepicker(
        {
            "singleDatePicker": true,
            "startDate": moment(),
            "minDate": moment(),
            "endDate": moment().endOf('year'),
            "opens": "center"
        });
}