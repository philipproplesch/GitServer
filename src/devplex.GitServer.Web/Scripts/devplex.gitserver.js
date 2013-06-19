var devplex = {};
devplex.gitserver = {};

(function ($) {

  $('time').timeago();

  $('#organization').on('change', function () {
    $('.repositories').hide();
    
    var selected = $(':selected', this);
    $('#organization_' + selected.text()).show();
  });

})(window.jQuery);