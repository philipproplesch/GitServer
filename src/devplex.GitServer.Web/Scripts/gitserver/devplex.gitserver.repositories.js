
(function($) {
  return $('#organization').on('change', function() {
    var list, organization, url;
    list = $('#repositories ul');
    list.children().remove();
    organization = $(':selected', this);
    url = window.fetchRepositoriesUrl + '/' + organization.text();
    return $.getJSON(url, function(data) {
      return $.each(data, function(index, repository) {
        var a, li;
        a = $('<a />');
        a.attr('href', repository.Path);
        a.text(repository.Name);
        li = $('<li />');
        a.appendTo(li);
        return li.appendTo(list);
      });
    });
  });
})(window.jQuery);
