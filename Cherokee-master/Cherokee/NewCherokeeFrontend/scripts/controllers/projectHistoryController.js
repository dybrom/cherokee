(function(){
    var app = angular.module("timeKeeper");

    app.controller("projectHistoryController", ['$scope', 'dataService','$location', '$uibModal', function($scope,dataService,$location, $uibModal){
        $scope.showing=false;
        $scope.projId = 1;
        if($scope.currentUser.role!= 'Administrator'){
            $location.path('/calendar');
        }

        else {
            dataService.list("projects", function (data) {
                $scope.projects = data;
                $scope.listReport();
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


                        // chart for project history by year
                        $scope.labelsForProjectHistory = [];
                        $scope.dataForProjectHistory = [[],[],[]];
                        for(i=0;i<12;i++){
                            $scope.labelsForProjectHistory.push(data.total[0].monthlyHours[i].month);
                            $scope.dataForProjectHistory[0].push(data.total[0].monthlyHours[i].hours)
                            $scope.dataForProjectHistory[1].push(data.total[1].monthlyHours[i].hours)
                            $scope.dataForProjectHistory[2].push(data.total[2].monthlyHours[i].hours)
                        }

                        $scope.ProjectHistorySeries = ['2016', '2017', '2018'];

                        $scope.OptionsForProjectHistory = {
                            legend: { display: true },
                            scales: {
                                xAxes: [{
                                    stacked: false,
                                    beginAtZero: true,
                                    ticks: {
                                        min:0,
                                        autoSkip: false
                                    }
                                }],
                                yAxes: [
                                    {
                                        id: 'y-axis-1',
                                        type: 'linear',
                                        display: true,
                                        position: 'left'
                                    }
                                ]
                            }
                        };



                        // chart for total hours by year


                        console.log(data.total[0].totalHours);
                        $scope.dataForTotalHoursByYears = [];
                        $scope.labelsForTotalHoursByYears = ["2016", "2017", "2018"];
                        for(i=0;i<3;i++){
                            $scope.dataForTotalHoursByYears.push(data.total[i].totalHours);
                        }
                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForTotalHoursByYears = {
                            scaleShowValues: true,
                            scales: {
                                xAxes: [{
                                    stacked: false,
                                    beginAtZero: true,
                                    ticks: {
                                        min:0,
                                        autoSkip: false
                                    }
                                }],
                                yAxes: [{
                                    display: true,
                                    ticks: {
                                        suggestedMin: 0,    // minimum will be 0, unless there is a lower value.
                                        // OR //
                                        beginAtZero: true   // minimum value will be 0.
                                    }
                                }]
                            }

                        }
                        $scope.BuildChart = function(index,data){
                            var modalInstance = $uibModal.open({
                                animation: true,
                                ariaLabelledBy : 'ProjectHistoryEmployee-chart-modal-title',
                                ariaDescribedBy : 'ProjectHistoryEmployee-chart-modal-body',
                                templateUrl : 'views/modals/ProjectHistoryEmployeeChartModal.html',
                                controller : 'ChartEmployeeController',
                                controllerAs : '$paaaaf',
                                resolve: {
                                    index : function(){
                                        return index;
                                    },
                                    data:function(){
                                        return data;
                                    }
                                }
                            });
                        }


                    });
                }
            }
        }





    }]);

    app.controller("ChartEmployeeController", ["$scope", 'dataService', '$uibModalInstance', "index", "data", function($scope,dataService,$uibModalInstance, index, data){
        var $paaaaf = this;
        $scope.index = index;
        $scope.name = data.name;



            // chart for project hours  by month
            $scope.CloseModal = function(){
                $uibModalInstance.close();
            }
            $scope.labelsForEmployeeProjectHistoryModal = [];
            $scope.dataForEmployeeProjectHistoryModal = [[],[],[]];
            for(i=0;i<12;i++){
                $scope.labelsForEmployeeProjectHistoryModal.push(data.sums[0].monthlyHours[i].month);
                $scope.dataForEmployeeProjectHistoryModal[0].push(data.sums[0].monthlyHours[i].hours)
                $scope.dataForEmployeeProjectHistoryModal[1].push(data.sums[1].monthlyHours[i].hours)
                $scope.dataForEmployeeProjectHistoryModal[2].push(data.sums[2].monthlyHours[i].hours)
            }

            $scope.ProjectHistoryEmployeeSeries = ['2016', '2017', '2018'];


            // ovdje se stimaju koje ces boje
            //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
            $scope.OptionsForEmployeeProjectHistoryModal = {
                legend: { display: true },
                scales: {
                    xAxes: [{
                        stacked: false,
                        beginAtZero: true,
                        ticks: {
                            min:0,
                            autoSkip: false
                        }
                    }],
                    yAxes: [
                        {
                            id: 'y-axis-1',
                            type: 'linear',
                            display: true,
                            position: 'left'
                        }
                    ]
                }

            }

    }]);


}());