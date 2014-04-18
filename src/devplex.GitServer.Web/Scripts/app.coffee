app = angular.module 'GitServer', ['ngRoute', 'ngResource']

app.config ['$routeProvider', ($routeProvider) ->
  $routeProvider
    .when '/', {
      templateUrl: 'templates/home.html',
      controller: 'HomeCtrl'
    }
    
  return
]