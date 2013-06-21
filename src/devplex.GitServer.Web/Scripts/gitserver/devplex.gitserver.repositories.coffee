(($) ->

  $('#organization').on 'change', ->

    repositories = $ '#repositories'

    spinner = $ '.spinner', repositories
    spinner.show()

    list = $ 'ul', repositories
    list.children().remove()

    organization = $ ':selected', this
    url = window.fetchRepositoriesUrl + '/' + organization.text()
    
    $.getJSON url, (data) ->
      $.each data, (index, repository) ->
        li = $('<li />')

        if (repository.HasBranches is true)
          a = $('<a />')
          a.attr 'href', repository.Path
          a.text repository.Name
          a.appendTo li
        else
          li.text repository.Name
        
        li.appendTo list
        spinner.hide()

)(window.jQuery)