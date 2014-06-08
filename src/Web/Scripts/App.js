"use strict";
var app = angular.module("app", ["ng", "ngRoute", "app.services"]);
angular.module("app.services", []);

var routeProvider = function ($routeProvider) {
    $routeProvider.
        when("/", { templateUrl: "template/get/city-index", controller: "WeatherIndexCtrl" }).
      otherwise({ redirectTo: "/" });
};

app.config(["$routeProvider", routeProvider]);