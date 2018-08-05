(function(){

    var app = angular.module("timeKeeper");

    app.controller("employeesController", ["$scope", "dataService",'$location',
                                   function($scope,   dataService,  $location) {
        var $emp = this;

        if($scope.currentUser.role!='Administrator'){
            $location.path('/calendar');
        } else {
            $scope.$on('employeesUpdated', function() {
                dataService.list("employees", function(data){
                    $scope.people = data;
                });
            });
        }
    }]);

    app.controller("empController", ['$scope', '$rootScope', '$uibModal', 'dataService', '$location',
                             function($scope,   $rootScope,   $uibModal,   dataService,   $location){
        var $emp = this;

        $scope.showPagination = true;
        $scope.currentPage = 0;

        dataService.list("employees", function(data,headers){
            $scope.page = JSON.parse(headers('pagination'));
            console.log($scope.page);
            $scope.people = data;
            $scope.totalItems = $scope.page.TotalItems;
        });

        $scope.view = function(data) {
            $location.path("/profile");
            $rootScope.currentUser.profileId = data.id;
        };

        $scope.edit = function(data){
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy : 'emp-modal-title',
                ariaDescribedBy : 'emp-modal-body',
                templateUrl : 'views/modals/empModal.html',
                controller : 'EmpModalCtrl',
                controllerAs : '$emp',
                resolve: {
                    person : function(){
                        return data;
                    }
                }
            }).closed.then(function() {$scope.$emit("employeesUpdated")});
        };

        $scope.filter = function(filter) {
            if (filter != "") {
                dataService.list("employees?pageSize=50&filter=" + filter, function (data, headers) {
                    $scope.people = data;
                    $scope.showPagination = false;
                })
            } else if (filter == "") {
                dataService.list("employees", function (data, headers) {
                    $scope.people = data;
                    $scope.showPagination = true;
                })
            }
        };

        $scope.add = function(){
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy : 'emp-modal-title',
                ariaDescribedBy : 'emp-modal-body',
                templateUrl : 'views/modals/empAddModal.html',
                controller : 'EmpAddModalCtrl',
                controllerAs : '$emp',
                resolve: {
                    person : function(){
                    }
                }
            }).closed.then(function(){$scope.$emit("employeesUpdated")});
        };

        $scope.delete = function(data) {
            swal({
                    title: data.name,
                    text: "Are you sure you want to delete this employee?",
                    type: "warning",
                    showCancelButton: true,
                    customClass: "sweetClass",
                    confirmButtonText: "Yes, sure",
                    cancelButtonText: "No, not ever!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },

                function (isConfirm) {
                    if (isConfirm) {
                        dataService.delete("employees", data.id, function (data) {
                            window.alert("Employee deleted!");
                            $scope.$emit('employeesUpdated');
                        });
                        swal.close();
                    }
                });
        };

        $scope.pageChanged = function() {
            var page = $scope.currentPage - 1;
            dataService.list("employees?page=" + page, function(data, headers){
                $scope.people = data;
            });
            console.log('Page changed to: ' + $scope.currentPage);
        };


    }]);

    app.controller('EmpModalCtrl', ['$uibModalInstance', '$scope', 'person', 'dataService',
                            function($uibModalInstance,   $scope,   person,   dataService){
        var $emp = this;

        dataService.list("roles", function (data) {
            $scope.roles = data;
        });

        $scope.person = person;
        $scope.oldPerson = angular.copy(person);

        $scope.save = function(person) {
                person.image = ($scope.person.image)?$scope.person.image.base64:"";
                dataService.update("employees", person.id, person, function(data){
                    $scope.$emit("employeesUpdated");
                    window.alert("Employee successfully updated!");
                });
            $uibModalInstance.close();
        };

        $scope.cancel = function(person){
            angular.copy($scope.oldPerson, person);
            $uibModalInstance.dismiss();
        }
    }]);

    app.controller('EmpAddModalCtrl', ['$uibModalInstance', '$scope', 'person', 'dataService',
                               function($uibModalInstance,   $scope,   person,   dataService){
            var $emp = this;

            dataService.list("roles", function (data) {
                $scope.roles = data;
            });

            $scope.person = person;

            $scope.add = function(person) {
                person.image = ($scope.person.image)?$scope.person.image.base64:"";
                dataService.insert("employees", person, function(){
                    $scope.$emit("employeesUpdated");
                    window.alert("Employee successfully inserted!");
                });
                $uibModalInstance.close();
            };

            $scope.cancel = function(){
                $uibModalInstance.dismiss();
            }
        }]);
}());