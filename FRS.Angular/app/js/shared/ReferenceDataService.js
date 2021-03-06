﻿/**=========================================================
 * Module: ReferenceDataService
 * ReferenceDataService
 =========================================================*/

(function () {
    'use strict';

    var core = angular.module('app.core');
    // ReSharper disable FunctionsUsedBeforeDeclared
    core.lazy.service('ReferenceDataService', ReferenceDataService);

    ReferenceDataService.$inject = ['$http', 'toaster'];

    function ReferenceDataService($http, toaster) {

        var sharedData = {};

        this.setSharedData = function(value) {
            sharedData = value;
        }

        this.getSharedData = function () {
            return sharedData;
        }

        this.url = "";

        this.getAll = function (onReady, onError, url) {
            if (!url)
                url = this.url;

            onError = onError || function (response) {
                if (response.ExceptionMessage) {
                    toaster.error(response.ExceptionMessage);
                }if (response.Message) {
                    toaster.error(response.Message);
                }
            };
            $http
              .get(frsApiUrl + url)
              .success(onReady)
              .error(onError);
        }

        this.loadById = function (Id, onReady, onError) {
            var urlMetaData = frsApiUrl + this.url + Id;

            onError = onError || function () { alert('Failure loading Data'); };

            $http
              .get(urlMetaData)
              .success(onReady)
              .error(onError);
        };

        this.load = function (url, data, onReady, onError) {
            if (!url)
                url = this.url;

            onError = onError || function (response) {
                if (response.ExceptionMessage) {
                    toaster.error(response.ExceptionMessage);
                } if (response.Message) {
                    toaster.error(response.Message);
                }
            };

            $http
              .get(frsApiUrl + url, {
                  params: data
              })
              .success(onReady)
              .error(onError);
        };

        this.delete = function (Id, onReady, onError) {
            var urlMetaData = frsApiUrl + this.url + Id;

            onError = onError || function (response) {
                if (response.ExceptionMessage) {
                    toaster.error(response.ExceptionMessage);
                };
                if(response.Message)
                    toaster.error(response.Message);
            };

            $http
              .delete(urlMetaData)
              .success(onReady)
              .error(onError);
        };

        this.save = function (data, onReady, onError, url) {
            if (!url)
                url = this.url;
            //var urlMetaData = window.frsApiUrl + '/api/LoadMetaData';

            onError = onError || function (response) {
                if (response.ExceptionMessage) {
                    toaster.error('Error', response.ExceptionMessage);
                } else {
                    toaster.error('Error', response.data.ExceptionMessage);
                }
            };

            $http(
                {
                    method: 'POST',
                    url: frsApiUrl + url,
                    data: JSON.stringify(data)
                }
              ).then(onReady, onError);
        }

        this.retrieveItems = function (url, val, id) {
            return $http.get(frsApiUrl + url, {
                params: {
                    name: val,
                    studentId : id
                }
            });
        }
    }
})();