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

                    if (query.location) {
                        queryRows.push(' <a href="/query/' + query.id + '" class="btn btn-success"><i class="icon-globe"></i> ' + query.name + '</a>');
                    }
                    else {
                        queryRows.push(' <a href="/query/' + query.id + '" class="btn btn-info">' + query.name + '</a>');
                    }

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

        if ($('#location').val() == "") {
            $.ajax({
                type: 'POST',
                url: 'http://localhost:1111/query/' + query_data,
                complete: function () {
                    fillQueries();
                }
            });
        }
        else {
            $.ajax({
                type: 'POST',
                url: 'http://localhost:1111/query/' + query_data + '/' + $('#place').val(),
                complete: function () {
                    fillQueries();
                }
            });
        }

        // Clear the input box
        $('#queryTerms').val("");
        $('#location').val("");
        $("#searchLocation").removeAttr("checked");
    }

    $('#queryTerms').keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            sendQuery();
        }
    })

    $('#monitor').click(function (e) {
        //todo: grab the location as well
        e.preventDefault();
        sendQuery();
    });

    $('#editLocation').click(function (e) {
        e.preventDefault();
        $('#myModal').modal('show');
    });

    // Location checkbox used
    $('#searchLocation').change(function (e) {
        e.preventDefault();
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
                    var place = {};
                    place.name = result.full_name;
                    place.latitude = result.bounding_box.coordinates[0][0][0];
                    place.longitude = result.bounding_box.coordinates[0][0][1];
                    place.id = result.id;

                    validLocations.push(place);
                });

                var rendered = [];

                validLocations.map(function (location) {
                    rendered.push(locationTemplate.render(location));
                });

                if (rendered.length != 0) {
                    $('#locationRadios').html(rendered.join(' '));
                }
                else {
                    $('#locationRadios').html('<p>No results found, try again</p>');
                }
            }
        });

        $('#useLocation').click(function () {
            var selectedLocationValue = $('#locationRadios input[name=optionsRadios]:checked');
            $('#place').val(selectedLocationValue.val());
            $('#place').text(selectedLocationValue.attr('data-place'));
        });
    });
});
