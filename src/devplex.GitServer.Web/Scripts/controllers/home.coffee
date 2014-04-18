app.controller 'HomeCtrl', ['$scope', 'Organization', ($scope, Organization) ->
  $scope.organizations = Organization.query()
]