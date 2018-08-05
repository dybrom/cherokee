(function(){
    var app = angular.module("timeKeeper");

    app.controller("companyDashboardController", ['$scope','dataService', 'timeConfig',
                                          function($scope,  dataService,   timeConfig) {
        $scope.months = timeConfig.months;
        $scope.month = 6;
        $scope.year = 2017;
        $scope.showing = false;

        if($scope.currentUser.role!= 'Administrator'){
            $location.path('/calendar');
        } else {

            $scope.companyReport = function () {
                if ($scope.year == undefined) {
                    $scope.showing = false;
                    window.alert("Choose year!");
                } else if ($scope.month == undefined) {
                    $scope.showing = false;
                    window.alert("Choose month!");
                } else {
                    var month = $scope.month+1;
                    dataService.list("reports/CompanyReport?" + "year=" + $scope.year + "&month=" + month, function (data) {

                        $scope.report = data;
                        $scope.showing = true;
                        $scope.year = data.year;
                        $scope.month = data.month-1;


                        console.log($scope.report);

                        // chart for utilization
                        $scope.dataForUtilizationForCompany = [];
                        $scope.labelsForUtilizationForCompany = ["Total hours in this month", "Total hours possible"];
                        $scope.dataForUtilizationForCompany.push(data.totalHours);
                        $scope.dataForUtilizationForCompany.push(data.maxPossibleHours);
                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForUtilizationForCompany = {
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


                        // chart for utilization by role


                        $scope.dataForUtilizationForCompanyByRole = [];
                        $scope.labelsForUtilizationForCompanyByRole  = ["Developer", "QA Engineer", "UI/UX", "Project Manager"];
                        $scope.dataForUtilizationForCompanyByRole.push(data.devUtilization);
                        $scope.dataForUtilizationForCompanyByRole.push(data.qaUtilization);
                        $scope.dataForUtilizationForCompanyByRole.push(data.uiuxUtilization);
                        $scope.dataForUtilizationForCompanyByRole.push(data.pmUtilization);

                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForUtilizationForCompanyByRole  = {
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


                        // chart for utilization by team


                        $scope.dataForUtilizationForCompanyByTeam = [];
                        $scope.labelsForUtilizationForCompanyByTeam  = [];
                        for(i=0;i<data.teams.length;i++){
                            $scope.labelsForUtilizationForCompanyByTeam.push(data.teams[i].name);
                            $scope.dataForUtilizationForCompanyByTeam.push(data.teams[i].utilization);
                        }


                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForUtilizationForCompanyByTeam  = {
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



                        // chart for missing entries by team


                        $scope.dataForMissingEntriesInCompany = [];
                        $scope.labelsForMissingEntriesInCompany  = [];
                        for(i=0;i<data.teams.length;i++){
                            $scope.labelsForMissingEntriesInCompany.push(data.teams[i].name);
                            $scope.dataForMissingEntriesInCompany.push(data.teams[i].missingEntries);
                        }


                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForMissingEntriesInCompany  = {
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


                        // chart for overtime hours by team



                        $scope.dataForOvertimeHoursByTeam = [];
                        $scope.labelsForOvertimeHoursByTeam  = [];
                        for(i=0;i<data.teams.length;i++){
                            $scope.labelsForOvertimeHoursByTeam.push(data.teams[i].name);
                            $scope.dataForOvertimeHoursByTeam.push(data.teams[i].overtimeHours);
                        }


                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForOvertimeHoursByTeam  = {
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




                        // chart for revenue by project


                        $scope.dataForRevenueByProject = [];
                        $scope.labelsForRevenueByProject  = [];
                        for(i=0;i<data.projects.length;i++){
                            $scope.labelsForRevenueByProject.push(data.projects[i].name);
                            $scope.dataForRevenueByProject.push(data.projects[i].revenue);
                        }


                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForRevenueByProject  = {
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

                        // chart for total hours by team


                        $scope.dataForTotalHoursByTeam = [];
                        $scope.labelsForTotalHoursByTeam  = [];
                        for(i=0;i<data.teams.length;i++){
                            $scope.labelsForTotalHoursByTeam.push(data.teams[i].name);
                            $scope.dataForTotalHoursByTeam.push(data.teams[i].totalHours);
                        }


                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForTotalHoursByTeam  = {
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


                        // chart for no. of roles in company

                        // chart for total hours by team

                        $scope.labelsForRolesInCompany  = ["Project Manager", "Developer", "QA Engineer", "UI/UX Designer"];
                        $scope.dataForRolesInCompany = [data.pmCount, data.devCount, data.qaCount,data.uiuxCount];




                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForRolesInCompany  = {
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


                        // chart for PTO by team


                        $scope.dataForPTOByTeamCompany = [];
                        $scope.labelsForPTOByTeamCompany = [];
                        for(i=0;i<data.teams.length;i++){
                            $scope.labelsForPTOByTeamCompany.push(data.teams[i].name);
                            $scope.dataForPTOByTeamCompany.push(data.teams[i].pto);
                        }
                        // ovdje se stimaju koje ces boje
                        //$scope.colorsForMVSY = ['#ADD8E6', '#808080', '#DCDCDC', '#46BFBD', '#FDB45C', '#949FB1', '#4D5360'];
                        $scope.OptionsForPTOByTeamCompany = {
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





                       /* var workingHoursCanvas = document.getElementById("companyTotalHoursChart");

                        Chart.defaults.global.defaultFontFamily = "Lato";
                        Chart.defaults.global.defaultFontSize = 18;

                        var companyTotalHours = {
                            labels: ["Worked Hours", "Remaining hours"],
                            datasets: [
                                {
                                    data: [$scope.report.maxPossibleHours, Math.abs((($scope.report.maxPossibleHours) - $scope.report.totalHours)) + 300],
                                    backgroundColor: [
                                        "#FF6384",
                                        "#63FF84"
                                    ]
                                }]
                        };

                        var workingHourspieChart = new Chart(workingHoursCanvas, {
                            type: 'pie',
                            data: companyTotalHours
                        });
                        //kraj chart koda za total hours tima

                        var listOfTeams = new Array();
                        var listOfMissingEntriesByTeam = new Array();
                        var listOfOvertimeHoursByTeam = new Array();
                        var listOfRevenuesByTeam = new Array();
                        var listOfProjects = new Array();

                        angular.forEach($scope.report.teams, function (team) {
                            listOfTeams.push(team.name);
                            listOfMissingEntriesByTeam.push(team.missingEntries);
                            listOfOvertimeHoursByTeam.push(team.overtimeHours);
                        });
                        angular.forEach($scope.report.projects, function (project) {
                            listOfProjects.push(project.name);
                            listOfRevenuesByTeam.push(project.revenue);
                        });

                        console.log(listOfProjects, listOfRevenuesByTeam);

                        //var perTeamMissingEntries = new Array();

                        new Chart(document.getElementById("companyDashMissingEntries"), {
                            type: 'horizontalBar',
                            data: {
                                labels: listOfTeams,
                                datasets: [
                                    {
                                        label: "Missing Entries",
                                        //backgroundColor: perMemberMissingHoursColor,
                                        data: listOfMissingEntriesByTeam
                                    }
                                ]
                            },
                            options: {
                                legend: {display: false},
                                title: {
                                    display: true,
                                    text: 'Missing Entries by Team'
                                }
                            }
                        }); //kraj missing entries

                        //PM utilization
                        var companyPMutilization = document.getElementById("companyPMutilization");

                        var companyPMutilizationData = {
                            labels: ['PM utilization', '14.77'],
                            datasets: [
                                {
                                    data: [$scope.report.pmUtilization, 100 - $scope.report.pmUtilization],
                                    backgroundColor: [
                                        "#FF6384",
                                        "#63FF84"
                                    ]
                                }]
                        };

                        var utilizationPM = new Chart(companyPMutilization, {
                            type: 'pie',
                            data: companyPMutilizationData
                        }); //kraj PM utilization chart

                        //DEV utilization
                        var companyDEVutilization = document.getElementById("companyDEVutilization");

                        var companyDEVutilizationData = {
                            labels: ['DEV utilization', '17.53'],
                            datasets: [
                                {
                                    data: [$scope.report.devUtilization, 100 - $scope.report.devUtilization],
                                    backgroundColor: [
                                        "#FF6384",
                                        "#63FF84"
                                    ]
                                }]
                        };

                        var utilizationDEV = new Chart(companyDEVutilization, {
                            type: 'pie',
                            data: companyDEVutilizationData
                        }); //kraj DEV utilization chart

                        //UXUI utilization
                        var companyUXUIutilization = document.getElementById("companyUXUIutilization");

                        var companyUXUIutilizationData = {
                            labels: ['UXUI utilization', '36.36'],
                            datasets: [
                                {
                                    data: [$scope.report.uiuxUtilization, 100 - $scope.report.uiuxUtilization],
                                    backgroundColor: [
                                        "#FF6384",
                                        "#63FF84"
                                    ]
                                }]
                        };

                        var utilizationUXUI = new Chart(companyUXUIutilization, {
                            type: 'pie',
                            data: companyUXUIutilizationData
                        }); //kraj UXUI utilization chart

                        //QA utilization
                        var companyQAutilization = document.getElementById("companyQAutilization");

                        var companyQAutilizationData = {
                            labels: ['QA utilization', '19.09'],
                            datasets: [
                                {
                                    data: [$scope.report.qaUtilization, 100 - $scope.report.qaUtilization],
                                    backgroundColor: [
                                        "#FF6384",
                                        "#63FF84"
                                    ]
                                }]
                        };

                        var utilizationQA = new Chart(companyQAutilization, {
                            type: 'pie',
                            data: companyQAutilizationData
                        }); //kraj QA utilization chart

                        //Overtime
                        new Chart(document.getElementById("companyOvertime"), {
                            type: 'bar',
                            data: {
                                labels: listOfTeams,
                                datasets: [
                                    {
                                        label: "Overtime",
                                        //backgroundColor: perMemberMissingHoursColor,
                                        data: listOfOvertimeHoursByTeam
                                    }
                                ]
                            },
                            options: {
                                legend: {display: false},
                                title: {
                                    display: true,
                                    text: 'Overtime hours by Team'
                                }
                            }
                        }); //kraj Overtime chart

                        //Reveneu
                        new Chart(document.getElementById("companyRevenue"), {
                            type: 'bar',
                            data: {
                                labels: listOfProjects,
                                datasets: [
                                    {
                                        label: "Revenue",
                                        //backgroundColor: perMemberMissingHoursColor,
                                        data: listOfRevenuesByTeam
                                    }
                                ]
                            },
                            options: {
                                legend: {display: false},
                                title: {
                                    display: true,
                                    text: 'Revenue by Team'
                                }
                            }
                        }); //kraj Revenue chart
                */
                    })
                }
            }
            $scope.companyReport();
        }
    }]);
}());