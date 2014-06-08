"use strict";
app.controller("WeatherIndexCtrl", ["$scope", "weatherApi", "backendApi", "cityEventsHub",
    function ($scope, weatherApi, backendApi, cityEventsHub) {
        $scope.selectedCity = null;

        $scope.select = function (id) {
            var idx = find($scope.cities, "id", id);
            $scope.selectedCity = $scope.cities[idx];
        };

        $scope.editVisible = false;

        //order by is controlled by angular's built-in filters
        $scope.orderBy = "weather.temp";

        //hardcoded list of cities
        $scope.cities = [];

        $scope.load = function () {
            backendApi.city.getAll().then(function (cities) {
                $scope.cities = cities;
            }).catch(function (error) {
                $scope.$root.$broadcast(error);
            });
            cityEventsHub.connection.start().fail(function (err) {
                $scope.$root.$broadcast("error", err);
            });
        };

        $scope.$watchCollection("cities", function () {
            fetchWeatherData();
        });

        $scope.$watch("editVisible", function () {
            $scope.newCityName = "";
            $scope.newCityCountry = "";
            fetchWeatherData();
        });

        $scope.deleteCity = function (id) {
            backendApi.city.remove(id)
                .catch(function (err) {
                    $scope.$root.$broadcast("error", err);
                });
        };

        $scope.addCity = function () {
            $scope.hideEdit();
            weatherApi.getByCity({ name: $scope.newCityName, country: $scope.newCityCountry })
                .then(function (city) {
                    addCity(city);
                }).catch(function (err) {
                    $scope.$root.$broadcast("error", err);
                });
        };

        $scope.showEdit = function () {
            $scope.editVisible = true;
        };

        $scope.hideEdit = function () {
            $scope.editVisible = false;
        };

        $scope.showDelete = function () {
            $scope.deleteVisible = true;
        };

        $scope.hideDelete = function () {
            $scope.deleteVisible = false;
        };

        cityEventsHub.client.cityAdded = function (data) {
            var city = data.Payload;
            if (!city) {
                return;
            }
            var cities = $scope.cities;
            cities.push({ id: city.Id, name: city.Name, country: city.Country });
            $scope.cities = cities;
            $scope.$apply();
        };

        cityEventsHub.client.cityRemoved = function (data) {
            if (!data.Payload) {
                return;
            }
            var idx = find($scope.cities, "id", data.Payload);
            var cities = $scope.cities;
            cities.splice(idx, 1);
            $scope.cities = cities;
            if ($scope.selectedCity && $scope.selectedCity.id == data.Payload) {
                $scope.selectedCity = undefined;
            }
            $scope.$apply();
        };

        var addCity = function (city) {
            backendApi.city.add(city)
                .catch(function (err) {
                    $scope.$root.$broadcast("error", err);
                });
        };

        var fetchWeatherData = function () {
            //loop cities to get more information to be shown on the list
            $scope.cities.forEach(function (city) {
                weatherApi.getByCity(city)
                 .then(function (data) {
                     city.coord = data.coord;
                     city.weather = data.weather;
                 }).catch(function (error) {
                     $scope.$root.$broadcast("error", error);
                 });
            });
        };

        //TODO: this could be in a helper module maybe
        var find = function (input, prop, value) {
            var i = 0, len = input.length;
            for (; i < len; i++) {
                if (input[i][prop] == value) {
                    return i;
                }
            }
            return -1;
        };

        $scope.load();
    }]);