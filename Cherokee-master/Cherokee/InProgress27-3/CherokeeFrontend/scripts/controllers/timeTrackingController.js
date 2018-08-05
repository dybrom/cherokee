(function(){

    var app = angular.module("timeKeeper");

    app.controller("timeTrackingController", ["$scope", "$uibModal", "dataService", "timeConfig",
        function($scope, $uibModal, dataService, timeConfig) {

            //$scope.year = new Date().getFullYear();
            //$scope.month = new Date().getMonth();
            $scope.dayType = timeConfig.dayType;
            $scope.months = timeConfig.months;
            $scope.month = 6;
            dataService.list("employees/all", function(data){
                $scope.people = data;
            });

            $scope.buildCalendar = function() {
                if($scope.empId === undefined)
                    window.alert('You have to choose an employee');
                else
                    listCalendar($scope.empId, $scope.year, $scope.month + 1);
            };

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
                   // console.log($scope.calendar);
                    //console.log($scope.empId);
                    //console.log($scope.year);
                    //console.log($scope.month);

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
        dataService.list("projects", function(data){
            $scope.projects = data;
            console.log($scope.day);
        });
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