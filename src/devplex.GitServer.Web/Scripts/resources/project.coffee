app.factory 'Project', ['$resource', ($resource) ->
  $resource 'api/project/:id'
]