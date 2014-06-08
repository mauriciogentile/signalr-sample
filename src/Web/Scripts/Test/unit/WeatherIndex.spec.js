describe('WeatherIndexCtrl', function () {
    var scope, q, httpBackend, _weatherApi, controller, _window, _getByCityOriginal, _cityEventsHub;

    var createController = function () {
        return controller('WeatherIndexCtrl', {
            $scope: scope,
            $window: _window,
            weatherApi: _weatherApi
        });
    };

    //we don't need the websocket
    beforeEach(module(function ($provide) {
        $provide.provider('cityEventsHub', function () {
            this.$get = function () {
                return { client: {}, connection: { start: function () { return { fail: function () { } }; } } };
            };
        });
    }));

    beforeEach(module('app'));

    //dependency injection set up
    beforeEach(inject(function ($controller, $rootScope, $q, $httpBackend, weatherApi, backendApi, cityEventsHub) {
        scope = $rootScope.$new();
        controller = $controller;
        _cityEventsHub = cityEventsHub;
        q = $q;
        _weatherApi = weatherApi;
        _window = {
            alert: function () { }
        };
        httpBackend = $httpBackend;
        $httpBackend.whenGET('api/city/getAll')
        .respond({ id: 1, name: "London", country: "UK" });
    }));

    //mocks configuration for each test
    beforeEach(function () {
        var apiResults = {
            id: 1, name: "London", country: "GB",
            coord: { lon: -0.12574, lat: 51.50853 },
            weather: {
                temp: 290.3, pressure: 1024, temp_min: 288.15, temp_max: 291.48, humidity: 77,
                conditions: "cloudy", icon: ""
            }
        };

        var defer1 = q.defer();
        _getByCityOriginal = _weatherApi.getByCity;
        spyOn(_weatherApi, "getByCity").and.returnValue(defer1.promise);
        defer1.resolve(apiResults);

        spyOn(scope.$root, "$broadcast");
    });

    beforeEach(createController);

    it('should have an empty list of cities', function () {
        expect(scope.cities).toBeDefined();
        expect(scope.cities.length == 0).toBe(true);
    });

    it('should invisible edit form', function () {
        expect(scope.editVisible).toBeDefined();
        expect(scope.editVisible).toBe(false);
    });

    it('should select city when slecect existing id', function () {
        scope.cities = [{ id: 1 }];
        scope.select(1);
        expect(scope.selectedCity).toBeDefined();
    });

    it('should have order direction undefined when loaded', function () {
        expect(scope.orderBy).toBe("weather.temp");
    });

    it('should fetch weather from existing list of cities when loaded', function () {
        var expectedCalls = [];

        //loop for setting up expected results
        scope.cities.forEach(function (city) {
            expectedCalls.push({ name: city.name, country: city.country });
        });

        for (var i = 0; i < expectedCalls.length; i++) {
            expect(_weatherApi.getByCity).toHaveBeenCalledWith(expectedCalls[i]);
        };
    });

    it('should unselect selected if removed', function () {
        scope.$apply = function () { };
        scope.cities = [{ id: 1 }];
        scope.select(1);
        _cityEventsHub.client.cityRemoved({ Payload: 1 });
        expect(scope.selectedCity).toBeUndefined();
        expect(scope.cities.length == 0).toBe(true);
    });

    it('should add city to list if cityAdded', function () {
        scope.$apply = function () { };
        scope.cities = [{ id: 1 }];
        scope.select(1);
        _cityEventsHub.client.cityAdded({ Payload: { id: 2 } });
        expect(scope.cities.length == 2).toBe(true);
    });

    it('should not have completed weather information when loaded', function () {
        scope.select(1);
        expect(scope.selectedCity).toBeUndefined();
    });

    /*it('should broadcast error when api fails', function () {

        //restore original, jasmine raises error if not restored
        _weatherApi.getByCity = _getByCityOriginal;

        var defer = q.defer();
        spyOn(_weatherApi, "getByCity").and.returnValue(defer.promise);
        defer.reject(new Error("Error!"));

        createController();

        scope.cities = [{ id: 1 }];

        httpBackend.flush();

        expect(scope.$root.$broadcast).toHaveBeenCalledWith("error", new Error("Error!"));
        expect(scope.$root.$broadcast.calls.count()).toBe(scope.cities.length);
    });*/
});