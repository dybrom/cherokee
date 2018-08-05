(function(){

    var app = angular.module("timeKeeper");

    app.controller("projectsController", ["$scope", "$uibModal", "dataService", "$rootScope", "$location",
                                  function($scope,   $uibModal,   dataService,   $rootScope,   $location) {

        if($scope.currentUser.role != 'Administrator') {
            $location.path('/calendar');
        } else {
            listProjects();

            $scope.$on('projectsUpdated', function(event) {
                listProjects();
            });

            function listProjects() {
                dataService.list("projects", function(data, headers) {
                    $scope.projects = data;
                })
            }
        }

        $scope.add = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/modals/projModalAdd.html',
                controller: 'projModalAdd',
                controllerAs: '$proj',
                resolve: {
                    project: function () {
                    }
                }
            });
        };

        $scope.view = function(data) {
            console.log(data);
            $location.path("/project");
            $rootScope.projectId = data.id;
        };

        $scope.edit = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/modals/projModal.html',
                controller: 'ModalCtrl',
                controllerAs: '$proj',
                resolve: {
                    project: function () {
                        return data;
                    }
                }
            });
        };

        $scope.delete = function(data) {
            swal({
                    title: data.name,
                    text: "Are you sure you want to delete this project?",
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
                        dataService.delete("projects", data.id, function (data) {
                            $scope.$emit('projectsUpdated');
                        });
                        swal.close();
                    }
                });
        }

    }]);

    app.controller('ModalCtrl', ["$uibModalInstance", "$scope", "project" , "dataService",function ($uibModalInstance, $scope, project,dataService) {
        var $proj = this;

        dataService.list("customers", function(data){
            $scope.customers = data;
        });

        dataService.list("teams", function(data){
            $scope.teams = data;
        });

        $scope.project = project;
        $scope.oldproject = angular.copy(project);

        $scope.save = function(project){
            dataService.update("projects", project.id, project, function(data) {
                $scope.$emit('projectsUpdated');
                window.alert("Project updated!");
                $uibModalInstance.close();
            });
        };

        $scope.cancel = function() {
            angular.copy($scope.oldproject, project);
            $uibModalInstance.dismiss();
        };
    }]);

    app.controller('projModalAdd', ["$uibModalInstance", "$scope", "project" , "dataService",
                           function ($uibModalInstance,   $scope,   project,    dataService) {
        var $proj = this;

        dataService.list("customers", function(data){
            $scope.customers = data;
        });

        dataService.list("teams", function(data){
            $scope.teams = data;
        });

        $scope.project = project;

        $scope.add = function(project) {
            dataService.insert("projects", project, function(data) {
                window.alert("Project inserted!");
                $uibModalInstance.close();
            });
        }

        $scope.cancel = function() {
            $uibModalInstance.dismiss();
        };
    }]);
}());