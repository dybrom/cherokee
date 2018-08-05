(function(){

    var app = angular.module("timeKeeper");

    app.controller("teamsController", ["$scope", "$uibModal", "dataService", function($scope, $uibModal, dataService) {

        $scope.message = "Wait...";
        dataService.list("teams", function(data){
            $scope.message = "";
            $scope.teams = data;
        });

        var $tm = this;
        $scope.$on('teamsUpdated', function(event) {
            dataService.list("teams", function(data){
                console.log("nest");
                $scope.message = "";
                $scope.teams = data;

            });
        });
        $scope.$on('membersUpdated', function(event) {
            dataService.list("members", function(data){
                console.log("nest");
                $scope.message = "";
                $scope.teams = data;

            });
        });

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
            }).closed.then(function(){$scope.$emit("teamsUpdated")});
        }
    }]);

    app.controller('TmNewTeamModalCtrl', ["$uibModalInstance", "$scope",
        "team" , "dataService",function ($uibModalInstance, $scope, team, dataService) {
            var $tm = this;
            $scope.team = team;

            $scope.add = function (team) {
                dataService.insert("teams", team, function(data){
                    $scope.$emit('teamsUpdated');
                    if($uibModalInstance.close()) window.alert("Team added!");


                });


            };
            $scope.cancel = function (team) {
                $uibModalInstance.dismiss();
            };
        }]);

    app.controller("tmController", ["$scope", "$uibModal", "dataService", function($scope, $uibModal, dataService) {

        var $tm = this;

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

        $scope.addMember = function(data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'tm-add-emp-modal-title',
                ariaDescribedBy: 'tm-add-emp-modal-body',
                templateUrl: 'views/modals/tmAddEmpModal.html',
                controller: 'TmAddEmpModalCtrl',
                controllerAs: '$tm',
                resolve: {
                    member: function () {
                        return data;
                    }
                }
            }).closed.then(function(){$scope.$emit("teamsUpdated")});
        }

        $scope.editMember = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'tm-emp-modal-title',
                ariaDescribedBy: 'tm-emp-modal-body',
                templateUrl: 'views/modals/tmEmpModal.html',
                controller: 'TmEmpModalCtrl',
                controllerAs: '$tm',
                resolve: {
                    member: function () {
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
        }


        $scope.deleteMember = function(data) {
            swal({
                    title: data.employee.name,
                    text: "Are you sure you want to delete this employee from team " + data.team.name + "?",
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
                        dataService.delete("members", data.id, function (data) {
                            $scope.$emit('teamsUpdated');

                        });
                        swal.close();
                    }
                });
        }
    }]);

    app.controller('TmModalCtrl', ["$uibModalInstance", "$scope", "team" , "dataService",function ($uibModalInstance, $scope, team, dataService) {
        var $tm = this;
        console.log(team);
        $scope.team = team;
        $scope.oldteam = angular.copy(team);
        $scope.ok = function (team) {
            dataService.update("teams", team.id, team, function(data){
                $scope.$emit('teamsUpdated');
                $uibModalInstance.close();

            });

        };
        $scope.cancel = function (team) {
            //angular.copy($scope.oldteam, team);
            $uibModalInstance.dismiss();
        };
    }]);

    app.controller('TmEmpModalCtrl', ["$uibModalInstance", "$scope", "member" , "dataService",function ($uibModalInstance, $scope, member, dataService) {
        var $tm = this;
        console.log(member);

        dataService.list('roles', function(data){
            $scope.roles = data;
        });

        $scope.member = member;
        $scope.oldMember = angular.copy(member);
        $scope.ok = function (member) {
            dataService.update("members", member.id, member, function(data){
                $uibModalInstance.close();
            });

        };
        $scope.cancel = function (member) {
            angular.copy($scope.oldMember, member);
            $uibModalInstance.dismiss();
        };
    }]);

    app.controller('TmAddEmpModalCtrl', ["$uibModalInstance", "$scope",
        "member" , "dataService",function ($uibModalInstance, $scope, member, dataService) {
            var $tm = this;
            console.log(member);
            $scope.member = member;
            $scope.oldteam = angular.copy(member);

            dataService.list("employees/all", function(data){
                $scope.employees = data;
            });
            dataService.list("roles", function(data) {
                $scope.roles = data;
            });

            $scope.add = function (member) {
                var newMember = {
                    Role :  {Id: member.role.id},
                    Team :{ Id: member.id },
                    Employee: {Id: member.employee.id},
                    hours: 40
                };
                console.log(newMember);
                dataService.insert("members", newMember, function(data){
                    if($uibModalInstance.close()) window.alert("Member added!");
                });

            };

            $scope.cancel = function (member) {
                angular.copy($scope.oldteam, member);
                $uibModalInstance.dismiss();
            };
        }]);
}());