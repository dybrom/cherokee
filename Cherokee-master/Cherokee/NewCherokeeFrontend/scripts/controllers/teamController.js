(function() {

    var app = angular.module("timeKeeper");

    app.controller("teamController", ["$scope", "dataService", "$location", "$rootScope", "$uibModal", 
                             function ($scope,   dataService,   $location,   $rootScope,   $uibModal) {
        if($scope.currentUser.role != 'Administrator' || $scope.teamId == 0){
          $location.path('/calendar');
        } else {
            listTeam();

            $scope.$on('membersUpdated', function(event) {
                listTeam();
            });

            function listTeam() {
                dataService.read("teams", $scope.teamId, function (data) {
                    $scope.team = data;
                })
            }
        }

        $scope.view = function(data) {
            $location.path("/profile");
            $rootScope.currentUser.profileId = data.employee.id;
            dataService.read("employees", data.employee.id, function(data){
              $scope.person = data;
            });
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
             }).closed.then(function(){$scope.$emit("membersUpdated")});
         };

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
                             $scope.$emit('membersUpdated');
                         });
                         swal.close();
                     }
                 });
         }
     }]);

    app.controller('TmModalCtrl', ["$uibModalInstance", "$scope", "team" , "dataService",
                          function ($uibModalInstance,   $scope,   team,    dataService) {

        var $tm = this;

        $scope.team = team;
        $scope.oldteam = angular.copy(team);

        $scope.ok = function (team) {
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

    app.controller('TmEmpModalCtrl', ["$uibModalInstance", "$scope", "member" , "dataService",
                             function ($uibModalInstance,   $scope,   member,    dataService) {
            var $tm = this;

            dataService.list('roles', function(data){
                $scope.roles = data;
            });

            $scope.member = member;
            $scope.oldMember = angular.copy(member);

            $scope.save = function (member) {
                dataService.update("members", member.id, member, function(data){
                    $uibModalInstance.close();
                });
            };

            $scope.cancel = function (member) {
                angular.copy($scope.oldMember, member);
                $uibModalInstance.dismiss();
            };
    }]);

    app.controller('TmAddEmpModalCtrl', ["$uibModalInstance", "$scope", "member" , "dataService",
                                function ($uibModalInstance,   $scope,   member,    dataService) {
            var $tm = this;

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
                    role : {id: member.role.id},
                    team : { id: member.id },
                    employee: {id: member.employee.id},
                    hours : member.hours
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