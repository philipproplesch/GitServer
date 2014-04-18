module.exports = function(grunt) {

  // Project configuration.
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
	sass: {
      dist: {
        files: [{
          expand: true,
          cwd: 'Content/Styles',
          src: ['*.scss'],
		  dest: 'Content/Styles',
          ext: '.css',
		  extDot: 'last'
        }]
      }
    },
	coffee: {
	  compileWithMaps: {
        options: {
          join: true
        },
        files: {
          'Scripts/devplex.GitServer.js': ['Scripts/**/*.coffee']
        }
      }
	},
	watch: {
      sass: {
        files: ['Content/**/*.scss'],
        tasks: ['sass']
      },
	  coffee: {
        files: ['Scripts/**/*.coffee'],
        tasks: ['coffee']
      }
    }
  });
   
  grunt.loadNpmTasks('grunt-contrib-sass');
  grunt.loadNpmTasks('grunt-contrib-coffee');
  grunt.loadNpmTasks('grunt-contrib-watch');
  
  grunt.file.setBase('src/devplex.GitServer.Web');

  // Default task(s).
  grunt.registerTask('default', ['sass', 'coffee', 'watch']);
};