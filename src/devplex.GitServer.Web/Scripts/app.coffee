app = angular.module 'GitServer', ['ngRoute', 'ngResource']

app.config ['$routeProvider', ($routeProvider) ->
  
  $routeProvider
    .when '/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeCtrl'
    }
    .when '/:organization/:project', {
      templateUrl: 'templates/project.html',
      controller: 'ProjectCtrl'
    }
    .otherwise {
      redirectTo: '/'
    }
    
  return
]