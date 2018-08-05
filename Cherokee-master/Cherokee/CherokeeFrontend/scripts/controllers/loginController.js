(function(){

    var app = angular.module("timeKeeper");

    app.controller("loginController", ["$scope", "$rootScope", "$http", "timeConfig", '$location',
        function($scope, $rootScope, $http, timeConfig, $location) {
            $rootScope.currentUser = currentUser;

          startApp("loginBtn");
            function startApp(actionButton) {
                gapi.load("auth2", function () {
                    auth2 = gapi.auth2.init({
                        client_id: "399815491541-mas0lligt1pm6lkqbkfvhph19vu419t7.apps.googleusercontent.com"
                    });
                    attachSignin(document.getElementById(actionButton));
                });
            };

            function attachSignin(element) {
                auth2.attachClickHandler(element, {}, function (googleUser) {

                    var authToken = googleUser.getAuthResponse().id_token;
                    $http.defaults.headers.common.Authorization = "Bearer " + authToken;
                    $http.defaults.headers.common.Provider = "google";
                    $http({method: "post", url: timeConfig.apiUrl + 'login'})
                        .then(function (response) {
                            currentUser = response.data;
                            $rootScope.currentUser = currentUser;
                            console.log(currentUser);
                            $location.path('/calendar');
                        }, function (error) {
                            window.alert(error.message);
                        });
                })
            };


            $scope.login = function(){
                var userData = {
                    grant_type: 'password',
                    username: $scope.user.name,
                    password: $scope.user.pass,
                    scope: 'openid'
                };
                var urlEncodedUrl = {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'Authorization': 'Basic dGltZWtlZXBlcjokY2gwMGw='
                };

                $http({
                    method: 'POST',
                    url: timeConfig.idsUrl,
                    headers: urlEncodedUrl,
                    data: userData,
                    transformRequest: function (obj) {
                        var str = [];
                        for (var p in obj)
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                        return str.join("&");
                    }
                }).success(function (data, status, headers, config) {
                    authToken = data.access_token;
                    $http.defaults.headers.common.Authorization = 'Bearer ' + authToken;
                    $http.defaults.headers.common.Provider = "iserver";
                    $http({
                        method: 'GET',
                        url: timeConfig.apiUrl + 'login'
                    }).success(function(data, status, headers, config){

                        currentUser = data;
                        console.log(currentUser);
                        $rootScope.currentUser = currentUser;
                        $location.path('/calendar');

                    });
                }).error(function (data, status, headers, config) {
                    console.log('ERROR: ' + status);
                });
            };


        }]);

    app.controller("logoutController", ["$rootScope", "$location", function($rootScope, $location) {
        currentUser = {
            id: 0,
            name: '',
            teams: [],
            provider: '',
            token: ''
        };
        $rootScope.currentUser = currentUser;
        window.location.reload();
        $location.path("/calendar");
        console.log(currentUser);
    }]);
}());

/*
var profile = googleUser.getBasicProfile();
console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
console.log('Name: ' + profile.getName());
console.log('Image URL: ' + profile.getImageUrl());
console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.

console.log(id_token);
*/