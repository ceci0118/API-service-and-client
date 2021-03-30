$(document).ready(function(){

    var currentRest = null;

    $.ajax({
        type: 'GET',
        url: "http://localhost:5000/restaurantreview",
        dataType: 'json',
        success: function(res){
            $.each(res, function (index, value){
                $('#drpRestaurant').append("<option value=" + value.id + ">" + value.name + "</option>")
            })
        },
        error: function(event, request, settings){
            console.log(settings)
        }
    })


    $('#drpRestaurant').change(function (){

        $('#lblConfirmation').css('visibility', 'hidden');

        restaurantId = parseInt($('#drpRestaurant').val());

        $.ajax({
            type: 'GET',
            url: "http://localhost:5000/restaurantreview/" + restaurantId,
            dataType: 'json',
            success: function (res){

                currentRest = res;
                $('#txtStreetAddress').val(res.address.street)
                $('#txtCity').val(res.address.city)
                $('#txtProvinceState').val(res.address.provstate)
                $('#txtPostalZipCode').val(res.address.postalzipcode)
                $('#txtSummary').val(res.summary)

                var min = res.rating.minRating
                var max = res.rating.maxRating
                var i = min
                $('#drpRating').empty();

                while (i <= max){
                    var sel = (i == res.rating.currentRating) ? "selected='selected'" : "";
                    $('#drpRating').append("<option value=" + i + ' ' + sel + " > " + i + " </option>")
                    i ++;

                }
            },
            error: function(event, request, settings){
                console.log(settings)
            }
        })

    })

    $('#btnSave').click(function (){
        
        currentRest.address.street = $('#txtStreetAddress').val();
        currentRest.address.city = $('#txtCity').val();
        currentRest.address.provstate = $('#txtProvinceState').val();
        currentRest.address.postalzipCode = $('#txtPostalZipCode').val();
        currentRest.summary = $('#txtSummary').val();
        currentRest.rating.currentRating = parseInt($('#drpRating').val());

        $.ajax({
            type: 'PUT',
            contentType: "application/json",
            url: "http://localhost:5000/restaurantreview",
            data: JSON.stringify(currentRest),
            success: function (res){
                $('#lblConfirmation')
                    .css('visibility', 'visible')
                    .text("Restaurant has been updated!");
            },
            error: function (xhr, status, error){
                var errorMessage = xhr.status + ': ' + xhr.statusText
                console.log('Error - ' + errorMessage);
            }
        })
    })
})
