(($) ->
  
  tree = $ 'table#tree'

  if (tree.length is 1)
    url = location.pathname + '?commits=true'
    $.get url, (data) ->
      tree.html data
      $('time').timeago()


)(window.jQuery)