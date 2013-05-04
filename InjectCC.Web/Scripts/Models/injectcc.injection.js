injectcc.injection = (function () {
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
    };

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

        $('[name="Injection.Date.Date"]').datepicker();

        // all defined locations
        $('.injection-site').each(function () {
            var location = $(this);
            markExistingPoint(location);
        });
    }

    function selectPoint($point) {
        console.log($point);
        $point.addClass('canvas-point-selected');
        $point.siblings('circle').removeClass('canvas-point-selected');
        var id = $point.attr('data-id');
        $('[name$=LocationId]').val(id);
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
            var id = $location.find('[name$=Id]').val();

            var point = generatePoint(x, y, 5);
            var $node = $(point.node);
            $node.attr('title', name);
            $node.attr('class', 'canvas-point canvas-point-' + ordinal);
            $node.attr('data-id', id);
            $node.tooltip({ 'container': 'body' });
            $node.click(function () {
                selectPoint($(this));
            });
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
