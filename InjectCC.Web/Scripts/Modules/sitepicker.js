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
            'point-radius': 5,
            'locations': $([]),
            'classPrefix': 'canvas-point'
        }, options);

        return this.each(function () {
            var _$canvas = $(this);
            var _paper = new Raphael(_$canvas[0], _$canvas.width(), _$canvas.height());
            var _injectionPoints = [];
            var _selectedPoint;
            
            _$canvas.click(function (e) {
                var x = e.offsetX / $(this).width();
                var y = e.offsetY / $(this).height();
                markSelectedPoint(x, y);
            });

            // all defined locations
            settings.locations.each(function () {
                var location = $(this);
                markExistingPoint(location);
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

            function markExistingPoint($location) {
                var x = $location.find('[name$=InjectionPointX]').val();
                var y = $location.find('[name$=InjectionPointY]').val();
                var name = $location.find('[name$=Name]').val();
                var ordinal = $location.find('[name$=Ordinal]').val();

                var point = generatePoint(x, y, 5);
                var $node = $(point.node);
                $node.attr('title', name);
                $node.attr('class', settings.classPrefix + ' ' + settings.classPrefix + '-' + ordinal);
                $node.tooltip({ 'container': 'body' });
                $node.click(injectionSite_clicked);
                _injectionPoints.push(point);
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
                    node.attr('class', settings.classPrefix + '-selected');
                }
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

        });
    };
})(jQuery);