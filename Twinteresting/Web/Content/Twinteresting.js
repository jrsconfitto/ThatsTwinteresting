function fillQueries() {
    // For delivering query results
    $.ajax({
        type: 'GET',
        url: '/results',
        success: function (data) {
            // Let's just do this the dumb way!
            var queryRows = [];

            // Build up a js object that mustache will like
            while (data.length) {
                queryRows.push('<div class="row" style="margin-bottom: 5px;">');

                data.splice(0, 4).forEach(function (query) {
                    queryRows.push('<div class="span3">');
                    queryRows.push(' <a href="/query/' + query.id + '" class="btn btn-info">' + query.name + '</a>');
                    queryRows.push('</div>');
                });

                queryRows.push('</div>');
            }

            $('#queryResults').html(queryRows.join(' '));
        }
    });
}

// Once everything is ready
$(document).ready(function () {
    // Compile a template
    var locationTemplate = Hogan.compile($('#locationTemplate').html());

    // Fill the queries in on the initial page load
    fillQueries();

    // For making new queries
    function sendQuery(queryText) {
        // Post to my querying service
        var query_data = $('#queryTerms').val();

        $.ajax({
            type: 'POST',
            url: 'http://localhost:1111/query/' + query_data,
            complete: function () {
                fillQueries();
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

    $('#monitor').click(function () {
        //todo: grab the location as well
        e.preventDefault();
        sendQuery();
    });

    // Location checkbox used
    $('#searchLocation').change(function () {
        $('#myModal').modal('show');
    });

    // Search for locations relevant to the user's search
    $('#findLocations').click(function () {
        var location = $('#address').val();

        // Query Twitter's API for viable locations
        $.ajax({
            url: '/locations/' + location,
            method: "GET",
            success: function (data) {
                var validLocations = [];

                data.forEach(function (result) {
                    var city = {};
                    city.name = result.full_name;
                    city.latitude = result.bounding_box.coordinates[0][0][0];
                    city.longitude = result.bounding_box.coordinates[0][0][1];

                    validLocations.push(city);
                });

                var rendered = [];

                validLocations.map(function (location) {
                    rendered.push(locationTemplate.render(location));
                });

                $('#locationRadios').html(rendered.join(' '));
            }
        });

        $('#useLocation').click(function () {
            var selectedLocationValue = $('#locationRadios input[name=optionsRadios]:checked').val();
            $('#location').val(selectedLocationValue);
        });
    });
});
