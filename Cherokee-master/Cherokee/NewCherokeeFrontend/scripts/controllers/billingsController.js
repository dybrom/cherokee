(function(){
    var app = angular.module("timeKeeper");

    app.controller("billingsController", ['$scope','dataService', 'timeConfig', '$uibModal',
        function($scope,  dataService,   timeConfig, $uibModal) {
            $scope.months = timeConfig.months;
            $scope.month = 6;
            $scope.year = 2017;
            $scope.showing = false;

            if($scope.currentUser.role!= 'Administrator'){
                $location.path('/calendar');
            } else {
                $scope.billingReport = function () {
                    if ($scope.year == undefined) {
                        $scope.showing = false;
                        window.alert("Choose year!");
                    } else if ($scope.month == undefined) {
                        $scope.showing = false;
                        window.alert("Choose month!");
                    } else {
                        dataService.list("reports/InvoiceReport?" + "year=" + $scope.year + "&month=" + ($scope.month + 1), function (data) {

                            $scope.billings = data;
                            $scope.showing = true;
                            $scope.year = data.year;
                            $scope.month = data.month;

                            console.log($scope.billings);

                            $scope.edit = function (data) {
                                var modalInstance = $uibModal.open({
                                    animation: true,
                                    ariaLabelledBy: 'tm-modal-title',
                                    ariaDescribedBy: 'tm-modal-body',
                                    templateUrl: 'views/modals/invoiceModal.html',
                                    controller: 'invoiceModalCtrl',
                                    controllerAs: '$bi',
                                    resolve: {
                                        invoice: function () {
                                            return data;
                                        }
                                    }
                                });
                            };


                        })
                    }
                }
            }
        }]);

    app.controller('invoiceModalCtrl', ["$uibModalInstance", "$scope", "invoice" , "dataService",
        function ($uibModalInstance,   $scope,   invoice,    dataService) {

            var $bi = this;

            $scope.invoice = invoice;
            console.log("unutar modala" + invoice);
            $scope.oldinvoice = angular.copy(invoice);

            $scope.cancel = function (invoice) {
                $uibModalInstance.dismiss();
            };

            var billingcount = 0;

            /*$scope.printDiv = function(divName) {
                var printContents = document.getElementById(divName).innerHTML;
                var popupWin = window.open('', '_blank', 'width=600,height=300');
                popupWin.document.open();

                popupWin.document.write('<html><head>' +
                    '<link rel="stylesheet" type="text/css" href="maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />' +
                    '<link rel="stylesheet" type="text/html" href="//raw.githubusercontent.com/bracikaa/AdminLTE/38b885bc27624195c4afa85a8535e283e3d46cd0/dist/css/AdminLTE.min.css"/>' +
                    '</head><body onload="window.print()">' + printContents + '</body></html>');
                popupWin.document.close();
            }*/

            /*$scope.DeleteInvoiceEntry = function(index){
                //$scope.invoice.roles.splice(index,1);
                $scope.invoice.roles[index].status = "Not Sent";
                //$scope.invoice.roles.status = "Not sent";
                console.log($scope.invoice);
            }
            $scope.StagnateInvoiceEntry = function(index){
                $scope.invoice.roles[index].status = "Canceled";
                console.log($scope.invoice);
            }*/
            /*$scope.IncludeInvoiceEntry = function(index){
                $scope.invoice.roles[index].status = "Sent";
                console.log($scope.invoice);
            }*/

            /*$scope.getTotal = function(){
                var total = 0;
                for(var i = 0; i < $scope.invoice.roles.length; i++){
                    var amount = $scope.invoice.roles["subTotal"];
                    total += amount;
                }
                return total;
            }*/

            $scope.DeleteInvoiceEntry = function(index){
                //$scope.invoice.roles.splice(index,1);
                $scope.invoice.roles[index].status = "Not included";
                console.log($scope.invoice);
            }
            /*$scope.StagnateInvoice = function(){
                console.log($scope.oldinvoice);
                dataService.insert("invoice", $scope.oldinvoice, function(data){
                    console.log("uspjesno stagniran");
                })
            }*/
            $scope.StagnateInvoice = function(){
                console.log($scope.invoice);
                $scope.invoice.status = "Canceled"
                dataService.insert("invoice", $scope.invoice, function(data){
                    console.log("uspjesno stagniran");
                })
            }

            $scope.SendInvoice = function () {
                //$scope.invoice = invoice;
                console.log($scope.invoice);

                var billingcount = 0;
                var totalbillingcount = 0;

                angular.forEach($scope.invoice.roles, function(role)
                {
                    if( role.status == "Included")
                        {billingcount++}

                    totalbillingcount++;
                })

                console.log(billingcount + " " + totalbillingcount);
                if ( billingcount == 0)
                {
                    $scope.invoice.status = "Not sent";
                }
                else if (billingcount > 0 && billingcount < totalbillingcount)
                {
                    $scope.invoice.status = "Partially sent";
                }
                else if (billingcount === totalbillingcount)
                {
                    $scope.invoice.status = "Full bill sent";
                }

                //$scope.invoice.status = "Sent"
                dataService.insert("invoice", $scope.invoice, function(data){
                    console.log("uspjesno");
                })
            }

        }]);
}());