(function(){

    var app = angular.module("timeKeeper");

    app.controller("projectsController", ["$scope", "dataService",  function($scope, dataService) {

        $scope.message = "Wait...";
        dataService.list("projects", function(data){
            $scope.message = "";
            $scope.projects = data;
        });
    }]);

    app.controller("projController", ["$scope", "$uibModal", "dataService", function($scope, $uibModal, dataService) {

        var $proj = this;
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
        }

        $scope.save = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/modals/projModal.html',
                controller: 'ModalCtrl',
                controllerAs: '$proj',
                resolve: {
                    project: function () {
                        return;
                    }
                }
            });
        }

        $scope.add = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/modals/projModalAdd.html',
                controller: 'ModalCtrl',
                controllerAs: '$proj',
                resolve: {
                    project: function () {
                        return data;
                    }
                }
            });
        }

        $scope.delete = function(data){
            //console.log(project);
            if(data.id != undefined){
                dataService.delete("projects", data.id, function(data){
                    window.alert("Project deleted!");
                });
            }
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

        console.log(project);
        $scope.project = project;

        $scope.oldproject = angular.copy(project);



        $scope.save = function(project){
            console.log(project);
            if(project.id === undefined){
                dataService.insert("projects", project, function(data){
                    window.alert("Project inserted!");
                });
            }
            else{
                dataService.update("projects", project.id, project, function(data){
                    window.alert("Project updated!");
                });
            }
        }

        $scope.cancel = function () {
            angular.copy($scope.oldproject, project);
            $uibModalInstance.dismiss();
        };
    }]);

}());