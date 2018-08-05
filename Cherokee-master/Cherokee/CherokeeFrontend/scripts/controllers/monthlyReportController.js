(function(){
    var app = angular.module("timeKeeper");

    app.controller("monthlyReportController", ['$scope', 'dataService', 'timeConfig','$location',
                function($scope,dataService,timeConfig,$location){

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
                            else if ($scope.month == undefined) {
                                $scope.showing = false;
                                window.alert("Choose month!");
                            }
                            else {
                                monthlyReport($scope.year, $scope.month);
                                $scope.showing = true;
                            }
                        }

                        function monthlyReport(year, month) {
                            var url = 'reports/monthly/';
                            if (year != 'undefined') url += year;
                            if (month != 'undefined') url += "/" + month;
                            dataService.list(url, function (data) {

                                $scope.report = data;
                                $scope.year = data.year;
                                $scope.month = data.month - 1;
                            })
                        }
                    }
    }]);


}());