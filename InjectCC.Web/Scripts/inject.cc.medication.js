$(function () {
    $('#LocationList')
        .sortable()
        .disableSelection();

    $(document).on('click', '#LocationList li .close', function () {
        $(this).parents('li').remove();
    });

    var getMinutes = function (timeValue, timeUnit) {
        var minutesUntilNextInjection = timeValue;
        if (minutesUntilNextInjection === NaN) {
            return NaN;
        }
        if (timeUnit === 'days') {
            minutesUntilNextInjection *= 24;
            timeUnit = 'hours';
        }
        if (timeUnit === 'hours') {
            minutesUntilNextInjection *= 60;
            timeUnit = 'minutes';
        }
        if (timeUnit === 'minutes') {
            minutesUntilNextInjection = Math.round(minutesUntilNextInjection);
        }

        return minutesUntilNextInjection;
    };

    var locationValidator = $('#LocationForm').validate({
        messages: {
            timeUntilNext: {
                required: true,
                number: true,
                min: 0
            },
            locationName: {
                required: true
            }
        }
    });

    #('#ReferenceImageSelector').change(function () {
        
    });

    $('#LocationImage').click(function () {
        if (!$('#LocationForm').valid()) {
            return;
        }

        var name = $('#LocationNameField').val();
        var timeValue = $('#ValueUntilNextInjectionField').val();
        var timeUnit = $('#UnitUntilNextInjectionField').val();
        var minutesUntilNextInjection = getMinutes(timeValue, timeUnit);

        var ordinal = $('#LocationList li').length;
        var newListItem = $('<li><div class="well well-small"><button type="button" class="close">×</button><b>' + name + '</b><p>Next injection - ' + +'</p></div></li>');
        newListItem.append('<input type="hidden" name="Locations[' + ordinal + '].Name" value="' + name + '" />');
        newListItem.append('<input type="hidden" name="Locations[' + ordinal + '].Ordinal" value="' + ordinal + '" />');
        newListItem.append('<input type="hidden" name="Locations[' + ordinal + '].InjectionPointX" value="0" />');
        newListItem.append('<input type="hidden" name="Locations[' + ordinal + '].InjectionPointY" value="0" />');
        newListItem.append('<input type="hidden" name="Locations[' + ordinal + '].ReferenceImageUrl" value="goatse.jpg" />');
        newListItem.append('<input type="hidden" name="Locations[' + ordinal + '].MinutesUntilNextInjection" value="' + minutesUntilNextInjection + '" />');
        $('#LocationList').append(newListItem);
        $('#LocationNameTextBox').val('');
        $(this).parents('form').removeClass('error');
    });
});