app.factory 'Organization', ['$resource', ($resource) ->
  $resource 'api/organization/:id'
]