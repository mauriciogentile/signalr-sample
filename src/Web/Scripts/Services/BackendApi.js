"use strict";

angular
  .module("app.services")
  .service("backendApi", ["$http", "$q", function ($http, $q) {

      var post = function (uri, body) {
          var defer = $q.defer();
          $http.post(uri, body).then(function (response) {
              defer.resolve(response.data);
          }).catch(function (response) {
              defer.reject(new Error(response.data.Message));
          });
          return defer.promise;
      };

      var get = function (uri, options) {
          var defer = $q.defer();
          $http.get(uri, options).then(function (response) {
              defer.resolve(response.data);
          }).catch(function (response) {
              defer.reject(new Error(response.data.Message));
          });
          return defer.promise;
      };

      return {
          city: {
              getAll: function () {
                  return get("api/city/getAll?v=" + new Date().getTime());
              },
              add: function (city) {
                  return post("api/city/post/", city);
              },
              remove: function (id) {
                  return post("api/city/delete/" + id);
              }
          }
      }
      ;
  }]);