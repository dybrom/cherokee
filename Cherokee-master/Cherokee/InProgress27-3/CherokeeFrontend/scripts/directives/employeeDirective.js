(function(){
    var app = angular.module("timeKeeper");

    app.directive("employee",[function(){
        return {
            restrict : 'E',
            scope : {
                people : '=employee'
            },
            controller : "empController as $emp",
            templateUrl : 'views/employees.html'
        }
    }]);


}());