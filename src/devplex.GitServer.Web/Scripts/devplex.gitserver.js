var devplex = {};
devplex.gitserver = {};

(function ($) {

  $('time').timeago();

  $('#organization').on('change', function () {
    $('.organization-tree').hide();
    
    var selected = $(':selected', this);
    $('#organization_' + selected.text()).show();
  });

})(window.jQuery);