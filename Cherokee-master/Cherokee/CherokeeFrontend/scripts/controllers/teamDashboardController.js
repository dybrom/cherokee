(function(){
    var app = angular.module("timeKeeper");

    app.controller("teamDashboardController", ['$scope', 'dataService', 'timeConfig', '$location',
        function ($scope,dataService, timeConfig, $location) {
        $scope.months = timeConfig.months;
        $scope.month = 6;
        $scope.year = 2017;
        $scope.showing = false;
        $scope.teamsShow=false;
        $scope.teams = [];
        var user = new Object();
      /*  dataService.read("employees", $scope.currentUser.id, function (data) {
            user = data;
            if(user.position.name == 'Administrator') {
                $scope.teamsShow = true;
                dataService.list("teams", function (data) {
                    $scope.teams = data;
                });
            }
            ovo je za sprjecavanje nekog da preko konzole promijeni role u administrator
        });*/
        if($scope.currentUser.role == 'Administrator') {
            $scope.teamsShow = true;
            dataService.list("teams", function (data) {
                $scope.teams = data;
            });



        }
        else if($scope.currentUser.teams.length>0){
            $scope.teamsShow = true;
            var empTeams = [];
            for(i =0;i<$scope.currentUser.teams.length;i++){
                empTeams.push($scope.currentUser.teams[i].id);
            }
            for(j=0;j<empTeams.length;j++){
                dataService.read("teams", empTeams[j], function (data) {
                    $scope.teams.push(data);
                })
            }
        }
        else{
            $location.path('/calendar');
        }
        $scope.listReport = function () {
            if ($scope.teamId == undefined) {
                $scope.showing = false;
                window.alert("Choose team!");
            }
            if ($scope.year == undefined) {
                $scope.showing = false;
                window.alert("Choose year!");
            }
            else if ($scope.month == undefined) {
                $scope.showing = false;
                window.alert("Choose month!");
            }
            else {
                teamDashboard($scope.teamId, $scope.year, $scope.month);
                $scope.show = true;
            }
        }

        function teamDashboard(teamId, year,month){
            var url = 'reports/TeamReport?teamId=' + teamId;
            if(year != 'undefined') url += "&year=" + year;
            if(month != 'undefined') url += "&month=" + month;
            dataService.list(url, function(data){

                $scope.report = data;
                $scope.teamId = data.id;
                $scope.year = data.year;
                $scope.month = data.month - 1;

                console.log($scope.report.reports);

                //chart total hours za team na teamDashboard.html
                var workingHoursCanvas = document.getElementById("teamTotalHoursChart");

                Chart.defaults.global.defaultFontFamily = "Lato";
                Chart.defaults.global.defaultFontSize = 18;

                var workingHoursData = {
                    labels: ["Total Hours Possible  [ENTIRE TEAM]", "Worked Hours"],
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
                //kraj chart koda za total hours tima

                //inicijalizacija nizova za chartove
                var listOfMembers = new Array();
                var perMemberMissingEntries = new Array();
                var perMemberMissingHoursColor = new Array();
                var ptoHours = new Array();
                var overtimeHours = new Array();

                //console.log($scope.report.numberOfEmployees);
                //console.log(listOfMembers);

                //foreach petlja za chatove clanova tima, totalhours
                angular.forEach($scope.report.reports, function(emp){
                    //$scope.report.reports.forEach(function(emp){

                    //console.log($scope.report.reports);
                    //console.log(emp);

                    //pronadji div sa id="teamDashboardCharts" na teamDashboard
                    //kreiraj canvas element sa formiranim id-jem za svakog clana tima i appendaj
                    var membersdiv = document.createElement('canvas');
                    membersdiv.id = "teamMember " + emp.employee.id + " TotalHoursChart";

                    //membersdiv.setAttribute("style", "width:200px; height:200px");

                    teamDashboardCharts.append(membersdiv);

                    var workingHoursCanvas = document.getElementById("teamMember " + emp.employee.id + " TotalHoursChart");

                    Chart.defaults.global.defaultFontFamily = "Lato";
                    Chart.defaults.global.defaultFontSize = 18;

                    var workingHoursData = {
                        labels: ["Total Hours Possible  [" + emp.employee.name + "]  ", "Worked Hours"],
                        datasets: [
                            {
                                data: [emp.totalHours, (emp.workingDaysInMonth)*8-emp.totalHours],
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

                    //dodaj u niz missing entries za trenutnog clana tima, niz kao data za chart //foreach
                    perMemberMissingEntries.push(emp.days.missingEntries);

                    //dodaj listu clanova tima za chatove poslije foreach-a //foreach
                    listOfMembers.push(emp.employee.name);

                    //ideja za random boje u chart-u //foreach
                    var dynamicColors = function() {
                        var r = Math.floor(Math.random() * 255);
                        var g = Math.floor(Math.random() * 255);
                        var b = Math.floor(Math.random() * 255);
                        return "rgb(" + r + "," + g + "," + b + ")";
                    };
                    perMemberMissingHoursColor.push(dynamicColors);

                    //console.log(emp.employee.name + ":");
                    //console.log(emp.days);

                    //suma PTO po clanu tima, niz kao data za chart //foreach
                    var ptoHoursSum = ((emp.days.overtimeHours + emp.days.vacationDays + emp.days.publicHolidays + emp.days.sickLeaveDays + emp.days.religiousDays + emp.days.otherAbscenceDays) * 8);
                    ptoHours.push(ptoHoursSum);

                    //dadaj u niz overtime hours za trenutnog clana tima, niz kao data za chart //foreach
                    overtimeHours.push(emp.days.overtimeHours);

                }); //kraj foreach petlje, podaci za chartove ispod dobavljeni unutar foreach-a


                new Chart(document.getElementById("teamDashMissingEntries"), {
                    type: 'horizontalBar',
                    data: {
                        labels: listOfMembers,
                        datasets: [
                            {
                                label: "Missing Hours",
                                backgroundColor: perMemberMissingHoursColor,
                                data: perMemberMissingEntries
                            }
                        ]
                    },
                    options: {
                        legend: { display: false },
                        title: {
                            display: true,
                            text: 'Missing Entries by Member'
                        }
                    }
                }); //kraj missing entries

                new Chart(document.getElementById("teamDashPTOByMember"), {
                    type: 'bar',
                    data: {
                        labels: listOfMembers,
                        datasets: [
                            {
                                label: "PTO",
                                backgroundColor: perMemberMissingHoursColor,
                                data: ptoHours
                            }
                        ]
                    },
                    options: {
                        legend: { display: false },
                        title: {
                            display: true,
                            text: 'PTO by Member'
                        }
                    }
                }); //kraj PTO


                new Chart(document.getElementById("teamDashOvertimeHoursByMember"), {
                    type: 'bar',
                    data: {
                        labels: listOfMembers,
                        datasets: [
                            {
                                label: "Overtime Hours",
                                backgroundColor: perMemberMissingHoursColor,
                                data: overtimeHours
                            }
                        ]
                    },
                    options: {
                        legend: { display: false },
                        title: {
                            display: true,
                            text: 'Overtime Hours by Member'
                        }
                    }
                }); //kraj overtime
            })
        }

    }]);


}());