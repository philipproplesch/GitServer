(($) ->

  $('#organization').on 'change', ->

    list = $ '#repositories ul'
    list.children().remove()

    organization = $ ':selected', this
    url = window.fetchRepositoriesUrl + '/' + organization.text()
    
    $.getJSON url, (data) ->      
      $.each data, (index, repository) ->
        a = $('<a />')
        a.attr 'href', repository.Path
        a.text repository.Name

        li = $('<li />')

        a.appendTo li
        li.appendTo list

)(window.jQuery)