"use strict";

angular
  .module("app.services")
  .service("cityEventsHub", function () {
      return (function () {
          return {
              client: $.connection.cityEventsHub.client,
              server: $.connection.cityEventsHub.server,
              connection: $.connection.hub
          };
      })();
  });