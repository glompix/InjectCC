/**
* pointpicker.js
* Displays an overlay on an element on which 2D points can be selected
* or viewed.
* 
* @property object self The module itself
* @property object $ The jQuery object
*/
(function ($) {
    'use strict';

    jQuery.fn.sitepicker = function (options) {

        var settings = $.extend({
            'enabled': true,
            'readonly': false,
            'pointRadius': '3%',
            'locations': $([]),
            'datasource': $([]),
            'classPrefix': 'canvas-point',
            'aspectRatio': 0.75
        }, options);

        return this.each(function () {
            var _$canvas = $(this);
            var _injectionPoints = [];
            var _selectedPoint;

            if (!settings.readonly) {
                _$canvas.click(function (e) {
                    var x = e.offsetX / $(this).width();
                    var y = e.offsetY / $(this).height();
                    markSelectedPoint(x, y);
                });
            }
            else {
                _$canvas.addClass('readonly');
            }

            // Maintain 4:3 aspect ratio
            $(window).resize(function () {
                _$canvas.height(_$canvas.width() * settings.aspectRatio);
                _paper.setSize(_$canvas.width(), _$canvas.height());
                _$canvas.find('.canvas-point').each(function () {
                    var $point = $(this);
                    var relx = parseFloat($point.attr('data-rel-x'));
                    var rely = parseFloat($point.attr('data-rel-y'));
                    $point.attr('cx', relx * _$canvas.width());
                    $point.attr('cy', rely * _$canvas.height());
                    $point.attr('r', calculateRadius(settings.pointRadius))
                });
            });
            _$canvas.height(_$canvas.width() * settings.aspectRatio);
            var _paper = new Raphael(_$canvas[0], _$canvas.width(), _$canvas.height());

            if (settings.enabled) {
                _$canvas.addClass('enabled');
            }

            // all defined locations
            settings.locations.each(function () {
                var location = $(this);
                markExistingPoint(location);
            });

            console.log(settings.datasource.val());
            if (settings.datasource.val()) {
                var $point = _$canvas.find('[data-id="' + settings.datasource.val() + '"]');
                selectPoint($point);
            }

            function injectionSite_clicked() {
                if (settings.enabled) {
                    selectPoint($(this));
                }
            }

            function markExistingPoint($location) {
                var x = $location.find('[name$=InjectionPointX]').val();
                var y = $location.find('[name$=InjectionPointY]').val();
                var name = $location.find('[name$=Name]').val();
                var ordinal = $location.find('[name$=Ordinal]').val();
                var id = $location.find('[name$=LocationId]').val();

                var point = generatePoint(x, y, settings.pointRadius);
                var $node = $(point.node);
                $node.attr('title', name);
                $node.attr('class', settings.classPrefix + ' ' + settings.classPrefix + '-' + ordinal);
                $node.attr('data-id', id);
                $node.tooltip({ 'container': 'body' });
                $node.click(injectionSite_clicked);
                _injectionPoints.push(point);
            }

            function removeClass($point, className) {
                var oldClass = $point.attr('class') || '';
                var newClass = $.trim(oldClass.replace(className, ''));
                $point.attr('class', newClass);
            }

            function selectPoint($point) {
                removeClass(_$canvas.find('.canvas-point-selected'), 'canvas-point-selected');
                $point.attr('class', $point.attr('class') + ' canvas-point-selected');
                var id = $point.attr('data-id');
                settings.datasource.val(id);
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
                    _selectedPoint = generatePoint(x, y, settings.pointRadius);
                    var node = $(_selectedPoint.node);
                    node.attr('class', settings.classPrefix + '-selected');
                }
            }

            function calculateRadius(size) {
                if (size === undefined) {
                    size = 5.0;
                }
                else if (typeof (size) === 'string') {
                    var pctRegex = /(\d+)%/g;
                    var pctStr = pctRegex.exec(size)[0];
                    var pct = parseFloat(pctStr);
                    size = _$canvas.height() * (pct / 100.0);
                }
                return size;
            }

            function generatePoint(x, y, size, color) {
                if (color === undefined) {
                    color = '#111111';
                }
                size = calculateRadius(size);

                var realX = _$canvas.width() * x;
                var realY = _$canvas.height() * y;

                var circle = _paper.circle(realX, realY, size);
                circle.attr('stroke', '#000000');
                $(circle.node).attr('data-rel-x', x);
                $(circle.node).attr('data-rel-y', y);
                return circle;
            }

        });
    };
})(jQuery);