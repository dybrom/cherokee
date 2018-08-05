(function(){
    var app = angular.module("timeKeeper", ["ngRoute", "ui.bootstrap", "toaster", "ngAnimate","naif.base64"]);

    currentUser = {
        id: 0,
        name: '',
        teams: [],
        provider: '',
        token: ''
    };

    app.constant('timeConfig', {
        apiUrl: 'http://localhost:56111/api/',
        idsUrl: 'http://localhost:53834/connect/token',
        dayType: ['empty', 'workingday', 'publicholiday', 'otherabsence', 'religiousday', 'sickleave', 'vacation', 'businessabsence', 'weekend', 'future'],
        dayDesc: [' ', 'Working Day', 'Public Holiday', 'Other Absence', 'Religious Day', 'Sick Leave', 'Vacation', 'Business Absence'],
        months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
    });

    app.config(['$routeProvider', function($routeProvider) {
        $routeProvider
            .when('/login', {templateUrl: 'views/login.html', controller: 'loginController', loginR: false })
            .when('/logout', {templateUrl: 'views/login.html', controller: 'logoutController', loginR: true })
            .when('/teams',     { templateUrl: 'views/teams.html', controller: 'teamsController', loginR: true })
            .when('/employees', { templateUrl: 'views/employees.html', controller: 'employeesController', loginR:true })
            .when('/customers', { templateUrl: 'views/customers.html', controller: 'customersController', loginR:true })
            .when('/projects',  { templateUrl: 'views/projects.html', controller: 'projectsController',loginR: true })
            .when('/calendar',  { templateUrl: 'views/timeTracking.html', controller: 'timeTrackingController', loginR:true })
            .when('/personalreport', {templateUrl: 'views/personalReport.html', controller: 'personalReportController', loginR:true})
            .when('/annualreport', {templateUrl: 'views/annualReport.html', controller: 'annualReportController', loginR:true})
            .when('/monthlyreport', {templateUrl: 'views/monthlyReport.html', controller: 'monthlyReportController', loginR:true})
            .when('/projecthistory', {templateUrl: 'views/projectHistory.html', controller: 'projectHistoryController', loginR:true})
            .when('/teamdashboard', {templateUrl: 'views/teamDashboard.html', controller: 'teamDashboardController', loginR:true})
            .when('/companydashboard', {templateUrl: 'views/companyDashboard.html', controller: 'companyDashboardController', loginR:true})



            .otherwise({ redirectTo: '/login' });
    }]).run(['$rootScope', '$location', function($rootScope, $location){
        $rootScope.$on('$routeChangeStart', function (event, next, current){
            if (currentUser.id === 0 && next.$$route.loginR){
                $location.path("/login");
            }

        })
    }]);
}());