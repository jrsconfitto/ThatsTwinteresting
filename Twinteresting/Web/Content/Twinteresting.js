$(document).ready(function () {
    $('#search').click(function () {
        // Post to my querying service
        var query_data = $('#queryTerms').val();

        $.ajax({
            type: 'POST',
            url: 'http://localhost:1111/query/' + query_data,
            success: function (data) {
                console.log(data);
            },
        });
    });
});