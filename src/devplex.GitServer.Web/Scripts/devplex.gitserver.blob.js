(function($) {
  
  devplex.gitserver.Blob = function () {

    if ($('.prettyprint').length > 0 && prettyPrint) {
      prettyPrint();
    }
  };

  var repositoryBlob = new devplex.gitserver.Blob();

}(window.jQuery));