(function(){
    var app = angular.module("timeKeeper");

    app.controller("monthlyReportController", ['$scope', 'dataService', 'timeConfig','$location', '$uibModal',
        function($scope,dataService,timeConfig,$location, $uibModal){

        $scope.year = 2017;
        $scope.month = 7;
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
                        monthlyReport($scope.year, ($scope.month+1));
                        $scope.showing = true;
                    }
                }
                $scope.listReport();

                function monthlyReport(year, month) {
                    var url = 'reports/monthly/';
                    if (year != 'undefined') url += year;
                    if (month != 'undefined') url += "/" + month;
                    dataService.list(url, function (data) {

                        $scope.report = data;
                        $scope.year = data.year;
                        $scope.month = data.month-1;



                        // chart for total hours by project

                        $scope.dataForMonthlyReportTotalHoursByProject = [];
                        $scope.labelsForMonthlyReportTotalHoursByProject = [];

                        for(i=0;i<data.projects.length;i++){
                            $scope.labelsForMonthlyReportTotalHoursByProject.push(data.projects[i].project);
                            $scope.dataForMonthlyReportTotalHoursByProject.push(data.projects[i].hours);
                        }
                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForMonthlyReportTotalHoursByProject = {
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

                        $scope.dataForNumberOfEmployeesByProject = [];
                        $scope.labelsForNumberOfEmployeesByProject = [];

                        for(i=0;i<data.projects.length;i++){
                            var count = 0;
                            $scope.labelsForNumberOfEmployeesByProject.push(data.projects[i].project);
                            for(j=0;j<data.items.length;j++){
                                if(data.items[j].hours[i]!=0){
                                    count++;
                                }

                            }
                            $scope.dataForNumberOfEmployeesByProject.push(count);
                        }
                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForNumberOfEmployeesByProject = {
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

                        $scope.MakeProjectChart = function(index,report){
                            var modalInstance = $uibModal.open({
                                animation: true,
                                ariaLabelledBy : 'monthlyProject-chart-modal-title',
                                ariaDescribedBy : 'monthlyProject-chart-modal-body',
                                templateUrl : 'views/modals/monthlyProjectModal.html',
                                controller : 'monthlyProjectChartModalCtrl',
                                controllerAs : '$pcm',
                                resolve: {
                                    index : function(){
                                        return index;
                                    },
                                    report:function(){
                                        console.log(report);
                                        return report;
                                    }
                                }
                            });
                        }

                        $scope.MakeEmployeeChart = function(index,report){
                            var modalInstance = $uibModal.open({
                                animation: true,
                                ariaLabelledBy : 'monthlyEmployee-chart-modal-title',
                                ariaDescribedBy : 'monthlyEmployee-chart-modal-body',
                                templateUrl : 'views/modals/monthlyEmployeeModal.html',
                                controller : 'monthlyEmployeeChartModalCtrl',
                                controllerAs : '$acm',
                                resolve: {
                                    index : function(){
                                        return index;
                                    },
                                    report:function(){
                                        return report;
                                    }
                                }
                            });
                        }




                    })
                }
            }
        }]);


    app.controller("monthlyProjectChartModalCtrl", ["$scope", 'dataService', '$uibModalInstance', "index", "report", function($scope,dataService,$uibModalInstance, index, report){
        var $pcm = this;
        $scope.index = index;
        console.log($scope.index);
        $scope.year = report.year;
        console.log($scope.year);
        console.log(report);
        $scope.name = report.projects[index].project;
        $scope.totalHours = report.projects[index].hours;
            // chart for project hours  by month
            $scope.CloseModal = function(){
                $uibModalInstance.close();
            }
            $scope.dataForHoursByProjectMonthlyModal = [];
            $scope.labelsForHoursByProjectMonthlyModal = [];
            for(i=0;i<report.items.length;i++){
                if(report.items[i].hours[index]>0) {
                    $scope.labelsForHoursByProjectMonthlyModal.push(report.items[i].employee);
                    $scope.dataForHoursByProjectMonthlyModal.push(report.items[i].hours[index]);
                }
            }


            // ovdje se stimaju koje ces boje
            //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
            $scope.OptionsForHoursByProjectMonthlyModal = {
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

    app.controller("monthlyEmployeeChartModalCtrl", ["$scope", 'dataService', '$uibModalInstance', "index", "report", function($scope,dataService,$uibModalInstance, index, report){
        var $pcm = this;
        $scope.index = index;
        $scope.year = report.year;
        $scope.name = report.items[index].employee;
        $scope.totalHours = report.items[index].total;

            // chart for project hours  by month
            $scope.CloseModal = function(){
                $uibModalInstance.close();
            }
            $scope.dataForHoursForEmployeeByProjectMonthlyModal = [];
            $scope.labelsForHoursForEmployeeByProjectMonthlyModal = [];
            for(i=0;i<report.projects.length;i++){
                if(report.items[index].hours[i]>0) {
                    $scope.labelsForHoursForEmployeeByProjectMonthlyModal.push(report.projects[i].project);
                    $scope.dataForHoursForEmployeeByProjectMonthlyModal.push(report.items[index].hours[i]);
                }
            }


            // ovdje se stimaju koje ces boje
            //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
            $scope.OptionsForHoursForEmployeeByProjectMonthlyModal = {
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