(function(){

    var app = angular.module("timeKeeper");

    app.controller("timeTrackingController", ["$scope", "$uibModal", "dataService", "timeConfig",
                                      function($scope,   $uibModal,   dataService,   timeConfig) {
            $scope.empId = currentUser.id;
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.month = 3;
            $scope.year = 2018;
            $scope.showEmployees = false;
            $scope.people=[];
            $scope.missingEntries = [];

            if($scope.currentUser.role=='Administrator') {
                $scope.showEmployees=true;
                dataService.list("employees/all", function (data) {
                    $scope.people = data;
                });

            }
            else if($scope.currentUser.teams.length>0){
                $scope.showEmployees=true;
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
            }
            else {
                $scope.empId = $scope.currentUser.id;
            }

            $scope.buildCalendar = function () {
                if ($scope.empId === undefined)
                    window.alert('You have to choose an employee');
                else {
                    listCalendar($scope.empId, $scope.year, $scope.month + 1);

                }
            };
            $scope.buildCalendar();

            $scope.$on('calendarUpdated', function(event) {
                listCalendar($scope.empId, $scope.year, $scope.month + 1);
            });

            function listCalendar(empId, year, month) {
                var url = "calendar?id=" + empId;
                if(year != 'undefined') url += "&year=" + year;
                if(month != 'undefined') url += "&month=" + month;
                dataService.list(url, function(data){
                    $scope.calendar = data;
                    $scope.empId = data.employee.id;
                    $scope.year = data.year;
                    $scope.month = data.month - 1;

                    $scope.num = function(){
                        var size = new Date(data.days[0].date).getDay() - 1;
                        if (size < 0) size = 6;
                        return new Array(size);
                    }
                });





            };

            $scope.edit = function(day) {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'views/modals/calendarModal.html',
                    controller: 'ModalCalendarCtrl',
                    size: 'lg',
                    resolve: {
                        day: function () {
                            return day;
                        }
                    }
                });
            }
        }]);

    app.controller('ModalCalendarCtrl',["$uibModalInstance", "$scope",  "dataService", "timeConfig", "day",
        function ($uibModalInstance, $scope, dataService, timeConfig, day) {
            $scope.day = day;
            $scope.dayType = timeConfig.dayDesc;
            $scope.projects = [];

            dataService.read("employees", day.employee.id, function(data){
               var employee = data;
                for(i=0;i<employee.projects.length;i++){
                    $scope.projects.push(employee.projects[i]);
                }
                console.log($scope.projects);
            })


            initNewTask();

            $scope.add = function(task){
                $scope.day.details.push(task);
                sumHours();
                initNewTask();
            };

            $scope.upd = function(task, index) {
                sumHours();
            };

            $scope.del = function(index) {
                $scope.day.details[index].deleted = true;
                sumHours();
            };

            function sumHours() {
                $scope.day.hours = 0;
                for(var i=0; i<$scope.day.details.length; i++) {
                    if(!$scope.day.details[i].deleted) $scope.day.hours += Number($scope.day.details[i].hours);
                }
            }

            function initNewTask() {
                $scope.newTask = {id: 0, description: '', hours: 0, project: {id: 0, name: ''}, deleted: false};
            }

            $scope.ok = function () {
                dataService.insert("calendar", $scope.day, function(data){
                    $scope.$emit('calendarUpdated');

                });
                sumHours();
                $uibModalInstance.close();
            };

            $scope.cancel = function () {
                $uibModalInstance.dismiss();
            };

            $scope.typeChanged = function() {
                if($scope.day.type != 1) {
                    $scope.day.hours = 8;
                }
            }
        }]);
}());