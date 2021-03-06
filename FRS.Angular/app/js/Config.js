﻿/**=========================================================
 * Module: config.js
 * App routes and resources configuration
 =========================================================*/


(function () {
    'use strict';

    angular
        .module('app.routes')
        .config(routesConfig);

    routesConfig.$inject = ['$stateProvider', '$locationProvider', '$urlRouterProvider', 'RouteHelpersProvider', '$controllerProvider', '$provide'];

    function routesConfig($stateProvider, $locationProvider, $urlRouterProvider, helper, $controllerProvider, $provide) {

        // Set the following to true to enable the HTML5 Mode
        // You may have to set <base> tag in index and a routing configuration in your server
        $locationProvider.html5Mode(false);

        // defaults to dashboard
        $urlRouterProvider.otherwise('/home/index');

        var core = angular.module('app.core');
        // Lazy loading
        core.lazy = {
            controller: $controllerProvider.register,
            factory: $provide.factory,
            service: $provide.service
        }

        //
        // Application Routes
        // -----------------------------------
        $stateProvider
            .state('app', {
                url: '/Aversion',
                abstract: true,
                templateUrl: helper.basepath('../../../../app/views/Aversion.html'),
                resolve: helper.resolveFor('fastclick', 'ng-animate', 'modernizr', 'icons', 'screenfull', 'animo', 'sparklines', 'slimscroll', 'classyloader', 'toaster', 'whirl', 'loaders.css', 'spinkit', 'Utility.module', 'partials.module')
            })
            .state('home', {
                url: '/home',
                templateUrl: helper.basepath('../../../../app/views/static.html'),
                controller: ['$rootScope', '$localStorage', '$scope', function ($rootScope, $localStorage, $scope) {
                        $rootScope.app.layout.isBoxed = false;
                        if ($localStorage['authorizationData'] && $localStorage['authorizationData'].isAuthenticated) {
                            $scope.isAuthenticated = true;
                            $scope.userName = $localStorage['authorizationData'].userName;
                        } else
                            $scope.isAuthenticated = false;
                        $scope.CurrentDate = new Date();
                    }
                ]
            })
            .state('home.index', {
                url: '/index',
                title: 'Home',
                templateUrl: helper.basepath('../../../../app/views/Home/home.html'),
                controller: ['$scope', '$localStorage', '$http', '$rootScope', function ($scope, $localStorage, $http) {
                    if ($localStorage['authorizationData'] && $localStorage['authorizationData'].isAuthenticated) {
                        $scope.isAuthenticated = true;
                        $scope.userName = $localStorage['authorizationData'].userName;
                    } else
                        $scope.isAuthenticated = false;
                    //Logout
                    $scope.logout = function () {
                        $http.post(window.frsApiUrl + "/api/Account/Logout")
                            .success(function () {
                                delete $localStorage['authorizationData'];
                                console.log("LoggedOut");
                                //$.connection.hub.stop();
                                $scope.isAuthenticated = false;
                                $http.defaults.headers.common = {
                                    'Content-Type': 'application/json'
                                };
                                //$state.go('home.index');
                            })
                            .error(function (err) {
                                showErrors(err);
                            });
                    }
                }]
            })
            .state('home.signin', {
                url: '/SignIn',
                title: 'Sign In',
                templateUrl: helper.basepath('../../../../app/views/SignIn/SignIn.html'),
                resolve: helper.resolveFor('flot-chart', 'flot-chart-plugins', 'weather-icons', 'login.module')
            })
            .state('app.dashboard', {
                url: '/dashboard',
                title: 'Dashboard',
                controller: 'DashboardController',
                controllerAs: 'dc',
                templateUrl: helper.basepath('../../../../app/views/Dashboard/Dashboard.html'),
                resolve: helper.resolveFor('flot-chart', 'flot-chart-plugins', 'weather-icons', 'Dashboard.module')
            })
            .state('app.adminDashboard', {
                url: '/admin/dashboard',
                title: 'Admin Dashboard',
                templateUrl: helper.basepath('../../../../app/views/dashboard-admin.html'),
                resolve: helper.resolveFor('flot-chart', 'flot-chart-plugins', 'weather-icons', 'AdminDashboard.module')
            })

            //Users
            .state('app.Users', {
                url: '/Users',
                title: 'Users',
                templateUrl: helper.basepath('../../../../app/views/Users/Users.html'),
                controller: 'UsersController',
                controllerAs: 'uc',
                resolve: helper.resolveFor('ui.select', 'users.module', 'oitozero.ngSweetAlert', 'datatables')
            })

            //Reference data- Allergy
            .state('app.Allergy', {
                url: '/Allergy',
                title: 'Allergy',
                templateUrl: helper.basepath('../../../../app/views/Allergy/Allergy.html'),
                controller: 'AllergyController',
                controllerAs: 'ac',
                resolve: helper.resolveFor('Allergy.module', 'oitozero.ngSweetAlert', 'datatables')
            })
            .state('app.CreateAllergy', {
                url: '/CreateAllergy/:Id',
                title: 'Allergy - Add/Edit',
                templateUrl: helper.basepath('../../../../app/views/Allergy/CreateAllergy.html'),
                controller: 'CreateAllergyController',
                controllerAs: 'clc',
                resolve: helper.resolveFor('oitozero.ngSweetAlert', 'Allergy.module')
            })

            //Student
            .state('app.students', {
                url: '/Students',
                title: 'Students',
                templateUrl: helper.basepath('../../../../app/views/Student/Student.html'),
                controller: 'StudentController',
                controllerAs: 'sc',
                resolve: helper.resolveFor('Student.module', 'oitozero.ngSweetAlert', 'datatables', 'ui.select')
            })

            //Profile
            .state('app.Profile', {
                url: '/Profile/:userName',
                title: 'Profile',
                templateUrl: helper.basepath('../../../../app/views/Users/Profile.html'),
                controller: 'ProfileController',
                controllerAs: 'upc',
                resolve: helper.resolveFor('ui.select', 'profile.module')
            })
            .state('app.changePassword', {
                url: '/changePassword',
                title: 'Change Password',
                templateUrl: helper.basepath('../../../../app/views/Users/ChangePassword.html'),
                controller: 'changePasswordController',
                controllerAs: 'changePassword',
                resolve: helper.resolveFor('changePassword.module')
            })
            .state('app.rightsManagement', {
                url: '/RightsManagement',
                title: 'Rights Management',
                templateUrl: helper.basepath('../../../../app/views/RightsManagement/index.html'),
                controller: 'RightsManagementController',
                controllerAs: 'rightsManagement',
                resolve: helper.resolveFor('rightsManagement.module', 'ui.grid', 'loaders.css', 'spinkit', 'ui.select')
            })

            //Account
            .state('account', {
                url: '/account',
                templateUrl: 'app/pages/page.html',
                resolve: helper.resolveFor('modernizr', 'icons', 'toaster', 'whirl', 'loaders.css', 'spinkit'),
                controller: [
                    '$rootScope', function ($rootScope) {
                        $rootScope.app.layout.isBoxed = false;
                    }
                ]
            })
            .state('account.login', {
                url: '/login?:returnUrl',
                title: 'Login',
                //templateUrl: 'app/pages/login.html'
                templateUrl: 'app/Views/Users/login.html',
                resolve: helper.resolveFor('login.module','oitozero.ngSweetAlert')
            })
            .state('account.register', {
                url: '/register',
                title: 'Register',
                templateUrl: 'app/Views/Users/Register.html',
                resolve: helper.resolveFor('register.module')
            })
            .state('account.recover', {
                url: '/recover',
                title: 'Recover',
                templateUrl: 'app/Views/Users/Recover.html',
                controller: 'ForgotPasswordController',
                controllerAs: 'forgotPassword',
                resolve: helper.resolveFor('recover.module')
            })
            .state('account.lock', {
                url: '/lock',
                title: 'Lock',
                templateUrl: 'app/Views/Users/Lock.html'
            })
            .state('account.404', {
                url: '/404',
                title: 'Not Found',
                templateUrl: 'app/pages/404.html'
            })
            .state('account.ResetPassword', {
                url: '/ResetPassword?:email:code',
                title: 'Reset Password',
                templateUrl: 'app/Views/Users/ResetPassword.html',
                controller: 'ResetPasswordController',
                controllerAs: 'resetPassword',
                resolve: helper.resolveFor('resetPassword.module', 'toaster')
            });


        //
        // CUSTOM RESOLVES
        //   Add your own resolves properties
        //   following this object extend
        //   method
        // -----------------------------------
        // .state('app.someroute', {
        //   url: '/some_url',
        //   templateUrl: 'path_to_template.html',
        //   controller: 'someController',
        //   resolve: angular.extend(
        //     helper.resolveFor(), {
        //     // YOUR RESOLVES GO HERE
        //     }
        //   )
        // })

    } // routesConfig

})();
