(function(){
    var app = angular.module("timeKeeper", ["ngRoute", "ui.bootstrap"]);

    app.constant('timeConfig', {
        apiUrl: 'http://localhost:56111/api/',
        idsUrl: 'http://localhost:9002/connect/token',
        dayType: ['empty', 'workingday', 'publicholiday', 'otherabsence', 'religiousday', 'sickleave', 'vacation', 'businessabsence', 'weekend', 'future'],
        dayDesc: [' ', 'Working Day', 'Public Holiday', 'Other Absence', 'Religious Day', 'Sick Leave', 'Vacation', 'Business Absence'],
        months: ['jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec'],
        years: ['2016', '2017', '2018']
    });

    app.config(['$routeProvider', function($routeProvider) {
        $routeProvider
            .when('/dashboard', { templateUrl: 'views/dashboard.html', controller: 'timeTrackingController'})
            .when('/teams',     { templateUrl: 'views/teams.html', controller: 'teamsController' })
            .when('/employees', { templateUrl: 'views/employees.html', controller: 'employeesController' })
            .when('/customers', { templateUrl: 'views/customers.html', controller: 'customersController' })
            .when('/projects',  { templateUrl: 'views/projects.html', controller: 'projectsController' })
            .when('/calendar',  { templateUrl: 'views/timeTracking.html', controller: 'timeTrackingController' })
            .when('/invoices',  { templateUrl: 'views/invoices.html', controller: 'timeTrackingController' })
            .otherwise({ redirectTo: '/dashboard' });
    }]);
}());