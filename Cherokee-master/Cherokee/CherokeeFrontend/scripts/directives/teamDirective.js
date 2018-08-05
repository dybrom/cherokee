(function(){

    var app = angular.module("timeKeeper");

    app.directive("team", [function() {
        return {
            restrict: 'E',
            scope: {
                data: '=team'
            },
            controller: 'tmController as $tm',
            templateUrl: 'views/widgets/team-widget.html'
        }
    }]);
}());