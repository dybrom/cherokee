(function(){

    var app = angular.module("timeKeeper");

    app.controller("customersController", ["$scope", "dataService", '$location',  function($scope, dataService, $location) {
        if($scope.currentUser.role!= 'Administrator'){
            $location.path('/calendar');
        }
        else {


            listCustomers();

            $scope.$on('customersUpdated', function (event) {
                listCustomers();
            });

            function listCustomers() {
                dataService.list("customers", function (data) {
                    $scope.customers = data;
                });
            }
        }
    }]);

    app.controller("custController", ["$scope", "$uibModal", "dataService", function($scope, $uibModal, dataService) {

        var $cust = this;
        $scope.edit = function (data) {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/modals/custModal.html',
                controller: 'custModalCtrl',
                controllerAs: '$cust',
                resolve: {
                    customer: function () {
                        return data;
                    }
                }
            });
        }

        $scope.add = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/modals/custModal.html',
                controller: 'custModalCtrl',
                controllerAs: '$cust',
                resolve: {
                    customer: function () {
                        return;
                    }
                }
            }).closed.then(function(){$scope.$emit("customersUpdated");});
        }//dodao save ovde da otvara modal bez podataka

        $scope.delete = function(data) {
            swal({
                    title: data.name,
                    text: "Are you sure you want to delete this customer?",
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
                        dataService.delete("customers", data.id, function (data) {
                            $scope.$emit('customersUpdated');

                        });
                        swal.close();
                    }
                });
        }

    }]);
    //app.controller('ModalCtrl',["$uibModalInstance", "$scope", "customer", function ($uibModalInstance, $scope, customer)
    //app.controller('ModalCtrl', function ($uibModalInstance, $scope, customer) {

    app.controller('custModalCtrl', ["$uibModalInstance", "$scope", "customer" , "dataService",function ($uibModalInstance, $scope, customer,dataService) {
        var $cust = this;
        console.log(customer);
        $scope.customer = customer;

        $scope.oldcustomer = angular.copy(customer);

        /*$scope.save = function(customer){
                dataService.update("customers", customer.id, customer, function(data){
                    window.alert("Data updated!");
                });
        };*/

        $scope.save = function(customer){
            console.log(customer);
            if(customer.id === undefined){
                dataService.insert("customers", customer, function(data){
                    window.alert("customer inserted!");
                    $uibModalInstance.close();
                });
            }
            else{
                dataService.update("customers", customer.id, customer, function(data){
                    window.alert("customer updated!");
                    $uibModalInstance.close();
                });
            }
        }// dodao sta radi dugme save unutar modala

        /*$scope.ok = function () {
            $uibModalInstance.close();
        };*/

        $scope.cancel = function () {
            angular.copy($scope.oldcustomer, customer);
            $uibModalInstance.dismiss();
        };
    }]);



}());