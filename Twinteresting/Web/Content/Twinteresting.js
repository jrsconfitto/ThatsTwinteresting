$(document).ready(function () {
    function sendQuery(queryText) {
        // Post to my querying service
        var query_data = $('#queryTerms').val();

        $.ajax({
            type: 'POST',
            url: 'http://localhost:1111/query/' + query_data,
            success: function (data) {
                console.log(data);
            },
        });
    }

    $('#queryTerms').keypress(function(e) {
        if (e.which == 13) {
          e.preventDefault();
          sendQuery();
        }
    })

    $('#search').click(function () {
        sendQuery();
    });
});