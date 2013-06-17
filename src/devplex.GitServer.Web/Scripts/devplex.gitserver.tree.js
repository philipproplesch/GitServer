(function ($) {

  devplex.gitserver.Tree = function () {

    var tree = $('table#tree');
    if (tree.length == 1) {

      var url = location.pathname + '?commits=true';
      $.get(url, function(data) {
        tree.html(data);
        
        // Update times.
        $('time').timeago();
      });
    }
  };

  new devplex.gitserver.Tree();

}(window.jQuery));