(function(){
    var app = angular.module("timeKeeper");

    app.controller("projectHistoryController", ['$scope', 'dataService', function($scope,dataService){
        $scope.showing=false;

        if($scope.currentUser.role!= 'Administrator'){
            $location.path('/calendar');
        }

        else {
            dataService.list("projects", function (data) {
                $scope.projects = data;
            })


            $scope.listReport = function () {
                if ($scope.projId == undefined) {
                    $scope.showing = false;
                    window.alert("Choose project!!");
                }
                else {
                    dataService.list("reports/projectsHistory?projectId=" + $scope.projId, function (data) {

                        $scope.report = data;
                        $scope.showing = true;
                        $scope.projId = data.id;
                    });
                }
            }
        }
    }]);


}());