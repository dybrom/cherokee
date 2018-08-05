(function(){

    var app = angular.module("timeKeeper");

    // app.controller("employeesController", ["$scope", "dataService", function($scope, dataService) {
    //     var $emp = this;
    //     dataService.list("employees", function(data,headers){
    //         $scope.page = JSON.parse(headers('pagination'));
    //
    //         $scope.message = "";
    //         $scope.people = data;
    //         $scope.totalItems = $scope.page.TotalItems;
    //
    //     });
    //
    // }]);

    app.controller("employeesController", ['$scope', '$uibModal', 'dataService', function($scope, $uibModal, dataService){
        var $emp = this;
        $scope.showPagination = true;
        $scope.currentPage = 0;
        dataService.list("employees", function(data,headers){
            $scope.page = JSON.parse(headers('pagination'));

            $scope.message = "";
            $scope.people = data;
            $scope.totalItems = $scope.page.TotalItems;

        });
        $scope.edit = function(data){
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy : 'emp-modal-title',
                ariaLabelledBy : 'emp-modal-body',
                templateUrl : 'views/modals/empModal.html',
                controller : 'EmpModalCtrl',
                controllerAs : '$emp',
                resolve: {
                    person : function(){
                        return data;
                    }
                }
            });
        };
        $scope.filter = function (filter) {
            if (filter != "") {
                dataService.list("employees?pageSize=50&filter=" + filter, function (data, headers) {
                    $scope.people = data;
                    $scope.showPagination = false;

                })
            }
            else if(filter == ""){
                dataService.list("employees", function (data, headers) {
                    $scope.people = data;
                    $scope.showPagination = true;
                })
            }
        }
        $scope.add = function(){
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy : 'emp-modal-title',
                ariaLabelledBy : 'emp-modal-body',
                templateUrl : 'views/modals/empModal.html',
                controller : 'EmpModalCtrl',
                controllerAs : '$emp',
                resolve: {
                    person : function(){
                    }
                }
            });
        };

        $scope.delete = function(data){
            var r = window.confirm("Press a button");
            if(r==true) {
                dataService.delete("employees", data.id, function (data) {
                    window.alert("Successfully deleted!");

                });
            }
        }


        $scope.pageChanged = function() {

            var page = $scope.currentPage - 1;
            dataService.list("employees?page=" + page, function(data, headers){
                $scope.people = data;
            });
            console.log('Page changed to: ' + $scope.currentPage);
        };


    }]);

    app.controller('EmpModalCtrl', ['$uibModalInstance', '$scope', 'person', 'dataService',
        function($uibModalInstance, $scope, person, dataService){
        var $emp = this;
        dataService.list("roles", function (data) {
           $scope.roles = data;
        });
        console.log(person);
        $scope.person = person;
        $scope.oldPerson = angular.copy(person);
        $scope.ok = function(person) {
            if(person.id==undefined){
                dataService.insert("employees", person, function(){
                    window.alert("Employee successfully inserted!");
                    $uibModalInstance.close();
                });
            }
            else{
                dataService.update("employees", person.id, person, function(data){
                    window.alert("Employee successfully updated!");
                    $uibModalInstance.close();
                })
            }
        }
        $scope.cancel = function(person){
            angular.copy($scope.oldPerson, person);
            $uibModalInstance.dismiss();
        }
    }]);


}());