(function() {

    var app = angular.module("timeKeeper");

    app.controller("profileController", ["$scope", "dataService", "$uibModal", "$location",
                                 function($scope,   dataService,   $uibModal,   $location) {

        if(currentUser.id == currentUser.profileId) {
            dataService.read("employees", currentUser.id, function (data) {
                $scope.person = data;
            });
        } else {
            dataService.read("employees", currentUser.profileId, function(data){
                $scope.person = data;
                currentUser.profileId = currentUser.id;
            })
        }

        $scope.edit = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'emp-modal-title',
                ariaDescribedBy: 'emp-modal-body',
                templateUrl: 'views/modals/empModal.html',
                controller: 'EmpModalCtrl',
                controllerAs: '$emp',
                resolve: {
                    person: function () {
                        return data;
                    }
                }
            })
        };

        $scope.delete = function (data) {
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
                            $location.path("/employees");
                        });
                        swal.close();
                    }
                });
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
                        window.alert("Employee successfully updated!");
                        if(currentUser.id == currentUser.profileId) {
                            dataService.read("employees", currentUser.id, function (data) {
                                $scope.person = data;
                            });
                        } else {
                            dataService.read("employees", currentUser.profileId, function(data){
                                $scope.person = data;
                                currentUser.profileId = currentUser.id;
                            })
                        }
                    })
                $uibModalInstance.close();
            };

            $scope.cancel = function(person){
                angular.copy($scope.oldPerson, person);
                $uibModalInstance.dismiss();
            }
        }]);
}());