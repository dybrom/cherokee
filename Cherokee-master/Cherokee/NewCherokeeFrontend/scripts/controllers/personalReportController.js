(function(){
    var app = angular.module("timeKeeper");

    app.controller("personalReportController", ['$scope','dataService', 'timeConfig', function($scope, dataService, timeConfig){
        $scope.months = timeConfig.months;
        $scope.month = 6;
        $scope.year = 2017;
        $scope.showing = false;
        $scope.empsShow = true;
        $scope.people = [];
        $scope.empId = currentUser.id;


        // $scope.currentUser.teams[0].role.id == 'TL '


        if($scope.currentUser.role=='Administrator'){
            $scope.empsShow = true;

            dataService.list("employees/all", function (data) {
                $scope.people = data;
            });

            $scope.listReport = function () {

                if ($scope.empId === undefined) {
                    $scope.showing = false;
                    window.alert('You have to choose an employee');
                }
                else {
                    personalReport($scope.empId, $scope.year, ($scope.month+1));
                    $scope.showing = true;
                }
            }
            $scope.listReport();
        }

        else if($scope.currentUser.teams.length>0){

            var membersInTeam = [];
            membersInTeam.push($scope.currentUser.id);
            dataService.list("members", function (data) {
                $scope.members = data;
                for(i=0;i<$scope.members.length;i++){
                    for(j=0;j<$scope.currentUser.teams.length;j++) {
                        if ($scope.members[i].team.id == currentUser.teams[j].id && $scope.members[i].employee.id!=$scope.currentUser.id) {
                            membersInTeam.push($scope.members[i].employee.id);
                        }
                    }
                }


                for(j=0;j<membersInTeam.length;j++){
                    dataService.read("employees", membersInTeam[j], function (data) {
                        $scope.people.push(data);
                    })
                }
            });

            $scope.empsShow = true;



            $scope.listReport = function () {

                if ($scope.empId === undefined) {
                    $scope.showing = false;
                    window.alert('You have to choose an employee');
                }
                else {
                    personalReport($scope.empId, $scope.year, ($scope.month + 1));
                    $scope.showing = true;
                }
            }
            $scope.listReport();
        }
        else {
            $scope.empsShow = false;
            $scope.listReport = function () {
                personalReport($scope.currentUser.id, $scope.year, ($scope.month + 1));
                $scope.showing = true;
            }
            $scope.listReport();
        }

        function personalReport(empId, year,month){
            var url = 'reports/personal/' + empId;
            if(year != 'undefined') url += "/" + year;
            if(month != 'undefined') url += "/" + month;
            dataService.list(url, function(data){

                $scope.report = data;
                $scope.empId = data.employee.id;
                $scope.year = data.year;
                $scope.month = data.month - 1;


                /* Chart for Hours in month vs Hours in Year */
                $scope.dataForMVSY = [];
                $scope.labelsForMVSY = ["Total hours in this month", "Total hours in this year"];
                $scope.dataForMVSY.push(data.totalHours);
                $scope.dataForMVSY.push(data.totalHoursInYear);
                // ovdje se stimaju koje ces boje
                //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                $scope.OptionsForMVSY = {
                    legend: {
                        display: true,
                        position: 'top',
                    }

                }
                // Chart za day statistic

                $scope.dataForDayStatistic = [];
                $scope.labelsForDayStatistic = ["Working days", "Vacation days",
                    'Public holidays', 'Religious days', 'Business absences' ,'Sick leave days' , 'Other Absence'];
                $scope.dataForDayStatistic.push(data.workingDays);
                $scope.dataForDayStatistic.push(data.vacationDays);
                $scope.dataForDayStatistic.push(data.publicHolidays);
                $scope.dataForDayStatistic.push(data.religiousDays);
                $scope.dataForDayStatistic.push(data.businessAbsences);
                $scope.dataForDayStatistic.push(data.sickLeaveDays);
                $scope.dataForDayStatistic.push(data.otherAbsenceDays);
                $scope.OptionsForDayStatistic = {
                    scaleShowValues: true,
                    scales: {
                        xAxes: [{
                            stacked: false,
                            beginAtZero: true,
                            ticks: {
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

                // chart for team hours:

                $scope.dataForHoursByProject = [];
                $scope.labelsForHoursByProject = [];
                for(i=0;i<data.projects.length;i++){
                    $scope.labelsForHoursByProject.push(data.projects[i].name);
                    $scope.dataForHoursByProject.push(data.projects[i].hours);
                }
                $scope.OptionsForHoursByProject = {
                    scaleShowValues: true,
                    scales: {
                        xAxes: [{
                            stacked: false,
                            beginAtZero: true,
                            ticks: {
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

                // chart for utilization

                $scope.dataForUtilization = [];
                $scope.labelsForUtilization = ["Total hours in this month", "Total hours possible"];
                $scope.dataForUtilization.push(data.totalHours);
                $scope.dataForUtilization.push(data.totalPossibleHours);
                // ovdje se stimaju koje ces boje
                //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                $scope.OptionsForUtilization = {
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
               // console.log(data.reports[0].employee.name);
                //console.log(data.reports[0].days.missingEntries);












               /* var utilizationCanvas = document.getElementById("utilizationChart");

                Chart.defaults.global.defaultFontFamily = "Lato";
                Chart.defaults.global.defaultFontSize = 18;

                var utilizationData = {
                    labels: [
                        "Utilization",
                        "Exploit"
                    ],
                    datasets: [
                        {
                            data: [$scope.report.percentageOfWorkingDays, 100-$scope.report.percentageOfWorkingDays],
                            //data: [$scope.report.maxPossibleHours, Math.abs((($scope.report.maxPossibleHours)-$scope.report.totalHours))+ 20],
                            backgroundColor: [
                                "#FF6384",
                                "#63FF84"
                            ],
                            borderColor: "white",
                            borderWidth: 2
                        }]
                };

                var chartOptions = {
                    rotation: -Math.PI,
                    cutoutPercentage: 30,
                    circumference: Math.PI,
                    legend: {
                        position: 'left'
                    },
                    animation: {
                        animateRotate: false,
                        animateScale: true
                    }
                };

                var utilizationChart = new Chart(utilizationCanvas, {
                    type: 'doughnut',
                    data: utilizationData,
                    options: chartOptions
                });


                var workingHoursCanvas = document.getElementById("workingHoursChart");

                Chart.defaults.global.defaultFontFamily = "Lato";
                Chart.defaults.global.defaultFontSize = 18;

                var workingHoursData = {
                    labels: ["Total Hours Possible", "Remaining"],
                    datasets: [
                        {
                            data: [$scope.report.totalHours+10, $scope.report.totalHours],
                            backgroundColor: [
                                "#FF6384",
                                "#63FF84"
                            ]
                        }]
                };

                var workingHourspieChart = new Chart(workingHoursCanvas, {
                    type: 'pie',
                    data: workingHoursData
                });*/
            })
        }
    }]);

}());