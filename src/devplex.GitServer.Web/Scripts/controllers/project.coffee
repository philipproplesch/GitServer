app.controller 'ProjectCtrl', ['$scope', '$routeParams', 'Project', ($scope, $routeParams, Project) ->
  console.log $routeParams
  $scope.project = Project.get({  })
]