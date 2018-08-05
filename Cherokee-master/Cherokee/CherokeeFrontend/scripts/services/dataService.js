(function () {
    var app = angular.module("timeKeeper");

    app.factory("dataService", ['$rootScope', '$http', 'timeConfig', 'infoService', function ($rootScope, $http, timeConfig, infoService) {
        var source = timeConfig.apiUrl;
        function setLoader(flag){
            $rootScope.waitForLoad = flag;
        }

        return {
            list: function (dataSet, callback) {
                setLoader(true);
                $http.get(source + dataSet).then(
                    function(response) {
                        setLoader(false);
                        //infoService.success(dataSet,  "data successfully retrieved" );
                        return callback(response.data, response.headers);
                    },
                    function(reason) {
                        setLoader(false);
                        infoService.error(dataSet, message);
                    });
            },

            read: function (dataSet, id, callback) {
                setLoader(true);
                $http.get(source + dataSet + "/" + id)
                    .then(function success(response) {
                        setLoader(false);
                        //infoService.success(dataSet, "data successfully retrieved" );
                        return callback(response.data);
                    }, function error(error) {
                        setLoader(false);
                        infoService.error(dataSet, message);
                    });
            },

            insert: function (dataSet, data, callback) {
                setLoader(true);
                $http({ method: "post", url: source + dataSet, data: data })
                    .then(function success(response) {
                        setLoader(false);
                        infoService.success(dataSet, "data successfully inserted" );
                        return callback(response.data);
                    }, function error(error) {
                        setLoader(false);
                        infoService.error(dataSet, message);
                    });
            },

            update: function (dataSet, id, data, callback) {
                setLoader(true);
                $http({ method: "put", url: source + dataSet + "/" + id, data: data })
                    .then(function success(response) {
                        setLoader(false);
                        infoService.success(dataSet, "data successfully updated" );
                        return callback(response.data);
                    }, function error(error) {
                        setLoader(false);
                        infoService.error(dataSet, message);
                    });
            },

            delete: function (dataSet, id, callback) {
                setLoader(true);
                $http({ method: "delete", url: source + dataSet + "/" + id })
                    .then(function success(response) {
                        setLoader(false);
                        infoService.warning("are yA SURE?", "data successfully deleted" );
                        return callback(response.data);
                    }, function error(error) {
                        setLoader(false);
                        infoService.error(dataSet, message);
                    });
            }
        };
    }]);
}());