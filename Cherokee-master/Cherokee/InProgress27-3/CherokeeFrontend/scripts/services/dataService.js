(function () {
    var app = angular.module("timeKeeper");

    app.factory("dataService", ['$http', 'timeConfig', function ($http, timeConfig) {
            var source = timeConfig.apiUrl;

            return {
                list: function (dataSet, callback) {
                    $http.get(source + dataSet).then(
                        function(response) {
                            return callback(response.data, response.headers);
                        },
                        function(reason) {
                            window.alert(reason.data.message);
                        });
                },

                read: function (dataSet, id, callback) {
                    $http.get(source + dataSet + "/" + id)
                        .then(function success(response) {
                            return callback(response.data);
                        }, function error(error) {
                            window.alert(error.data.message);
                        });
                },

                insert: function (dataSet, data, callback) {
                    $http({ method: "post", url: source + dataSet, data: data })
                        .then(function success(response) {
                            return callback(response.data);
                        }, function error(error) {
                            window.alert(error.data.message);
                        });
                },

                update: function (dataSet, id, data, callback) {
                    $http({ method: "put", url: source + dataSet + "/" + id, data: data })
                        .then(function success(response) {
                            return callback(response.data);
                        }, function error(error) {
                            window.alert(error.data.message);
                        });
                },

                delete: function (dataSet, id, callback) {
                    $http({ method: "delete", url: source + dataSet + "/" + id })
                        .then(function success(response) {
                            return callback(response.data);
                        }, function error(error) {
                            window.alert(error.data.message);
                        });
                }
            };
        }]);
}());