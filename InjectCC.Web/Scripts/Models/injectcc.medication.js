(function () {
    'use strict';
    
    // These are basically private statics, works but figure out better org
    var _paper;
    var _selectedPoint;
    var _injectionPoints = [];
    var $canvas;
    var $imageSelector;

    injectcc.models.Medication = Backbone.Model.extend({
        defaults: {
            name: ""
        },
        initialize: function() {
            // constructor code
            $canvas = $('#ReferenceCanvas');
            $imageSelector = $('#ReferenceImageSelector');
            _paper = new Raphael($canvas[0], $canvas.width(), $canvas.height());
            
            // Draw selected injection site diagram when changed, as well as on load.
            $imageSelector.change(function () {
                drawSelectedImage();
            });
            drawSelectedImage();

            // Sortable injection sites.
            $('.injection-site').parent()
                .sortable()
                .disableSelection();

            // Enable injection site delete button.
            $(document).on('click', '.injection-site .remove-site', function () {
                $(this).parents('.injection-site').remove();
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

            $canvas.click(function (e) {
                if (_selectedPoint) {
                    _selectedPoint.remove();
                }

                var x = e.offsetX / $(this).width();
                var y = e.offsetY / $(this).height();
                _selectedPoint = generatePoint(x, y, 5);
                var node = $(_selectedPoint.node);
                node.attr('class', 'canvas-point-selected')
            });

            // all defined locations
            $('.injection-site').each(function () {
                var location = $(this);
                var refImage = location.find('[name$="ReferenceImageUrl"]').val();

                // Don't draw dot if defined on different ref image.
                if (refImage === $imageSelector.val()) {
                    var x = location.find('[name$=InjectionPointX]').val();
                    var y = location.find('[name$=InjectionPointY]').val();
                    var name = location.find('[name$=Name]').val();

                    var point = generatePoint(x, y, 5);
                    var node = $(point.node);
                    node.attr('title', name);
                    node.attr('class', 'canvas-point');
                    node.tooltip({ 'container': 'body' });
                    node.click(injectionSite_clicked);
                    _injectionPoints.push(point);
                }
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
                    injectionPointX: _selectedPoint.attr('cx'),
                    injectionPointY: _selectedPoint.attr('cy'),
                    referenceImageUrl: $('#ReferenceImageSelector').val(),
                    ordinal: $('.injection-site').length
                };

                var locationCompactTemplate = _.template($('#_LocationCompact').html());
                var newListItem = locationCompactTemplate(location);

                $('.injection-site').parent().append(newListItem);
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
        },
    });

    function injectionSite_clicked() {
        var point;
        for (var i = 0; i < _injectionPoints.length; i++) {
            if (_injectionPoints[i].attr('data-id') === $(this).attr('data-id')) {
                point = _injectionPoints[i];
                break;
            }
        }

        if (point)
            console.log(point);
        else
            console.log('whoops');        
    }

    function drawSelectedImage() {
        $canvas.removeClass(function (index, css) {
            return (css.match(/\breference-image-\S+/g) || []).join(' ');
        });
        var imageClass = 'reference-image-' + $.trim($imageSelector.text());
        $canvas.addClass(imageClass);
    };

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

    function generatePoint(x, y, size, color) {
        if (size === undefined) {
            size = 5.0;
        }
        if (color === undefined) {
            color = '#111111';
        }

        var realX = $canvas.width() * x;
        var realY = $canvas.height() * y;

        var circle = _paper.circle(realX, realY, size);
        // circle.attr('fill', color);
        circle.attr('stroke', '#000000');
        return circle;
    }
})();
