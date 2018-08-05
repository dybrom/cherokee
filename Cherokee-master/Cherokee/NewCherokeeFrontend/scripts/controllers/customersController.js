(function(){

    var app = angular.module("timeKeeper");

    app.controller("customersController", ["$scope", "$uibModal", "$location", "$rootScope", "dataService",
                                   function($scope,   $uibModal,   $location,   $rootScope,   dataService) {

        if($scope.currentUser.role!= 'Administrator'){
            $location.path('/calendar');
        } else {
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

        $scope.view = function(data) {
            console.log(data);
            $location.path("/customer");
            $rootScope.customerId = data.id;
        };

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
        };

        $scope.add = function() {
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'views/modals/custModalAdd.html',
                controller: 'custModalAddCtrl',
                controllerAs: '$cust',
                resolve: {
                    customer: function () {
                    }
                }
            });
        }

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

    app.controller('custModalCtrl', ["$uibModalInstance", "$scope", "customer" , "dataService",
                            function ($uibModalInstance,   $scope,   customer,    dataService) {

        var $cust = this;

        $scope.customer = customer;
        $scope.oldcustomer = angular.copy(customer);

        $scope.save = function(customer){
            dataService.update("customers", customer.id, customer, function(data){
                window.alert("Customer updated!");
                $scope.$emit('customersUpdated');
                $uibModalInstance.close();
            });
        };

        $scope.cancel = function () {
            angular.copy($scope.oldcustomer, customer);
            $uibModalInstance.dismiss();
        };
    }]);

    app.controller('custModalAddCtrl', ["$uibModalInstance", "$scope", "customer" , "dataService",
                               function ($uibModalInstance,   $scope,   customer,    dataService) {
        var $cust = this;

        $scope.customer = customer;

        $scope.add = function(customer){
            dataService.insert("customers", customer, function(data){
                window.alert("Customer inserted!");
                $scope.$emit('customersUpdated');
                $uibModalInstance.close();
            });
        }

        $scope.cancel = function () {
            angular.copy($scope.oldcustomer, customer);
            $uibModalInstance.dismiss();
        };
    }]);
}());