
(function($) {
  $('time').timeago();
  if ($('#pjax-container').length === 1) {
    return $(document).pjax('a', '#pjax-container');
  }
})(window.jQuery);
