(function(){
    var app = angular.module("timeKeeper");

    app.controller("companyDashboardController", ['$scope','dataService', 'timeConfig', function($scope, dataService, timeConfig) {
        $scope.months = timeConfig.months;
        $scope.month = 6;
        $scope.year = 2017;
        $scope.showing = false;

        $scope.companyReport = function () {
            if ($scope.year == undefined) {
                $scope.showing = false;
                window.alert("Choose year!");
            }
            else if ($scope.month == undefined) {
                $scope.showing = false;
                window.alert("Choose month!");
            }
            else {
                dataService.list("reports/CompanyReport?" + "year=" + $scope.year + "&month=" + $scope.month, function (data) {

                    $scope.report = data;
                    $scope.showing = true;
                    $scope.year = data.year;
                    $scope.month = data.month;

                    console.log($scope.report);

                    var workingHoursCanvas = document.getElementById("companyTotalHoursChart");

                    Chart.defaults.global.defaultFontFamily = "Lato";
                    Chart.defaults.global.defaultFontSize = 18;

                    var companyTotalHours = {
                        labels: ["Worked Hours", "Total Hours Possible: Worked: " + $scope.report.maxPossibleHours + " + NotWorked: " + Math.abs(($scope.report.maxPossibleHours)-$scope.report.totalHours) + " = " + $scope.report.totalHours],
                        datasets: [
                            {
                                data: [$scope.report.maxPossibleHours, Math.abs(($scope.report.maxPossibleHours)-$scope.report.totalHours)],
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

                    angular.forEach($scope.report.teams, function(team){
                        listOfTeams.push(team.name);
                        listOfMissingEntriesByTeam.push(team.missingEntries);
                        listOfOvertimeHoursByTeam.push(team.overtimeHours);
                    });
                    angular.forEach($scope.report.projects, function(project){
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
                            legend: { display: false },
                            title: {
                                display: true,
                                text: 'Missing Entries by Team'
                            }
                        }
                    }); //kraj missing entries

                    //PM utilization
                    var companyPMutilization = document.getElementById("companyPMutilization");

                    var companyPMutilizationData = {
                        labels: ['PM utilization','14.77'],
                        datasets: [
                            {
                                data: [$scope.report.pmUtilization, 100-$scope.report.pmUtilization],
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
                        labels: ['DEV utilization','17.53'],
                        datasets: [
                            {
                                data: [$scope.report.devUtilization, 100-$scope.report.devUtilization],
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
                        labels: ['UXUI utilization','36.36'],
                        datasets: [
                            {
                                data: [$scope.report.uiuxUtilization, 100-$scope.report.uiuxUtilization],
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
                        labels: ['QA utilization','19.09'],
                        datasets: [
                            {
                                data: [$scope.report.qaUtilization, 100-$scope.report.qaUtilization],
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
                            legend: { display: false },
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
                            legend: { display: false },
                            title: {
                                display: true,
                                text: 'Revenue by Team'
                            }
                        }
                    }); //kraj Revenue chart

                })
            }
        }

    }]);

}());