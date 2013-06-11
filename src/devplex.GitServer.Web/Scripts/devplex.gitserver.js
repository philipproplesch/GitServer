(function($) {
  
  // Apply syntax highlighting.
  if ($('.prettyprint').length > 0 && prettyPrint) {
    prettyPrint();
  }
  
  $(function () {
    $('time').timeago();
  });

})(jQuery);