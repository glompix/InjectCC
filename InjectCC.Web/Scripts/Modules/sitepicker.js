/**
* pointpicker.js
* Displays an overlay on an element on which 2D points can be selected
* or viewed.
*
* To initialize a sitepicker, call $('#picker').sitepicker([ options ]);
* To get the { x, y } of an existing sitepicker, call $('#picker').sitepicker('getSelectedPoint').
* 
* @property object self The module itself
* @property object $ The jQuery object
*/
(function ($) {
    'use strict';

    // No time to refactor this into something reasonable. For now, just global it
    // up and assume one sitepicker per page. (which is reasonable)
    var _settings;
    var _paper;
    var _$canvas;

    jQuery.fn.sitepicker = function (options) {
        _$canvas = $(this);

        if (options === 'refresh') {
            _$canvas.find('.' + _settings.classPrefix).remove();
            $(_settings.locations).each(function () {
                var $location = $(this);
                displayLocation($location);
            });
        }
        else if (options === 'clearMark') {
            _$canvas.find('.' + _settings.classPrefix + '-marked').remove();
        }
        else if (options === 'getMark') {
            var $point = $(this).find('.' + _settings.classPrefix + '-marked');
            console.log('marked point', $point);
            return {
                x: parseFloat($point.attr('data-rel-x')),
                y: parseFloat($point.attr('data-rel-y'))
            };  
        }
        else {
            _settings = $.extend({
                'enabled': true,
                'readonly': false,
                'pointRadius': '3%',
                'locations': $([]),
                'datasource': $([]),
                'classPrefix': 'canvas-point',
                'aspectRatio': 0.75
            }, options);

            if (!_settings.readonly) {
                _$canvas.click(function (e) {
                    var x = e.offsetX / $(this).width();
                    var y = e.offsetY / $(this).height();
                    markPoint(x, y);
                });
            }
            else {
                _$canvas.addClass('readonly');
            }

            // Maintain aspect ratio
            $(window).resize(function () {
                _$canvas.height(_$canvas.width() * _settings.aspectRatio);
                _paper.setSize(_$canvas.width(), _$canvas.height());
                _$canvas.find('.' + _settings.classPrefix).each(function () {
                    var $point = $(this);
                    var relx = parseFloat($point.attr('data-rel-x'));
                    var rely = parseFloat($point.attr('data-rel-y'));
                    $point.attr('cx', relx * _$canvas.width());
                    $point.attr('cy', rely * _$canvas.height());
                    $point.attr('r', calculateRadius(_settings.pointRadius))
                });
            });

            _$canvas.height(_$canvas.width() * _settings.aspectRatio);
            _paper = new Raphael(_$canvas[0], _$canvas.width(), _$canvas.height());

            if (_settings.enabled) {
                _$canvas.addClass('enabled');
            }

            // all defined locations
            $(_settings.locations).each(function () {
                var $location = $(this);
                displayLocation($location);
            });

            if (_settings.datasource.val()) {
                var $point = _$canvas.find('[data-id="' + _settings.datasource.val() + '"]');
                selectLocation($point);
            }             
        }

        return _$canvas;

        /// <summary>
        /// Marks the given [x, y] as the selected point.  If falsey values given, the existing
        /// point is just cleared.
        /// </summary>
        function markPoint(x, y) {
            _$canvas.find('.' + _settings.classPrefix + '-marked').remove();

            if (x && y) {
                var markedPoint = generatePoint(x, y, _settings.pointRadius);
                var node = $(markedPoint.node);
                node.attr('class', _settings.classPrefix + '-marked');
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
            size = calculateRadius(size);
            console.log('relative', x, y)
            var realX = x * _$canvas.width();
            var realY = y * _$canvas.height();
            console.log('new point at ', realX, realY, size);
            var circle = _paper.circle(realX, realY, size);
            circle.attr('stroke', '#000000');
            $(circle.node).attr('data-rel-x', x);
            $(circle.node).attr('data-rel-y', y);
            return circle;
        }

        function displayLocation($location) {
            console.log('display', $location);
            var x = $location.find('[name$=InjectionPointX]').val();
            var y = $location.find('[name$=InjectionPointY]').val();
            var name = $location.find('[name$=Name]').val();
            var ordinal = $location.find('[name$=Ordinal]').val();
            var id = $location.find('[name$=LocationId]').val();

            var point = generatePoint(x, y, _settings.pointRadius);
            var $node = $(point.node);
            $node.attr('title', name);
            $node.attr('class', _settings.classPrefix + ' ' + _settings.classPrefix + '-' + ordinal);
            $node.attr('data-id', id);
            $node.tooltip({ 'container': 'body' });
            $node.click(injectionSite_clicked);
        }

        function injectionSite_clicked() {
            if (_settings.enabled) {
                selectLocation($(this));
            }
        }

        // I think the reason for this funky class attribute manipulation is because we're working with SVG
        // elements, not HTML. Can't remember for sure.
        function removeClass($point, className) {
            var oldClass = $point.attr('class') || '';
            var newClass = $.trim(oldClass.replace(className, ''));
            $point.attr('class', newClass);
        }

        function selectLocation($point) {
            removeClass(_$canvas.find('.' + _settings.classPrefix + '-selected'), '.' + _settings.classPrefix + '-selected');
            $point.attr('class', $point.attr('class') + ' ' + _settings.classPrefix + '-selected');
            var id = $point.attr('data-id');
            _settings.datasource.val(id);
        }

    };
})(jQuery);