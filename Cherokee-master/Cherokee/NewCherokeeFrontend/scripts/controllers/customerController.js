(function(){

    var app = angular.module("timeKeeper");

    app.controller("customerController", ["$scope", "dataService", '$location',  function($scope, dataService, $location) {

        if($scope.currentUser.role != 'Administrator' || $scope.customerId == 0){
            $location.path('/calendar');
        }
        else {
            console.log($scope.customerId);
            dataService.read("customers", $scope.customerId, function(data) {
                $scope.customer = data;
            })
        }
    }])
}());