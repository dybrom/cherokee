(function(){
    var app = angular.module("timeKeeper");

    app.controller("personalReportController", ['$scope','dataService', 'timeConfig', function($scope, dataService, timeConfig){
        $scope.months = timeConfig.months;
        $scope.month = 6;
        $scope.year = 2017;
        $scope.showing = false;
        $scope.empsShow = true;
        $scope.people = [];



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
                    personalReport($scope.empId, $scope.year, $scope.month + 1);
                    $scope.showing = true;
                }
            }
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
                    personalReport($scope.empId, $scope.year, $scope.month + 1);
                    $scope.showing = true;
                }
            }
        }
        else {
            $scope.empsShow = false;
            $scope.listReport = function () {
                personalReport($scope.currentUser.id, $scope.year, $scope.month + 1);
                $scope.showing = true;
            }
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

                var utilizationCanvas = document.getElementById("utilizationChart");

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
                    labels: ["Total Hours Possible", "Worked Hours"],
                    datasets: [
                        {
                            data: [$scope.report.totalHours, ($scope.report.workingDaysInMonth)*8-$scope.report.totalHours],
                            backgroundColor: [
                                "#FF6384",
                                "#63FF84"
                            ]
                        }]
                };

                var workingHourspieChart = new Chart(workingHoursCanvas, {
                    type: 'pie',
                    data: workingHoursData
                });
            })
        }
    }]);

}());