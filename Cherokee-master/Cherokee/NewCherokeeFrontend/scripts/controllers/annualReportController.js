(function(){
    var app = angular.module("timeKeeper");

    app.controller("annualReportController", ['$scope', 'dataService', 'timeConfig', '$location', '$uibModal',
                                      function($scope,   dataService,   timeConfig,   $location, $uibModal) {

        $scope.year = 2017;
            if($scope.currentUser.role!= 'Administrator'){
                $location.path('/calendar');
            } else {


                $scope.showing = false;

                $scope.listReport = function () {
                    if ($scope.year == undefined) {
                        $scope.showing = false;
                        window.alert("Choose year!");
                    } else {
                        annualReport($scope.year);
                        $scope.showing = true;
                    }
                };
                $scope.listReport();
                function annualReport(year) {

                    var url = 'reports/annual/';
                    if (year != 'undefined') url += year;
                    dataService.list(url, function (data) {

                        $scope.report = data;
                        $scope.year = data.year;

                        $scope.numOfProjects = data.list.length;
                        $scope.numOfEmployees = 0;
                        $scope.totalHours = 0;
                        for(i=0;i<data.list.length-1;i++){
                            $scope.numOfEmployees+=data.list[i].numOfEmployees;
                            $scope.totalHours += data.list[i].totalHours;
                        }


                        // chart for total hours by project

                        $scope.dataForYearlyReportTotalHoursByProject = [];
                        $scope.labelsForYearlyReportTotalHoursByProject = [];

                        for(i=0;i<data.list.length-1;i++){
                            $scope.labelsForYearlyReportTotalHoursByProject.push(data.list[i].projectName);
                            $scope.dataForYearlyReportTotalHoursByProject.push(data.list[i].totalHours);
                        }
                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForYearlyReportTotalHoursByProject = {
                            //legend: { display: true },
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



                        // chart for total number of employees by project

                        $scope.dataForNoOfEmployeesByProject = [];
                        $scope.labelsForNoOfEmployeesByProject = [];

                        for(i=0;i<data.list.length-1;i++){
                            $scope.labelsForNoOfEmployeesByProject.push(data.list[i].projectName);
                            $scope.dataForNoOfEmployeesByProject.push(data.list[i].numOfEmployees);
                        }
                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForNoOfEmployeesByProject = {
                            //legend: { display: true },
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


                        $scope.GetProjectChart = function(index,year, data) {
                            var modalInstance = $uibModal.open({
                                animation: true,
                                ariaLabelledBy : 'annual-chart-modal-title',
                                ariaDescribedBy : 'annual-chart-modal-body',
                                templateUrl : 'views/modals/annualChartModal.html',
                                controller : 'annualChartModalCtrl',
                                controllerAs : '$acm',
                                resolve: {
                                    index : function(){
                                        return index;
                                    },
                                    year:function(){
                                        return year;
                                    },
                                    data:function(){
                                        console.log(data);
                                        return data;
                                    }
                                }
                            });
                        }



                    })
                }
            }
        }]);
    app.controller("annualChartModalCtrl", ["$scope", 'dataService', '$uibModalInstance', "index", "year","data", function($scope,dataService,$uibModalInstance, index, year,data){
         var $acm = this;
            $scope.index = index;
            console.log($scope.index);
            $scope.year = year;
            console.log($scope.year);
            $scope.name = data.projectName;
            $scope.totalHours = data.totalHours;
                // chart for project hours  by month
                $scope.CloseModal = function(){
                    $uibModalInstance.close();
                }
                $scope.dataForHoursByProjectAnnualModal = [];
                $scope.labelsForHoursByProjectAnnualModal = ["January", "February", "March", "April", "May", "June", "July",
                "August", "September", "October", "November", "December"];
                for(i=0;i<data.monthlyHours.length;i++){
                    $scope.dataForHoursByProjectAnnualModal.push(data.monthlyHours[i]);
                }


                // ovdje se stimaju koje ces boje
                //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                $scope.OptionsForHoursByProjectAnnualModal = {
                    //legend: { display: true },
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

    }]);
}());