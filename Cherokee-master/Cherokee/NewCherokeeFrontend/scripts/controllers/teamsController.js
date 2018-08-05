(function(){

    var app = angular.module("timeKeeper");

    app.controller("teamsController", ["$scope", "$uibModal", "dataService", "$location", "$rootScope",
                               function($scope,   $uibModal,   dataService,   $location,   $rootScope) {
        
        if ($scope.currentUser.role!='Administrator'){
            $location.path('/calendar');
        } else {
            listTeams();

            $scope.$on('teamsUpdated', function(event) {
                listTeams();
            });

            function listTeams() {
                dataService.list("teams", function(data, headers) {
                    $scope.teams = data;
                })
            }
        }

        $scope.add = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'tm-new-modal-title',
                ariaDescribedBy: 'tm-new-modal-body',
                templateUrl: 'views/modals/tmNewTeamModal.html',
                controller: 'TmNewTeamModalCtrl',
                controllerAs: '$tm',
                resolve: {
                    team: function() {
                    }
                }
            }).closed.then(function(){$scope.$emit("teamsUpdated")});;
        }

        $scope.view = function(data) {
            console.log(data);
            $location.path("/team");
            $rootScope.teamId = data.id;
        };

        $scope.edit = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'tm-modal-title',
                ariaDescribedBy: 'tm-modal-body',
                templateUrl: 'views/modals/tmModal.html',
                controller: 'TmModalCtrl',
                controllerAs: '$tm',
                resolve: {
                    team: function () {
                        return data;
                    }
                }
            });
        };

        $scope.delete = function(data) {
            swal({
                    title: data.name,
                    text: "Are you sure you want to delete this team?",
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
                        dataService.delete("teams", data.id, function (data) {
                            window.alert("Successfully deleted!");
                            $scope.$emit('teamsUpdated');
                        });
                        swal.close();
                    }
                });
        };
    }]);

    app.controller('TmNewTeamModalCtrl', ["$uibModalInstance", "$scope", "team" , "dataService",
                                 function ($uibModalInstance,   $scope,   team,    dataService) {

        var $tm = this;

        $scope.team = team;

        $scope.add = function (team) {
            dataService.insert("teams", team, function() {
                $scope.$emit('teamsUpdated');
                window.alert("Team added!");
            });
            $uibModalInstance.close();
        };

        $scope.cancel = function() {
            $uibModalInstance.dismiss();
        };
    }]);

    app.controller('TmModalCtrl', ["$uibModalInstance", "$scope", "team" , "dataService",
                          function ($uibModalInstance,   $scope,   team,    dataService) {

        var $tm = this;

        $scope.team = team;
        $scope.oldteam = angular.copy(team);

        $scope.save = function (team) {
            dataService.update("teams", team.id, team, function(data){
                $scope.$emit('teamsUpdated');
                $uibModalInstance.close();
            });
        };

        $scope.cancel = function (team) {
            angular.copy($scope.oldteam, team);
            $uibModalInstance.dismiss();
        };
    }]);
}());