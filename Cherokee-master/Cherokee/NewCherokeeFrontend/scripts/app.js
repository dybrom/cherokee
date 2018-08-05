(function(){
    var app = angular.module("timeKeeper", ["ngRoute", "ui.bootstrap", "toaster", "ngAnimate", "naif.base64", "chart.js"]);

    currentUser = {
        id: 0
    };
    projectId = 0;
    teamId = 0;
    customerId = 0;

    var baseUrl = "#{ApiUrl}";
    var baseIdsUrl = "#{AuthServer}";

    app.constant('timeConfig', {
        apiUrl: (baseUrl[0] !== "#") ? baseUrl : 'http://localhost:56111/api/',
        idsUrl: (baseIdsUrl[0] !== "#") ? baseIdsUrl + '/connect/token' : 'http://localhost:53834/connect/token',
        dayType: ['empty', 'workingday', 'publicholiday', 'otherabsence', 'religiousday', 'sickleave', 'vacation', 'businessabsence', 'weekend', 'future'],
        dayDesc: [' ', 'Working Day', 'Public Holiday', 'Other Absence', 'Religious Day', 'Sick Leave', 'Vacation', 'Business Absence'],
        months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
    });

    app.config(['$routeProvider',  function($routeProvider) {
        $routeProvider
            .when('/teams',     { templateUrl: 'views/teams.html', controller: 'teamsController', loginR: true })
            .when('/team',     { templateUrl: 'views/team.html', controller: 'teamController', loginR: true })
            .when('/profile',     { templateUrl: 'views/profile.html', controller: 'profileController', loginR: true })
            .when('/employees', { templateUrl: 'views/employees.html', controller: 'employeesController', loginR:true })
            .when('/customers', { templateUrl: 'views/customers.html', controller: 'customersController', loginR:true })
            .when('/customer',     { templateUrl: 'views/customer.html', controller: 'customerController', loginR: true })
            .when('/projects',  { templateUrl: 'views/projects.html', controller: 'projectsController',loginR:true })
            .when('/project',     { templateUrl: 'views/project.html', controller: 'projectController', loginR: true })
            .when('/calendar',  { templateUrl: 'views/timeTracking.html', controller: 'timeTrackingController', loginR:true })
            .when('/login', {templateUrl: 'views/login.html', controller: 'loginController', loginR:false })
            .when('/logout', {templateUrl: 'views/login.html', controller: 'logoutController', loginR:false })
            .when('/personalreport', {templateUrl: 'views/personalReport.html', controller: 'personalReportController', loginR:true})
            .when('/annualreport', {templateUrl: 'views/annualReport.html', controller: 'annualReportController', loginR:true})
            .when('/monthlyreport', {templateUrl: 'views/monthlyReport.html', controller: 'monthlyReportController', loginR:true})
            .when('/projecthistory', {templateUrl: 'views/projectHistory.html', controller: 'projectHistoryController', loginR:true})
            .when('/teamdashboard', {templateUrl: 'views/teamDashboard.html', controller: 'teamDashboardController', loginR:true})
            .when('/companydashboard', {templateUrl: 'views/companyDashboard.html', controller: 'companyDashboardController', loginR:true})
            .when('/missingentries', {templateUrl: 'views/missingEntries.html', controller: 'missingEntriesController', loginR:true})
            .when('/charttest', {templateUrl: 'views/chartTest.html', controller: 'chartTestController', loginR:true})
            .when('/billings', {templateUrl: 'views/billings.html', controller: 'billingsController', loginR:true})

            .otherwise({ redirectTo: '/personalreport' });
        
    }]).run(['$rootScope', '$location', function($rootScope, $location){
        $rootScope.$on('$routeChangeStart', function (event, next, current){
            if (currentUser.id === 0 && next.$$route.loginR){
                $location.path("/login");
            }
        })
    }]);

}());
