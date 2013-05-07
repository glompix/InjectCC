injectcc.injection = (function () {
    'use strict';

    // Public interface that is returned to consumer.  Just need init for this module.
    var _this = {
        init: init
    };

    function init() {
        // constructor code
        $('.injection-diagram').sitepicker({
            'locations': $('.injection-site'),
            'datasource': $('[name$=LocationId]'),
            'readonly': true
        });

        $('.datepicker').datepicker();
    }

    return _this;
})();
