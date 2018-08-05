(function() {

    var app = angular.module("timeKeeper");

    app.controller("projectController", ["$scope", "dataService", '$location', '$uibModal'
                              , function ($scope,   dataService,   $location,   $uibModal) {

        if($scope.currentUser.role != 'Administrator' || $scope.projectId == 0){
            $location.path('/calendar');
        }
        else {
            console.log($scope.projectId);
            dataService.read("projects", $scope.projectId, function(data) {
                $scope.project = data;
            })
        }

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
                            $location.path('/projects');
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
            if (project.id === undefined) {
                dataService.insert("projects", project, function(data) {
                    window.alert("Project inserted!");
                    $uibModalInstance.close();
                });
            } else {
                dataService.update("projects", project.id, project, function(data) {
                    $scope.$emit('projectsUpdated');
                    window.alert("Project updated!");
                    $uibModalInstance.close();
                });
            }
        };

        $scope.cancel = function() {
            angular.copy($scope.oldproject, project);
            $uibModalInstance.dismiss();
        };
    }]);
}());
