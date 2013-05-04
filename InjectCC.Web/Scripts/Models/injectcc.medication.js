﻿injectcc.medication = (function () {
    'use strict';

    // These are basically private statics, works but figure out better org maybe
    var _paper;
    var _selectedPoint;
    var _injectionPoints = [];
    var _$canvas;
    var _$imageSelector;

    // Functional classes
    var _injectionPointPrefix = ".canvas-point"; // or .canvas-point-{ordinal} to id a specific point

    // Public interface that is returned to consumer.  Just need init for this module.
    var _this = {
        init: init
    }

    function init() {
        // constructor code
        _$canvas = $('#ReferenceCanvas');
        _$imageSelector = $('#ReferenceImageSelector');
        _paper = new Raphael(_$canvas[0], _$canvas.width(), _$canvas.height());

        // Draw selected injection site diagram when changed, as well as on load.
        _$imageSelector.change(function () {
            drawSelectedImage();
        });
        drawSelectedImage();

        // Sortable injection sites.
        $('.injection-site').parent()
            .sortable()
            .disableSelection();

        // Enable injection site delete button.
        $(document).on('click', '.injection-site .remove-site', function () {
            var $location = $(this).parents('.injection-site');
            var ordinal = $location.find('[name$=Ordinal]').val();
            var $point = _$canvas.find(_injectionPointPrefix + '-' + ordinal);
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

        _$canvas.click(function (e) {
            var x = e.offsetX / $(this).width();
            var y = e.offsetY / $(this).height();
            markSelectedPoint(x, y);
        });

        // all defined locations
        $('.injection-site').each(function () {
            var location = $(this);
            markExistingPoint(location);
        });

        $('#AddLocationButton').click(function () {
            if (!$('#LocationForm').valid()) {
                return;
            }

            // Synthesize a location object and render it to DOM.
            var timeValue = $('#ValueUntilNextInjectionField').val();
            var timeUnit = $('#UnitUntilNextInjectionField').val();
            var location = {
                name: $('#LocationNameField').val(),
                timeValue: timeValue,
                timeUnit: timeUnit,
                minutesUntilNextInjection: getMinutes(timeValue, timeUnit),
                injectionPointX: _selectedPoint.attr('cx') / _$canvas.width(),
                injectionPointY: _selectedPoint.attr('cy') / _$canvas.height(),
                referenceImageUrl: $('#ReferenceImageSelector').val(),
                ordinal: $('.injection-site').length
            };
            var locationCompactTemplate = _.template($('#_LocationCompact').html());
            var newListItem = locationCompactTemplate(location);
            $('.injection-sites').append(newListItem);
            var $location = $('.injection-sites :last-child');

            // Mark the point on the canvas and get rid of the selection marker.
            markSelectedPoint();
            markExistingPoint($location);

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

    /// <summary>
    /// Marks the given [x, y] as the selected point.  If falsey values given, the existing
    /// point is just cleared.
    /// </summary>
    function markSelectedPoint(x, y) {
        if (_selectedPoint) {
            _selectedPoint.remove();
        }

        if (x && y) {
            _selectedPoint = generatePoint(x, y, 5);
            var node = $(_selectedPoint.node);
            node.attr('class', 'canvas-point-selected');
        }
    }

    function markExistingPoint($location) {
        var refImage = $location.find('[name$="ReferenceImageUrl"]').val();

        // Don't draw dot if defined on different ref image.
        if (refImage === _$imageSelector.val()) {
            var x = $location.find('[name$=InjectionPointX]').val();
            var y = $location.find('[name$=InjectionPointY]').val();
            var name = $location.find('[name$=Name]').val();
            var ordinal = $location.find('[name$=Ordinal]').val();

            var point = generatePoint(x, y, 5);
            var $node = $(point.node);
            $node.attr('title', name);
            $node.attr('class', 'canvas-point canvas-point-' + ordinal);
            $node.tooltip({ 'container': 'body' });
            $node.click(injectionSite_clicked);
            _injectionPoints.push(point);
        }
    }

    function drawSelectedImage() {
        _$canvas.removeClass(function (index, css) {
            return (css.match(/\breference-image-\S+/g) || []).join(' ');
        });
        var imageClass = 'reference-image-' + $.trim(_$imageSelector.text());
        _$canvas.addClass(imageClass);
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

    function generatePoint(x, y, size, color) {
        if (size === undefined) {
            size = 5.0;
        }
        if (color === undefined) {
            color = '#111111';
        }

        var realX = _$canvas.width() * x;
        var realY = _$canvas.height() * y;

        var circle = _paper.circle(realX, realY, size);
        // circle.attr('fill', color);
        circle.attr('stroke', '#000000');
        return circle;
    }

    return _this;
})();
