
(function($) {
  return $('#organization').on('change', function() {
    var list, organization, repositories, spinner, url;
    repositories = $('#repositories');
    spinner = $('.spinner', repositories);
    spinner.show();
    list = $('ul', repositories);
    list.children().remove();
    organization = $(':selected', this);
    url = window.fetchRepositoriesUrl + '/' + organization.text();
    return $.getJSON(url, function(data) {
      return $.each(data, function(index, repository) {
        var a, li;
        li = $('<li />');
        if (repository.HasBranches === true) {
          a = $('<a />');
          a.attr('href', repository.Path);
          a.text(repository.Name);
          a.appendTo(li);
        } else {
          li.text(repository.Name);
        }
        li.appendTo(list);
        return spinner.hide();
      });
    });
  });
})(window.jQuery);
