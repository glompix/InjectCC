(function () {
    'use strict';

    injectcc.models.Medication = Backbone.Model.extend({
        defaults: {
            name: ""
        },
        initialize: function() {
            // constructor code
            $('.injection-site').parent()
                .sortable()
                .disableSelection();

            $(document).on('click', '.injection-site .remove-site', function () {
                $(this).parents('.injection-site').remove();
            });

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

            var _selectedX = undefined;
            var _selectedY = undefined;
            var _highlightedX = undefined;
            var _highlightedY = undefined;
            var refreshCanvas = function () {
                var context = $('#ReferenceCanvas')[0].getContext('2d');
                drawSelectedImage(context);
                // selected locations
                if (_selectedX !== undefined && _selectedY !== undefined) {
                    drawDot(context, _selectedX, _selectedY, 5, '#222222', true);
                }
                // highlighted locations
                if (_highlightedX !== undefined && _highlightedY !== undefined) {
                    drawDot(context, _highlightedX, _highlightedY, 5, '#cc1111', true);
                }
                // all defined locations
                $('.injection-site').each(function () {
                    var location = $(this);
                    var refImage = $(this).find('[name$="ReferenceImageUrl"]').val();

                    // Don't draw dot if defined on different ref image.
                    if (refImage === $('#ReferenceImageSelector').val()) {
                        var x = location.find('[name$=InjectionPointX]').val();
                        var y = location.find('[name$=InjectionPointY]').val();
                        drawDot(context, x, y, 5, '#cc1111', false);
                    }
                });
            };

            $('#ReferenceCanvas').click(function (e) {
                _selectedX = e.offsetX / $(this).width();
                _selectedY = e.offsetY / $(this).height();

                refreshCanvas();
            });

            $('#ReferenceImageSelector').change(function () {
                _selectedX = undefined;
                _selectedY = undefined;
                _image = undefined;
                refreshCanvas();
            });

            $('#AddLocationButton').click(function () {
                if (!$('#LocationForm').valid()) {
                    return;
                }

                var timeValue = $('#ValueUntilNextInjectionField').val();
                var timeUnit = $('#UnitUntilNextInjectionField').val();

                var location = {
                    name: $('#LocationNameField').val(),
                    timeValue: timeValue,
                    timeUnit: timeUnit,
                    minutesUntilNextInjection: getMinutes(timeValue, timeUnit),
                    injectionPointX: _selectedX,
                    injectionPointY: _selectedY,
                    referenceImageUrl: $('#ReferenceImageSelector').val(),
                    ordinal: $('.injection-site').length
                };

                var locationCompactTemplate = _.template($('#_LocationCompact').html());
                var newListItem = locationCompactTemplate(location);

                $('.injection-site').parent().append(newListItem);
                $('#LocationNameTextBox').val('');
                $(this).parents('form').removeClass('error');
                _selectedX = undefined;
                _selectedY = undefined;
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

            $('.injection-site').hover(function () {
                var location = $(this);
                _highlightedX = location.find('[name$=".InjectionPointX"]').val();
                _highlightedY = location.find('[name$=".InjectionPointY"]').val();
                refreshCanvas();
            }, function () {
                _highlightedX = undefined;
                _highlightedY = undefined;
                refreshCanvas();
            });

            refreshCanvas();
        },
    });

    function drawImage(image, context) {
        var canvas = $('#ReferenceCanvas')[0];

        var x, y, width, height;
        if (image.height < image.width) {
            width = canvas.width;
            height = canvas.width * (image.height / image.width);
            x = 0;
            y = (width - height) / -2.0;
        }
        else {
            width = canvas.height * (image.width / image.height);
            height = canvas.height;
            x = (height - width) / -2.0;
            y = 0;
        }
        if (context === undefined) {
            context = canvas.getContext('2d');
        }
        context.drawImage(image, x, y, width, height);
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

    function drawDot(context, x, y, size, color, filled) {
        if (size === undefined) {
            size = 5.0;
        }
        if (color === undefined) {
            color = '#111111';
        }
        if (filled === undefined) {
            filled = true;
        }

        var offset = size / 2.0;
        context.beginPath();
        context.arc(x * $('#ReferenceCanvas').width() - offset, y * $('#ReferenceCanvas').height() - offset, size, 0, Math.PI * 2, true);
        context.closePath();
        if (filled) {
            context.fillStyle = color;
            context.fill();
        }
        else {
            context.lineWidth = offset;
            context.strokeStyle = color;
            context.stroke();
        }
    }

    var _image;
    function drawSelectedImage(imageSrc, context) {
        if (_image === undefined) {
            _image = new Image();
            _image.onload = function () {
                drawImage(_image);
            };
            _image.src = $('#ReferenceImageSelector').val();
        }
        else {
            drawImage(_image);
        }
    }
})();
