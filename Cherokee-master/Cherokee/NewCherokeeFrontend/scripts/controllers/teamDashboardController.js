(function(){
    var app = angular.module("timeKeeper");

    app.controller("teamDashboardController", ['$scope', 'dataService', 'timeConfig', '$location', '$rootScope',
        function ($scope,dataService, timeConfig, $location, $rootScope) {
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

                    teamDashboard($scope.teamId, $scope.year, ($scope.month+1));
                    $scope.showing = true;



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
                    $scope.month = data.month-1;


                    // chart for utilization

                    $scope.dataForUtilizationForTeam = [];
                    $scope.labelsForUtilizationForTeam = ["Total hours in this month", "Total hours possible"];
                    $scope.dataForUtilizationForTeam.push(data.totalHours);
                    $scope.dataForUtilizationForTeam.push(data.maxPossibleHours);
                    // ovdje se stimaju koje ces boje
                    //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                    $scope.OptionsForUtilizationForTeam = {
                        scaleShowValues: true,
                        scales: {
                            xAxes: [{
                                stacked: false,
                                beginAtZero: true,
                                ticks: {
                                    min:0,
                                    stepSize: 1,
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


                    // chart for missing entries team

                    $scope.dataForMissingEntriesTeam = [];
                    $scope.labelsForMissingEntriesTeam = [];
                    for(i=0;i<data.reports.length;i++){
                        $scope.labelsForMissingEntriesTeam.push(data.reports[i].employee.name);
                        $scope.dataForMissingEntriesTeam.push(data.reports[i].days.missingEntries);
                    }
                    // ovdje se stimaju koje ces boje
                    //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                    $scope.OptionsForMissingEntriesTeam = {
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

                    // chart for hours by project

                    $scope.dataForHoursByProjectTeam = [];
                    $scope.labelsForHoursByProjectTeam = [];
                    for(i=0;i<data.projects.length;i++){
                        $scope.labelsForHoursByProjectTeam.push(data.projects[i].name);
                        $scope.dataForHoursByProjectTeam.push(data.projects[i].hours);
                    }
                    // ovdje se stimaju koje ces boje
                    //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                    $scope.OptionsForHoursByProjectTeam = {
                        scaleShowValues: true,
                        scales: {
                            xAxes: [{
                                stacked: false,
                                beginAtZero: true,
                                ticks: {
                                    min:0,
                                    stepSize: 1,
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


                    // chart for overtime hours by member

                    $scope.dataForOvertimeHoursTeam = [];
                    $scope.labelsForOvertimeHoursTeam = [];
                    for(i=0;i<data.reports.length;i++){
                        $scope.labelsForOvertimeHoursTeam.push(data.reports[i].employee.name);
                        $scope.dataForOvertimeHoursTeam.push(data.reports[i].days.overtimeHours);
                    }
                    // ovdje se stimaju koje ces boje
                    //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                    $scope.OptionsForOvertimeHoursTeam = {
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


                    // chart for utilization by member


                    $scope.dataForUtilizationByMemberTeam = [];
                    $scope.labelsForUtilizationByMemberTeam = [];
                    for(i=0;i<data.reports.length;i++){
                        $scope.labelsForUtilizationByMemberTeam.push(data.reports[i].employee.name);
                        $scope.dataForUtilizationByMemberTeam.push(data.reports[i].days.percentageOfWorkingDays);
                    }
                    // ovdje se stimaju koje ces boje
                    //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                    $scope.OptionsForUtilizationByMemberTeam = {
                        scaleShowValues: true,
                        scales: {
                            xAxes: [{
                                stacked: false,
                                ticks: {
                                    suggestedMin:0,
                                    autoSkip: false,
                                    beginAtZero: true
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

                    // chart for PTO by member


                    $scope.dataForPTOByMemberTeam = [];
                    $scope.labelsForPTOByMemberTeam = [];
                    for(i=0;i<data.reports.length;i++){
                        $scope.labelsForPTOByMemberTeam.push(data.reports[i].employee.name);
                        $scope.dataForPTOByMemberTeam.push(data.reports[i].pto);
                    }
                    // ovdje se stimaju koje ces boje
                    //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                    $scope.OptionsForPTOByMemberTeam = {
                        scaleShowValues: true,
                        scales: {
                            xAxes: [{
                                stacked: false,
                                ticks: {
                                    suggestedMin:0,
                                    autoSkip: false,
                                    beginAtZero: true
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




                   /* //chart total hours za team na teamDashboard.html
                    var workingHoursCanvas = document.getElementById("teamTotalHoursChart");

                    Chart.defaults.global.defaultFontFamily = "Lato";
                    Chart.defaults.global.defaultFontSize = 18;

                    var workingHoursData = {
                        labels: ["Total Hours Possible [Team]", "Remaining hours"],
                        datasets: [
                            {
                                data: [$scope.report.maxPossibleHours, Math.abs((($scope.report.maxPossibleHours)-$scope.report.totalHours))+ 300],
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
                                    data: [emp.maxPossibleHours, Math.abs((($scope.report.maxPossibleHours)-$scope.report.totalHours))+ 300],
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
                    }); //kraj overtime */
                })
            }
            if(currentUser.role=='Administrator'){
                $scope.teamId = 'A';
                $scope.listReport();
            }
            else if(currentUser.teams.length>0){
                $scope.teamId = currentUser.teams[0].id;
                $scope.listReport();
            }

        }]);


}());