"use strict";
app.controller("AppCtrl", ["$scope", function ($scope) {
    $scope.lastError = "";
    $scope.errorVisible = false;

    $scope.$watch("lastError", function (newValue) {
        if (newValue) {
            $scope.errorVisible = true;
        } else {
            $scope.errorVisible = false;
        }
    });

    $scope.dismissError = function () {
        $scope.lastError = "";
    };

    $scope.$on("error", function (evt, err) {
        $scope.lastError = err.message || "An error has occurred!";
    });
}]);