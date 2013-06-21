
(function($) {
  var tree, url;
  tree = $('table#tree');
  if (tree.length === 1) {
    url = location.pathname + '?commits=true';
    return $.get(url, function(data) {
      tree.html(data);
      return $('time').timeago();
    });
  }
})(window.jQuery);
