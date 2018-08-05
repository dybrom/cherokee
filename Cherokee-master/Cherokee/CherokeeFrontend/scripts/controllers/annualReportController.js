(function(){
    var app = angular.module("timeKeeper");

    app.controller("annualReportController", ['$scope', 'dataService', 'timeConfig', '$location',
                function($scope,dataService, timeConfig, $location){
                    if($scope.currentUser.role!= 'Administrator'){
                        $location.path('/calendar');
                    }


        else {
                        $scope.months = timeConfig.months;
                        $scope.showing = false;
            $scope.listReport = function () {
                if ($scope.year == undefined) {
                    $scope.showing = false;
                    window.alert("Choose year!");
                }
                else {
                    annualReport($scope.year);
                    $scope.showing = true;
                }
            }

            function annualReport(year) {
                var url = 'reports/annual/';
                if (year != 'undefined') url += year;
                dataService.list(url, function (data) {

                    $scope.report = data;
                    $scope.year = data.year;
                })
            }

        }


    }]);
}());