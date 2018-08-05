(function(){

    var app = angular.module("timeKeeper");

    app.directive("project", [function() {
        return {
            restrict: 'E',
            scope: {
                data: '='
            },
            controller: 'projController as $proj',
            templateUrl: 'views/widgets/project-widget.html'
        }
    }]);
}());