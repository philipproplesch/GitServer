(function($) {
  
  devplex.gitserver.Blob = function () {

    if ($('.prettyprint').length > 0 && prettyPrint) {
      prettyPrint();
    }
  };

  new devplex.gitserver.Blob();

}(window.jQuery));