(($) ->

  $('time').timeago()

  if $('#pjax-container').length == 1
    $(document).pjax 'a', '#pjax-container'

)(window.jQuery)