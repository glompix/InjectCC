injectcc.medication = (function () {
    'use strict';

    var _$canvas;

    // Public interface that is returned to consumer.  Just need init for this module.
    var _this = {
        init: init
    }

    function init() {
        // constructor code
        console.log('Initializing sitepicker...');
        _$canvas = $('.injection-diagram').sitepicker({
            locations: '.injection-site',
            enabled: false,
        });

        // Sortable injection sites.
        $('.injection-site').parent()
            .sortable()
            .disableSelection();

        // Enable injection site delete button.
        $(document).on('click', '.injection-site .remove-site', function () {
            var $location = $(this).parents('.injection-site');
            var ordinal = $location.find('[name$=Ordinal]').val();
            var $point = _$canvas.find('.canvas-point-' + ordinal);
            if ($point) {
                $point.remove();
            }

            $location.remove();
        });

        // Validation
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

        $('#AddLocationButton').click(function () {
            if (!$('#LocationForm').valid()) {
                return;
            }

            // Synthesize a location object and render it to DOM.
            var timeValue = $('#ValueUntilNextInjectionField').val();
            var timeUnit = $('#UnitUntilNextInjectionField').val();
            var selectedPoint = _$canvas.sitepicker('getMark');
            var location = {
                name: $('#LocationNameField').val(),
                timeValue: timeValue,
                timeUnit: timeUnit,
                minutesUntilNextInjection: getMinutes(timeValue, timeUnit),
                injectionPointX: selectedPoint.x,
                injectionPointY: selectedPoint.y,
                referenceImageUrl: $('#ReferenceImageSelector').val(),
                ordinal: $('.injection-site').length
            };
            var locationCompactTemplate = _.template($('#_LocationCompact').html());
            var newListItem = locationCompactTemplate(location);
            $('.injection-sites').append(newListItem);
            var $location = $('.injection-sites :last-child');

            // Mark the point on the canvas and get rid of the selection marker.
            _$canvas.sitepicker('clearMark');
            _$canvas.sitepicker('refresh');

            // Clear out form.
            $('#LocationNameTextBox').val('');
            $(this).parents('form').removeClass('error');
        });

        $('form').submit(function () {
            $('.injection-site').each(function (i) {
                var ordinal = i;
                $(this).find('[name^="Location"]').each(function () {
                    var name = $(this).attr('name').replace(/{ordinal}/, i);
                    $(this).attr('name', name);
                });
            });
        });
    }

    function getMinutes(timeValue, timeUnit) {
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
    }

    return _this;
})();
