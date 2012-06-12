$(document).ready(function () {
    // For delivering query results
    $.ajax({
        type: 'GET',
        url: '/results',
        success: function (data) {
            // Let's just do this the dumb way!
            var queryRows = [];

            // Build up a js object that mustache will like
            while (data.length) {
                queryRows.push('<div class="row">');

                data.splice(0, 4).forEach(function (query) {
                    queryRows.push('<div class="span3">');
                    queryRows.push(' <a href="/query/' + query.id +  '" class="btn btn-info">' + query.name + '</a>');
                    queryRows.push('</div>');
                });

                queryRows.push('</div>');
            }

            $('#queryResults').html(queryRows.join(' '));
        }
    });


    // For making new queries
    function sendQuery(queryText) {
        // Post to my querying service
        var query_data = $('#queryTerms').val();

        $.ajax({
            type: 'POST',
            url: 'http://localhost:1111/query/' + query_data,
            success: function (data) {
                console.log(data);
            }
        });

        // Clear the input box
        $('#queryTerms').val("");
    }

    $('#queryTerms').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            sendQuery();
        }
    })

    $('#search').click(function () {
        sendQuery();
    });
});
