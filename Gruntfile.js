module.exports = function (grunt) {
    "use strict";

    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),
        bower: {
            install: {
                options: {
                    install: true,
                    verbose: true,
                    targetDir: "./CameraCar/lib",
                    layout: function (type, component) { return component; }
                }
            }
        },
        tsd: {
            install: {
                options: {
                    command: 'reinstall',
                    latest: false,
                    config: './tsd.json'
                }
            }
        }
    });

    grunt.registerTask("default", []);
    grunt.registerTask("setup", ["bower", "tsd"]);

    grunt.loadNpmTasks("grunt-tsd");
    grunt.loadNpmTasks("grunt-bower-task");
};