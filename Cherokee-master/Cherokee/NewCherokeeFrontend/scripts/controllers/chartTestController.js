(function(){

    var app = angular.module("timeKeeper");

    app.controller("chartTestController", ["$scope",  function($scope) {
        $scope.message = "wow";


        $scope.labels = ["Download Sales", "In-Store Sales", "Mail-Order Sales"];
        $scope.data = [300, 500, 100];


        }]);
}());