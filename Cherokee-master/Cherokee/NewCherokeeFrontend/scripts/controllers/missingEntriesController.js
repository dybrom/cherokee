(function(){
    var app = angular.module("timeKeeper");

    app.controller("missingEntriesController", ['$scope', 'dataService','timeConfig',  function ($scope,dataService, timeConfig) {

        if($scope.currentUser.role!='Administrator'){
            $location.path('/calendar');
        }
        $scope.months = timeConfig.months;
        $scope.year = 2017;
        $scope.month = 6;
        dataService.list("missingentries?year="+$scope.year+"&month="+$scope.month, function(data){
            $scope.entriesreport = data;
            $scope.year = data.year;
            $scope.month = data.month;

        });
        $scope.listReport = function(){
            dataService.list("missingentries?year="+$scope.year+"&month="+$scope.month, function(data){
                $scope.entriesreport = data;
                $scope.year = data.year;
                $scope.month = data.month;
            });
        }



        $scope.deleteEntry = function(index){

            $scope.entriesreport.employees.splice(index,1);
            console.log($scope.entriesreport);
        }
        $scope.SendEmails = function () {
            dataService.insert("missingentries", $scope.entriesreport, function(data){
                console.log("uspjesno");
            })
        }

    }]);
}());